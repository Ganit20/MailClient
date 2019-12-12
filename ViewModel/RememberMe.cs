using MailClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MailClient.ViewModel
{
    class RememberMe
    {
        public void Save(User user)
        {
            var UserData = JsonConvert.SerializeObject(user);
            var EncryptedUserData = new Encryption().Encrypt(UserData);
            File.WriteAllBytes("usd.data",EncryptedUserData);
   
            
        }
        public User Load()
        {
            try
            {
                var data = File.ReadAllBytes("usd.data");
                var DecryptedData = new Encryption().Decrypt(data);
                var User = JsonConvert.DeserializeObject<User>(DecryptedData);
                return User;
            } catch(Exception)
            {
                return null;
            }
        }
    }
}
