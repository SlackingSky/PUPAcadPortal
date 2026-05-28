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
            CloudName = Encryptor.Decrypt(_cloudName);
            CloudKey = Encryptor.Decrypt(_cloudKey);
            CloudSecret = Encryptor.Decrypt(_cloudSecret);
        }
    }
}
