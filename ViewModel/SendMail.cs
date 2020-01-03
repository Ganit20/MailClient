using MailClient.Model;
using MailClient.View;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailClient.ViewModel
{
    class SendMail
    {
        public void Send(Inbox Page, User user, ConfigModel cfg, string to, string subject,string msg)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(user.Mail));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;
            var Body = new BodyBuilder();
            Body.HtmlBody = msg;
            message.Body = Body.ToMessageBody();

            using( SmtpClient client = new SmtpClient())
            {
                client.Connect(cfg.SmtpServer, cfg.SmtpPort);
                client.Authenticate(user.Mail, user.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
