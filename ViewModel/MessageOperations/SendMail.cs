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
        public void Send(MailWindow Page, User user, ConfigModel cfg, string to, string subject,string msg)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(user.Mail));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;
            var Body = new BodyBuilder();
            Body.HtmlBody = msg;
            foreach (var attachment in Message.AttachmentList)
            {
                Body.Attachments.Add(attachment);
            }
            message.Body = Body.ToMessageBody();

            using( SmtpClient client = new SmtpClient())
            {
                try
                {
                    client.Connect(cfg.SmtpServer, cfg.SmtpPort);
                    client.Authenticate(user.Mail, user.Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                catch(MailKit.Net.Smtp.SmtpCommandException)
                {
                    info inf = new info("Message Could not be Sent \n\r Maybe you are trying to send to much attachments");
                    inf.ShowDialog();
                }
               
            }
        }
    }
}
