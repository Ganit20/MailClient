using MailClient.Model;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MailClient.ViewModel
{
    internal class RememberMe
    {
        public void Save(User user)
        {
            string UserData = JsonConvert.SerializeObject(user);
            byte[] EncryptedUserData = new Encryption().Encrypt(UserData);
            File.WriteAllBytes("usd.data", EncryptedUserData);
        }
        public User Load()
        {
            try
            {
                byte[] data = File.ReadAllBytes("usd.data");
                string DecryptedData = new Encryption().Decrypt(data);
                User User = JsonConvert.DeserializeObject<User>(DecryptedData);
                return User;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
