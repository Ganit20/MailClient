using MailClient.Model;
using MailClient.View;
using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MailClient.ViewModel
{
    internal class ListMessages
    {
        public static int loaded = 0;
        public static int load = 10;
        public static string fold = "Inbox";
        public static short ShowMode = 0;
        public async Task DownloadMessages(User user, ConfigModel conf, Inbox inboxPage, string Folder = null)
        {
            using (ImapClient client = new ImapClient())
            {
                 if(load<10) load=10;
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                List<int> uids = new List<int>();
                var folder = GetFold(client, Folder);
                if (Folder != null)
                    fold = Folder;
                else Folder = fold;
                await folder.OpenAsync(MailKit.FolderAccess.ReadOnly);
                List<IMessageSummary> InfoReverse = new List<IMessageSummary>();
                string ENDToken = null;
                if(folder.Count<load + loaded)
                {
                    load = folder.Count - loaded;
                    ENDToken = "END";
                }
                
                for (int i = folder.Count - (loaded == 0 ? 1 : loaded); i >= folder.Count - (load==1 ? 1: (loaded + load - 1)); i--)
                {
                        IList<IMessageSummary> info = await folder.FetchAsync(new[] { i }, MessageSummaryItems.Envelope | MessageSummaryItems.Flags | MessageSummaryItems.UniqueId);
                        InfoReverse.Add(info.First());
                }
                    ShowMessages(InfoReverse, inboxPage,ENDToken);

                    client.Disconnect(true);

                }
            }
            public async Task ShowMessages(IList<IMessageSummary> messages, Inbox inboxPage,string EndToken = null)
            {
                List<string> atc = new List<string>();
                foreach (IMessageSummary message in messages)
                {
                    string From = message.Envelope.From.ToString();
                    string Name = message.Envelope.From[0].Name;
                    string Mail = (Name != null && Name != "") ? Mail = From.Substring(From.IndexOf('<') + 1, From.IndexOf('>') - From.IndexOf('<') - 1) : From;
                    bool open = checkOpen(message);
                    inboxPage.Dispatcher.Invoke(() =>
                    {
                        new Message().AddMail(new Message
                        {
                            UniqueID = message.UniqueId,
                            ID = message.Index,
                            FullFrom = message.Envelope.From.First().ToString(),
                            From = !string.IsNullOrEmpty(Name) ? Name : !string.IsNullOrEmpty(Mail) ? Mail : From,
                            Subject = message.Envelope.Subject,
                            To = message.Envelope.To.ToString(),
                            Time = message.Date.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy") ? message.Date.ToString("HH:mm") : message.Date.ToString("dd/MM/yyyy"),
                            Opened = open,
                            MessageColor = (open == true) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Color.FromRgb(211, 211, 211)),
                            IsLoadMore = 0,
                            IsFavorite = message.Flags.Value.HasFlag(MessageFlags.Flagged)
                        }) ;
                    });
                    atc.Clear();
                }
                inboxPage.Dispatcher.Invoke(() =>
                {
                    if(EndToken==null)
                    new Message().AddMail(new Message() { 
                        Subject = "Load more..." ,
                    IsLoadMore=(Visibility)2});
                    inboxPage.Load.ItemsSource = ConfigModel.LoadNumber;
                });
                loaded += load;
            }
            public IMailFolder GetFold(ImapClient client, string Folder)
            {
                if (Folder == null)
                {
                if (fold != null)
                    Folder = fold;
                else
                    Folder = "Inbox";
                }
                else
                {
                    loaded = 0;
                    Message.Mails.Clear();
                }
                return client.GetFolder(Folder);
            }

            public bool checkOpen(IMessageSummary info)
            {
                bool isopen;
                if (info.Flags.Value.HasFlag(MessageFlags.Seen)  )
                {
                    isopen = true;
                }
                else
                {
                    isopen = false;
                }
                return isopen;
            }
            public void Refresh(User user, ConfigModel conf, Inbox inboxPage)
            {

                loaded = 0;
                Message.Mails.Clear();
                Folder.Folders.Clear();
                new ListMessages().DownloadFolders(user, conf, inboxPage);
            if (ShowMode == 0)
                {
                    new ListMessages().DownloadMessages(user, conf, inboxPage);
                }
                else if (ShowMode == 1)
                {
                    new Search().ShowSeen(inboxPage, conf, user, ShowMode);
                }
                else if (ShowMode == 2)
                {
                    new Search().ShowSeen(inboxPage, conf, user, ShowMode);
                }

              
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
                            Folder.Folders.Add(new Folder()
                            {
                                Name = ClientFolders[i].FullName,
                                id = ClientFolders[i].Id
                            });
                        });
                    }
                    client.Disconnect(true);
                }
            }
            public void LoadMore(Message choosed, Inbox inboxPage, ConfigModel conf, User user)
            {
                //ShowMode 0 = Show all messages
                //ShowMode 1 = Show only unseen
                //ShowMode 2 = Show only seen
                if (ShowMode == 0)
                {
                    new ListMessages().DownloadMessages(user, conf, inboxPage);
                }
                else if (ShowMode == 1)
                {
                    new Search().ShowSeen(inboxPage, conf, user, ShowMode);
                }
                else if (ShowMode == 2)
                {
                    new Search().ShowSeen(inboxPage, conf, user, ShowMode);
                }
            }


        }
    }
