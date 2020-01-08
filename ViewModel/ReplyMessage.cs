using MailClient.Model;
using MailClient.View;
using MailKit.Net.Imap;
using MimeKit;
using System.Linq;
using System.Windows;

namespace Model
{
    internal class ReplyMessage
    { 
        public void Reply(Inbox inbox, Message messageToReply,User user, ConfigModel config)
        {
            inbox.MailBody.Visibility = (Visibility)2;
            inbox.NewMessage.Visibility = (Visibility)0;
            using (var client = new ImapClient()) {
                client.Connect(config.ImapServer, config.ImapPort);
                client.Authenticate(user.Mail,user.Password);
                var folder = client.Inbox;
                folder.Open( MailKit.FolderAccess.ReadOnly);
                var body = folder.Fetch(new[] { messageToReply.ID },MailKit.MessageSummaryItems.UniqueId| MailKit.MessageSummaryItems.BodyStructure);
               var htmlbody= folder.GetBodyPart(body.First().UniqueId, body.First().HtmlBody);
                inbox.SendBody.Text ="<div style='background:gray;'>"+ ((MimeKit.TextPart)htmlbody).Text + "</div>";
            }
            
            inbox.Subject.Text ="RE: "+ messageToReply.Subject;
            inbox.To.Text = messageToReply.From;
        }
    }
}