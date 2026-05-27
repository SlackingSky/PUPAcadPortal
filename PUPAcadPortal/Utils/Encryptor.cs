using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace PUPAcadPortal.Utils
{
    internal class Encryptor
    {
        // Encrypts using AES (Advanced Encryption Standard) with a 256-bit key and a 128-bit IV (Initialization Vector).
        // The key must be EXACTLY 32 characters long for AES-256
        private static readonly string _encryptionKey = "1PUPAcadPortal.2weeksmergeishard";

        // The IV (Initialization Vector) must be EXACTLY 16 characters long
        private static readonly string _iv = "3monthsfrontend?";

        public static string Encrypt(string plainText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_encryptionKey);
            byte[] ivBytes = Encoding.UTF8.GetBytes(_iv);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    // Here is where it converts the encrypted bytes into a Base64 string
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_encryptionKey);
            byte[] ivBytes = Encoding.UTF8.GetBytes(_iv);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // Here it reads the Base64 string from App.config and turns it back into encrypted bytes
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    // Unlocks the safe and returns your raw database password
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
