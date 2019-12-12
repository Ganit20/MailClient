using MailClient.Model;
using MailClient.View;
using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailClient.ViewModel
{
    internal class ListMessages
    {
        public static int loaded = 0;
        public static int load = 26;
        public static string fold = "Inbox";
        public async Task DownloadMessagesAsync(User user, ConfigModel conf, Inbox inboxPage,IMailFolder Folder=null)
        {
            using (ImapClient client = new ImapClient())
            {
                
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                    if(Folder!=null)
                {
                    fold = Folder.FullName;
                    Message.Mails.Clear();
                    loaded = 0;
                }
                    Folder = client.GetFolder(fold);
                await Folder.OpenAsync(MailKit.FolderAccess.ReadOnly);
                for (int i = Folder.Count - (loaded == 0 ? 1 : loaded); i > Folder.Count - (loaded + load); i--)
                {
                    string Mail = null, Name = null;
                    MimeKit.MimeMessage message = Folder.GetMessage(i);
                    Mail = message.From[0].ToString();
                    Name = message.From[0].Name.ToString();
                    new Message().AddMail(new Message
                    {
                        ID = i,
                        Body = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        FullFrom = message.From.ToString(),
                        From = !string.IsNullOrEmpty(Name) ? Name : Mail,
                        Subject = message.Subject,
                        Time = message.Date.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy") ? message.Date.ToString("HH:mm") : message.Date.ToString("MM/dd/yyyy"),
                    });
                }
                loaded += load;

                inboxPage.Dispatcher.Invoke(() =>
                {
                    inboxPage.Messages.ItemsSource = Message.GetMailList();
                    new Message().AddMail(new Message() { Subject = "Load more..." });
                });
                client.Disconnect(true);

            }
        }
        public async System.Threading.Tasks.Task DownloadFolders(User user, ConfigModel conf, Inbox InboxPage)
        {
            using (var client = new ImapClient())
            {
                client.Connect(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                var ClientFolders = client.GetFolders(client.PersonalNamespaces[0]);

                for (int i = ClientFolders.Count - 1; i > -1; i--)
                {
                    InboxPage.Dispatcher.Invoke(() =>
                    {
                        InboxPage.Folders.Items.Add(ClientFolders[i]);
                    });
                }
                client.Disconnect(true);
            }
        }
        public void OpenMail(Message m, Inbox inboxPage)
        {
            if (m != null)
            {
                inboxPage.Body.NavigateToString("<head><meta charset='UTF-8'></head>" + m.Body);
                inboxPage.Body.Visibility = 0;
                inboxPage.Back.IsEnabled = true;
            }
        }

    }
}
