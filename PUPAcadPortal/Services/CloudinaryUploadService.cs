using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.Services
{
    
    public class CloudinaryUploadService
    {
        private Cloudinary _cloudinary;

        public CloudinaryUploadService()
        {

        }

       

        private async Task InitializeCloudinaryAsync()
        {
            if (_cloudinary != null)
                return;

            try
            {
                await FileServerConnectService.GetDecryptedCredentialsAsync();

                string cloudName = FileServerConnectService.CloudName;
                string apiKey = FileServerConnectService.CloudKey;
                string apiSecret = FileServerConnectService.CloudSecret;

                if (string.IsNullOrEmpty(cloudName) ||
                    string.IsNullOrEmpty(apiKey) ||
                    string.IsNullOrEmpty(apiSecret))
                {
                    System.Diagnostics.Debug.WriteLine("Cloudinary credentials not configured.");
                    return;
                }

                _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
                System.Diagnostics.Debug.WriteLine("Cloudinary initialised successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialise Cloudinary: {ex.Message}");
            }
        }

        // Upload the file at sourceFilePath to Cloudinary after encrypting it.
        public async Task<string> UploadEncryptedFileAsync(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath) || !File.Exists(sourceFilePath))
            {
                System.Diagnostics.Debug.WriteLine($"Source file not found: {sourceFilePath}");
                return null;
            }

            string tempEncryptedPath = null;

            try
            {
                await InitializeCloudinaryAsync();

                if (_cloudinary == null)
                {
                    System.Diagnostics.Debug.WriteLine("Cloudinary not initialised — credentials may be missing.");
                    return null;
                }

                // 1. Read original bytes
                byte[] originalBytes = await File.ReadAllBytesAsync(sourceFilePath);

                // 2. Encrypt
                byte[] encryptedBytes = await FileServerConnectService.EncryptFileAsync(originalBytes);

                // 3. Write to a temp file  (keep original extension so Cloudinary stores it cleanly)
                string originalExt = Path.GetExtension(sourceFilePath);
                tempEncryptedPath = Path.Combine(Path.GetTempPath(),
                                        $"enc_{Guid.NewGuid():N}{originalExt}");
                await File.WriteAllBytesAsync(tempEncryptedPath, encryptedBytes);

                // 4. Upload encrypted temp file
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(tempEncryptedPath),
                    Folder = "pup_announcements",
                    // Tag so we can identify encrypted files later if needed
                    Tags = "encrypted"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Encrypted file uploaded: {result.SecureUrl}");
                    return result.SecureUrl.AbsoluteUri;
                }

                System.Diagnostics.Debug.WriteLine(
                    $"Cloudinary upload failed: {result.Error?.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"UploadEncryptedFileAsync error: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
            finally
            {
                // 5. Always clean up the temp file
                if (tempEncryptedPath != null && File.Exists(tempEncryptedPath))
                {
                    try { File.Delete(tempEncryptedPath); } catch { /* ignore */ }
                }
            }
        }

        // <summary>
        // Legacy upload without encryption — kept for backward compatibility.
        // New code should call <see cref="UploadEncryptedFileAsync"/> instead.
        // </summary>
        public async Task<string> UploadFileAsync(string sourceFilePath)
            => await UploadEncryptedFileAsync(sourceFilePath);

        // Download and decrypt the file from Cloudinary URL, then save to destination path.

        
        public static async Task<bool> DownloadDecryptedFileAsync(
            string cloudinaryUrl,
            string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(cloudinaryUrl))
            {
                System.Diagnostics.Debug.WriteLine("DownloadDecryptedFileAsync: URL is empty.");
                return false;
            }

            try
            {
                // 1. Fetch encrypted bytes from Cloudinary
                byte[] encryptedBytes;
                using (var http = new HttpClient())
                {
                    encryptedBytes = await http.GetByteArrayAsync(cloudinaryUrl);
                }

                // 2. Decrypt
                byte[] originalBytes =
                    await FileServerConnectService.DecryptFileAsync(encryptedBytes);

                // 3. Ensure destination directory exists
                string destDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(destDir))
                    Directory.CreateDirectory(destDir);

                // 4. Write decrypted bytes
                await File.WriteAllBytesAsync(destinationPath, originalBytes);

                System.Diagnostics.Debug.WriteLine(
                    $"File decrypted and saved to: {destinationPath}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"DownloadDecryptedFileAsync error: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }
    }
}