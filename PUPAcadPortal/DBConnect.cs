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

        public static bool VerifyUserLogin(string username, string plainTextPassword)
        {
            // I used parameterized query to prevent SQL injection
            string query = "SELECT PasswordHash FROM Users WHERE Username = @username LIMIT 1;";
            string storedHash = null;

            try
            {
                using (var conn = new MySqlConnection(GetDecodedConnectionString()))
                {
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                storedHash = reader.GetString("PasswordHash");
                            }
                        }
                    }
                }

                if (storedHash == null) return false;

                return BCrypt.Net.BCrypt.Verify(plainTextPassword, storedHash);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Database authentication error: {ex.Message}");
                return false;
            }
        }

        public static void AddUser(string username, string plainTextPassword)
        {
            string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@username, @password_hash);";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
            string loweredUsername = username.ToLower();
            try
            {
                using (var conn = new MySqlConnection(GetDecodedConnectionString()))
                {
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", loweredUsername);
                        cmd.Parameters.AddWithValue("@password_hash", passwordHash);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Database error: {ex.Message}");
            }
        }
    }
}
