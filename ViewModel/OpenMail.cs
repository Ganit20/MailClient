using MailClient.Model;
using MailClient.View;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MailClient.ViewModel
{
    class OpenMail
    {
        public async Task OpenText(Message m, Inbox inboxPage, User user, ConfigModel conf)
        {
            using (ImapClient client = new ImapClient())
            {
                List<string> atc = new List<string>();
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                IMailFolder Folder = client.GetFolder(ListMessages.fold);
                await Folder.OpenAsync(FolderAccess.ReadWrite);
                IList<IMessageSummary> msg = Folder.Fetch(new[] { m.ID }, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Envelope);
                var bodyHTML = (TextPart)Folder.GetBodyPart(msg.First().UniqueId, msg.First().HtmlBody);
                inboxPage.Attachments.Children.Clear();
                Folder.SetFlags(m.ID, MessageFlags.Seen, true);
                m.MessageColor = new SolidColorBrush(Colors.White);
                foreach (BodyPartBasic attachment in msg.First().Attachments)
                {
                    Button bt = new Button
                    {
                        Content = attachment.FileName
                    };
                    bt.Click += new RoutedEventHandler(OpenAttachment);
                    inboxPage.Attachments.Children.Add(bt);
                }

                if (m != null)
                {
                    inboxPage.Info.Text = ($"Subject: {msg.First().Envelope.Subject} \n\rFrom {msg.First().Envelope.From} at {msg.First().Envelope.Date} to {msg.First().Envelope.To}");
                    inboxPage.Body.NavigateToString("<html><head><meta charset='UTF-8'></head>" + bodyHTML.Text + "</html>");
                    inboxPage.MailBody.Visibility = 0;
                    inboxPage.Back.Visibility = (Visibility)0;
                    inboxPage.SelectAll.Visibility = (Visibility)2;
                }
                client.Disconnect(true);
            }

        }
        private void OpenAttachment(object sender, RoutedEventArgs e)
        {

        }
    }
}
