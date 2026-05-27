using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MySqlConnector;
using PUPAcadPortal.Utils;

namespace PUPAcadPortal.Services
{
    internal class DBConnectService
    {
        private static string _connectionString = ConfigurationManager.AppSettings["EncryptedConnectionString"] ?? "None";

        public static string? ConnectionString
        {
            get; private set;
        }

        public async static Task GetDecryptedConnectionStringAsync()
        {
            ConnectionString = Encryptor.Decrypt(_connectionString);
        }
    }
}
