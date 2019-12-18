using MailClient.Model;
using MailClient.View;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MailClient.ViewModel
{
    internal class ListMessages
    {
        public static int loaded = 0;
        public static int load = 25;
        private static string fold = "Inbox";
        public static IMailFolder folder;
        public async Task DownloadMessages(User user, ConfigModel conf, Inbox inboxPage, Dispatcher dispatcher, IMailFolder Folder = null)
        {
            using (ImapClient client = new ImapClient())
            {
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                Folder = GetFold(client, Folder);
                await Folder.OpenAsync(MailKit.FolderAccess.ReadWrite);
                for (int i = Folder.Count - (loaded == 0 ? 1 : loaded); i > Folder.Count - (loaded + load); i--)
                {
                    var info = await Folder.FetchAsync(new[] { i }, MessageSummaryItems.Flags | MessageSummaryItems.Size | MessageSummaryItems.Envelope);
                    await ShowMessages(info, dispatcher, inboxPage);
                }
                
              
                client.Disconnect(true);
                dispatcher.Invoke(() =>
                {
                    new Message().AddMail(new Message() { Subject = "Load more..." });
                });
                loaded += load;
            }
        }
        public async Task ShowMessages(IList<IMessageSummary> messages,Dispatcher dispatcher,Inbox inboxPage)
        {
            List<string> atc = new List<string>();
            foreach(var message in messages)
            {
                string From =  message.Envelope.From.ToString() ;
                string Name =  message.Envelope.From[0].Name ;
                string Mail = (Name != null && Name != "") ? Mail = From.Substring(From.IndexOf('<') + 1, From.IndexOf('>') - From.IndexOf('<') - 1) : From;
                bool open = checkOpen(message);
                dispatcher.Invoke(() =>
                {
                    new Message().AddMail(new Message
                    {
                        ID = message.Index,
                        FullFrom = message.Envelope.From.First().ToString(),
                        From = !string.IsNullOrEmpty(Name) ? Name : !string.IsNullOrEmpty(Mail) ? Mail : From,
                        Subject = message.Envelope.Subject,
                        To = message.Envelope.To.ToString(),
                        Time = message.Date.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy") ? message.Date.ToString("HH:mm") : message.Date.ToString("dd/MM/yyyy"),
                        Opened = open,
                        MessageColor = (open == true) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Color.FromRgb(211, 211, 211))
                    });
                });
                atc.Clear();
            }
            
            dispatcher.Invoke(() =>
            {
                inboxPage.Load.ItemsSource = ConfigModel.LoadNumber;
            });
        }
        public IMailFolder GetFold(ImapClient client, IMailFolder Folder)
        {
            if (Folder == null)
            {
                Folder = client.Inbox;
            }
            else
            {
                loaded = 0;
                Message.Mails.Clear();
            }
            fold = Folder.FullName;
            folder = Folder;
            return client.GetFolder(fold);
        }

        public bool checkOpen(IMessageSummary info)
        {
            bool isopen;
            if (info.Flags.Value.HasFlag(MessageFlags.Seen))
            {
                isopen = true;
            }
            else
            {
                isopen = false;
            }
            return isopen;
        }
        public void Refresh(User user, ConfigModel conf, Inbox inboxPage, Dispatcher dispatcher)
        {
            loaded = 0;
            Message.Mails.Clear();
            inboxPage.Folders.Items.Clear();
            new ListMessages().DownloadMessages(user, conf, inboxPage, dispatcher);
            new ListMessages().DownloadFolders(user, conf, inboxPage);
        }
        public async Task DownloadFolders(User user, ConfigModel conf, Inbox InboxPage)
        {
            using (ImapClient client = new ImapClient())
            {
                client.Connect(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                IList<IMailFolder> ClientFolders = client.GetFolders(client.PersonalNamespaces[0]);

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
        

       
    }
}
