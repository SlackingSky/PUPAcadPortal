using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MySqlConnector;

namespace PUPAcadPortal
{
    internal class DBConnect
    {
        private static string connectionString = "U2VydmVyPW15c3FsLXB1cGFjYWRwb3J0YWwtcHVwYWNhZHBvcnRhbC5oLmFpdmVuY2xvdWQuY29tO1BvcnQ9MTUyMDQ7RGF0YWJhc2U9ZGVmYXVsdGRiO1VpZD1wdXBhY2FkcG9ydGFsYXBwO1B3ZD1wdXBhY2FkcG9ydGFsYXBwO1NzbE1vZGU9UmVxdWlyZWQ7";

        public static string GetDecodedConnectionString()

        {
            byte[] data = Convert.FromBase64String(connectionString);
            return Encoding.UTF8.GetString(data);
        }
    }
}
