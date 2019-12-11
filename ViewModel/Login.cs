using MailClient.Model;
using MailClient.View;
using MailKit.Net.Imap;
using MimeKit;
using System;


namespace MailClient.ViewModel
{
    internal class Login
    {
        
        public async void ToMail(User user, MainWindow loginpage)
        {
            //MimeMessage message = new MimeMessage();
            //message.From.Add(new MailboxAddress("XDDDDDDDDDD", user.Mail));
            //message.To.Add(new MailboxAddress("ganit26@gmail.com"));
            //message.Subject = "Hi";
            //message.Body = new TextPart("plain")
            //{
            //    Text = "Hey there bi"
            //};
            ConfigModel Config = MainWindow.Config;
            using (ImapClient client = new ImapClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(Config.ImapServer, Config.ImapPort);
                    client.Authenticate(user.Mail, user.Password);
                    loginpage.Content = new Inbox(user,Config);

                }
                catch (Exception)
                {
                    loginpage.Info.Text = "Cannot Log-in Check Configuration";
                }

            }

        }
    }
}
