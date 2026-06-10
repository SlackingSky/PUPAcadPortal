using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Net.Http;

namespace PUPAcadPortal.Services
{
    public class CloudinaryService
    {
        private static CloudinaryService? _instance;

        public static CloudinaryService Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException(
                        "CloudinaryService has not been initialised. " +
                        "Call CloudinaryService.Initialise() after credentials are decrypted.");
                return _instance;
            }
        }

        public static void Initialise()
        {
            string? cloudName = FileServerConnectService.CloudName;
            string? apiKey = FileServerConnectService.CloudKey;
            string? apiSecret = FileServerConnectService.CloudSecret;

            if (string.IsNullOrWhiteSpace(cloudName) ||
                string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(apiSecret))
                throw new InvalidOperationException(
                    "Cloudinary credentials are missing. " +
                    "Check CloudinaryCloudName / CloudinaryApiKey / CloudinaryApiSecret in app.config.");

            var account = new Account(cloudName, apiKey, apiSecret);
            _instance = new CloudinaryService(new Cloudinary(account));
        }

        private readonly Cloudinary _cld;
        private CloudinaryService(Cloudinary cld) => _cld = cld;

        //  CREATE — upload a local file and return its secure URL

        public string UploadFile(string localPath, string folder, string? publicIdHint = null)
        {
            if (!File.Exists(localPath))
                throw new FileNotFoundException($"File not found: {localPath}");

            string ext = Path.GetExtension(localPath).ToLowerInvariant();

            // Build a stable public_id for future replace / delete
            string stem = string.IsNullOrWhiteSpace(publicIdHint)
                ? Guid.NewGuid().ToString("N")
                : SanitisePublicId(publicIdHint);
            string publicId = $"{SanitisePublicId(folder)}/{stem}";

            if (IsRawType(ext))
            {
                // Raw resource type for documents (PDF, DOCX, PPTX, XLS…)
                var p = new RawUploadParams
                {
                    File = new FileDescription(localPath),
                    PublicId = publicId,
                    Overwrite = false,
                    UniqueFilename = false,
                    UseFilename = false,
                };
                var r = _cld.Upload(p);
                if (r.Error != null)
                    throw new Exception($"Cloudinary upload error: {r.Error.Message}");
                return r.SecureUrl.ToString();
            }
            else
            {
                // Image / video — use the generic Upload overload
                var p = new ImageUploadParams
                {
                    File = new FileDescription(localPath),
                    PublicId = publicId,
                    Overwrite = false,
                    UniqueFilename = false,
                    UseFilename = false,
                };
                var r = _cld.Upload(p);
                if (r.Error != null)
                    throw new Exception($"Cloudinary upload error: {r.Error.Message}");
                return r.SecureUrl.ToString();
            }
        }

        //  READ — get a secure URL for an existing public_id (no network call)

        public string GetSecureUrl(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return string.Empty;
            return _cld.Api.UrlImgUp.BuildUrl(publicId);
        }

        //  UPDATE — replace an existing asset (same public_id → same URL)

        public string ReplaceFile(string localPath, string existingPublicId)
        {
            if (!File.Exists(localPath))
                throw new FileNotFoundException($"File not found: {localPath}");

            string ext = Path.GetExtension(localPath).ToLowerInvariant();

            if (IsRawType(ext))
            {
                var p = new RawUploadParams
                {
                    File = new FileDescription(localPath),
                    PublicId = existingPublicId,
                    Overwrite = true,
                    UniqueFilename = false,
                    UseFilename = false,
                };
                var r = _cld.Upload(p);
                if (r.Error != null)
                    throw new Exception($"Cloudinary replace error: {r.Error.Message}");
                return r.SecureUrl.ToString();
            }
            else
            {
                var p = new ImageUploadParams
                {
                    File = new FileDescription(localPath),
                    PublicId = existingPublicId,
                    Overwrite = true,
                    UniqueFilename = false,
                    UseFilename = false,
                };
                var r = _cld.Upload(p);
                if (r.Error != null)
                    throw new Exception($"Cloudinary replace error: {r.Error.Message}");
                return r.SecureUrl.ToString();
            }
        }

        //  DELETE — remove an asset from Cloudinary

        public void DeleteFile(string publicId, bool isRaw = true)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return;

            var p = new DeletionParams(publicId)
            {
                ResourceType = isRaw ? ResourceType.Raw : ResourceType.Image,
            };
            _cld.Destroy(p);   // result ignored — deletion is best-effort
        }

        /// <summary>
        /// Delete an asset by its full Cloudinary HTTPS URL.
        /// Extracts the public_id automatically.
        /// </summary>
        public void DeleteByUrl(string secureUrl, bool isRaw = true)
        {
            string pid = ExtractPublicIdFromUrl(secureUrl);
            if (!string.IsNullOrEmpty(pid))
                DeleteFile(pid, isRaw);
        }

        //  DOWNLOAD — stream a Cloudinary asset to a local temp file

        public string DownloadToTemp(string secureUrl, string originalFileName)
        {
            if (string.IsNullOrWhiteSpace(secureUrl))
                throw new ArgumentException("URL is empty.", nameof(secureUrl));

            string tempDir = Path.Combine(Path.GetTempPath(), "PUPAcadPortal");
            Directory.CreateDirectory(tempDir);

            string ext = Path.GetExtension(originalFileName);
            string baseName = Path.GetFileNameWithoutExtension(originalFileName);
            string tempPath = Path.Combine(tempDir,
                $"{baseName}_{Guid.NewGuid():N[..8]}{ext}");

            using var http = new HttpClient();
            using var stream = http.GetStreamAsync(secureUrl).GetAwaiter().GetResult();
            using var fs = File.Create(tempPath);
            stream.CopyTo(fs);

            return tempPath;
        }

        public static string ExtractPublicIdFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            try
            {
                const string marker = "/upload/";
                int idx = url.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
                if (idx < 0) return string.Empty;

                string rest = url[(idx + marker.Length)..];

                // Skip optional version segment (v1234567890/)
                if (rest.Length > 1 && rest[0] == 'v' && char.IsDigit(rest[1]))
                {
                    int slash = rest.IndexOf('/');
                    if (slash >= 0) rest = rest[(slash + 1)..];
                }

                // Strip file extension for image resources (Cloudinary public_ids have no ext)
                string ext = Path.GetExtension(rest).ToLowerInvariant();
                if (!IsRawType(ext) && !string.IsNullOrEmpty(ext))
                    rest = Path.ChangeExtension(rest, null);

                return rest;
            }
            catch { return string.Empty; }
        }

        public static bool IsRawType(string ext) =>
            ext.TrimStart('.').ToLowerInvariant() is
                "pdf" or "doc" or "docx" or "ppt" or "pptx" or
                "xls" or "xlsx" or "txt" or "csv" or "zip" or "rar" or "7z";

        private static string SanitisePublicId(string raw) =>
            System.Text.RegularExpressions.Regex
                .Replace(raw.Trim(), @"[^a-zA-Z0-9_\-/]", "_")
                .Trim('/');
    }
}