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
        public static string fold = "Inbox";
        private static IMailFolder folder;

        public async Task DownloadMessagesAsync(User user, ConfigModel conf, Inbox inboxPage, Dispatcher dispatcher, IMailFolder Folder = null)
        {
            using (ImapClient client = new ImapClient())
            {
                List<string> atc = new List<string>();
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                Folder = GetFold(client, Folder);
                await Folder.OpenAsync(MailKit.FolderAccess.ReadWrite);
                for (int i = Folder.Count - (loaded == 0 ? 1 : loaded); i > Folder.Count - (loaded + load); i--)
                {
                    IList<IMessageSummary> info = Folder.Fetch(new[] { i }, MessageSummaryItems.Flags | MessageSummaryItems.GMailLabels | MessageSummaryItems.UniqueId);
                    IList<IMessageSummary> message = Folder.Fetch(new[] { info.First().UniqueId }, MessageSummaryItems.Envelope);
                    string From = (message[0].Envelope.From.Count != 0) ? message[0].Envelope.From.ToString() : "Error";
                    string Name = (From != "Error") ? message[0].Envelope.From[0].Name : "Error";
                    string Mail = (Name != null && Name != "") ? Mail = From.Substring(From.IndexOf('<') + 1, From.IndexOf('>') - From.IndexOf('<') - 1) : "Error";

                    bool open = checkOpen(info);
                    dispatcher.Invoke(() =>
                    {
                        new Message().AddMail(new Message
                        {
                            ID = i,
                            FullFrom = message[0].Envelope.From.First().ToString(),
                            From = !string.IsNullOrEmpty(Name) ? Name : !string.IsNullOrEmpty(Mail) ? Mail : "Error",
                            Subject = message[0].Envelope.Subject,
                            To = message[0].Envelope.To.First().ToString(),
                            Time = message[0].Date.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy") ? message[0].Date.ToString("HH:mm") : message[0].Date.ToString("dd/MM/yyyy"),
                            Opened = open,
                            MessageColor = (open == true) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Color.FromRgb(211, 211, 211))
                        });
                    });
                    atc.Clear();
                }
                loaded += load;
                dispatcher.Invoke(() =>
                {
                    inboxPage.Messages.ItemsSource = Message.GetMailList();
                    new Message().AddMail(new Message() { Subject = "Load more..." });
                });
                client.Disconnect(true);

            }
        }

        private IMailFolder GetFold(ImapClient client, IMailFolder Folder)
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

        private bool checkOpen(IList<IMessageSummary> info)
        {
            bool open;
            if (info[0].Flags.Value.HasFlag(MessageFlags.Seen))
            {
                open = true;
            }
            else
            {
                open = false;
            }
            return open;
        }
        public void Refresh(User user, ConfigModel conf, Inbox inboxPage, Dispatcher dispatcher)
        {
            loaded = 0;
            Message.Mails.Clear();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            new ListMessages().DownloadMessagesAsync(user, conf, inboxPage, dispatcher);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task DownloadFolders(User user, ConfigModel conf, Inbox InboxPage)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
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
        public async Task OpenMail(Message m, Inbox inboxPage, User user, ConfigModel conf)
        {
            using (ImapClient client = new ImapClient())
            {
                List<string> atc = new List<string>();
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                IMailFolder Folder = client.GetFolder(folder.FullName);
                await Folder.OpenAsync(FolderAccess.ReadWrite);
                TextPart bodyHTML = null;
#pragma warning disable CS0219 // The variable 'body' is assigned but its value is never used
                BodyPartText body = null;
#pragma warning restore CS0219 // The variable 'body' is assigned but its value is never used
                IList<IMessageSummary> msg = Folder.Fetch(new[] { m.ID }, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Envelope);
                bodyHTML = (TextPart)Folder.GetBodyPart(msg.First().UniqueId, msg.First().HtmlBody);
                inboxPage.Attachments.Children.Clear();
                foreach (BodyPartBasic attachment in msg.First().Attachments)
                {
                    Button bt = new Button
                    {
                        Content = attachment.FileName
                    };
                    bt.Click += new RoutedEventHandler(AtcClicked);
                    inboxPage.Attachments.Children.Add(bt);
                }

                if (m != null)
                {
                    inboxPage.Info.Text = ($"Subject: {msg.First().Envelope.Subject} \n\rFrom {msg.First().Envelope.From} at {msg.First().Envelope.Date} to {msg.First().Envelope.To}");
                    inboxPage.Body.NavigateToString("<html><head><meta charset='UTF-8'></head>" + bodyHTML.Text + "</html>");
                    inboxPage.MailBody.Visibility = 0;
                    inboxPage.Back.IsEnabled = true;
                }
            }

        }

        private void AtcClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
