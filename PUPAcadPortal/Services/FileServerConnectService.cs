using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using PUPAcadPortal.Utils;

namespace PUPAcadPortal.Services
{
    internal class FileServerConnectService
    {
        private static string _cloudName = ConfigurationManager.AppSettings["CloudinaryCloudName"] ?? "None";
        private static string _cloudKey = ConfigurationManager.AppSettings["CloudinaryApiKey"] ?? "None";
        private static string _cloudSecret = ConfigurationManager.AppSettings["CloudinaryApiSecret"] ?? "None";
        private static bool _credentialsDecrypted = false;

        public static string? CloudName 
        { 
            get; private set; 
        }
        public static string? CloudKey 
        { 
            get; private set; 
        }
        public static string? CloudSecret 
        { 
            get; private set; 
        }

        public async static Task GetDecryptedCredentialsAsync()
        {
            if (!_credentialsDecrypted)
            {
                CloudName = Encryptor.Decrypt(_cloudName);
                CloudKey = Encryptor.Decrypt(_cloudKey);
                CloudSecret = Encryptor.Decrypt(_cloudSecret);
                _credentialsDecrypted = true;
            }
            await Task.CompletedTask;
        }

        public async static Task<byte[]> EncryptFileAsync(byte[] fileData)
        {
            return await Task.Run(() =>
            {
                // Convert bytes to base64 string so we can use Encrypt(string)
                // Dont remove this pls, it is necessary for the encryption and decryption to work properly
                var plainBase64 = Convert.ToBase64String(fileData);
                var encryptedString = Encryptor.Encrypt(plainBase64);
                return Encoding.UTF8.GetBytes(encryptedString);
            });
        }

        public async static Task<byte[]> DecryptFileAsync(byte[] encryptedData)
        {
            return await Task.Run(() =>
            {
                var encryptedString = Encoding.UTF8.GetString(encryptedData);
                var plainBase64 = Encryptor.Decrypt(encryptedString);
                return Convert.FromBase64String(plainBase64);
            });
        }

        public async static Task<string> EncryptFileNameAsync(string fileName)
        {
            return await Task.Run(() => Encryptor.Encrypt(fileName));
        }

        public async static Task<string> DecryptFileNameAsync(string encryptedName)
        {
            return await Task.Run(() => Encryptor.Decrypt(encryptedName));
        }
    }
}
