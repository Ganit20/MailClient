using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MailClient
{
    class Encryption
    {
        public byte[] Encrypt(string original)
        {

            using (Aes myAes = Aes.Create())
            {

                byte[] encrypted = EncryptStringToBytes_Aes(original);
                return encrypted;
            }
        }
        public string Decrypt(byte[] message)
        {
            using (Aes myAes = Aes.Create())
            {

                string decrypted = DecryptStringFromBytes_Aes(message);
                return decrypted;

            }
        }


        private string DecryptStringToBytes_Aes(byte[] message)
        {
            throw new NotImplementedException();
        }

        static byte[] EncryptStringToBytes_Aes(string Text)
        {
            var parkey = "48206829429835727543309257482014";
            var Vector = "8532744676923527";
            using (var aesManag = Aes.Create())
            {
                aesManag.KeySize = 256;

            }
            if (Text == null || Text.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (parkey == null || parkey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (Vector == null || Vector.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(parkey);
                aesAlg.IV = System.Text.Encoding.UTF8.GetBytes(Vector);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(Text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText)
        {
            var parkey = "48206829429835727543309257482014";
            var Vector = "8532744676923527";
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (parkey == null || parkey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (Vector == null || Vector.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(parkey);
                aesAlg.IV = System.Text.Encoding.UTF8.GetBytes(Vector);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            try
                            {

                                plaintext = srDecrypt.ReadToEnd();
                                Debug.WriteLine(plaintext);
                            }
                            catch (System.Security.Cryptography.CryptographicException e)
                            {
                                Debug.WriteLine(e);
                            }
                        }
                    }

                }

                return plaintext;

            }
        }
    }
}