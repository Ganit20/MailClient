using MailClient.Model;
using MailClient.View;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MailClient.ViewModel
{
    class Search
    {
        public async Task SearchMessageAsync(User user, ConfigModel config, Inbox inboxPage, string Text)
        {
            using (ImapClient client = new ImapClient())
            {
                await client.ConnectAsync(config.ImapServer, config.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                var Folder = new ListMessages().GetFold(client, ListMessages.folder);
                await Folder.OpenAsync(FolderAccess.ReadWrite);
                IList<MailKit.UniqueId> SearchUids;
                try
                {
                     SearchUids = await Folder.SearchAsync(TextSearchQuery.SubjectContains(Text));
                }catch(System.ArgumentException)
                {
                     new ListMessages().DownloadMessages(user,config,inboxPage,inboxPage.Dispatcher,Folder);
                    SearchUids = null;
                }

                var msgs = Folder.Fetch(SearchUids, MessageSummaryItems.Envelope | MessageSummaryItems.Flags | MessageSummaryItems.GMailLabels);
                Message.Mails.Clear();
                foreach (var message in msgs)
                {
                    var open = new ListMessages().checkOpen(message);
                    string From = (message.Envelope.From.Count != 0) ? message.Envelope.From.ToString() : "Error";
                    string Name = (From != "Error") ? message.Envelope.From[0].Name : "Error";
                    string Mail = (Name != null && Name != "") ? Mail = From.Substring(From.IndexOf('<') + 1, From.IndexOf('>') - From.IndexOf('<') - 1) : "Error";
                    Message.Mails.Add(new Message
                    {
                        ID = message.Index,
                        FullFrom = message.Envelope.From.ToString(),
                        From = !string.IsNullOrEmpty(Name) ? Name : !string.IsNullOrEmpty(Mail) ? Mail : "Error",
                        Subject = message.Envelope.Subject,
                        To = message.Envelope.To.ToString(),
                        Time = message.Date.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy") ? message.Date.ToString("HH:mm") : message.Date.ToString("dd/MM/yyyy"),
                        Opened = open,
                        MessageColor = (open == true) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Color.FromRgb(211, 211, 211))
                    });
                }

            }

        }

        public void ShowUnSeen()
        {

        }
    }
}
