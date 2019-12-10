using MailClient.Model;
using MailClient.View;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace MailClient.ViewModel
{
    class Login
    {
        public async void ToMail(User user,MainWindow loginpage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("XDDDDDDDDDD", user.Mail));
            message.To.Add(new MailboxAddress("ganit26@gmail.com"));
            message.Subject = "Hi";
            message.Body = new TextPart("plain")
            {
                Text = "Hey there bi"
            };
            using (var client = new ImapClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync("imap.poczta.onet.pl", 993);
                    client.Authenticate(user.Mail, user.Password);
                    
                    loginpage.Content = new Inbox(client);
                    
                }
                catch (Exception e)  
                {
                    loginpage.Info.Text = "Cannot Log-in Check Configuration";
                }
                
            }
                
            }
    }
}
