using MailClient.Model;
using MailClient.View;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                var Folder = new ListMessages().GetFold(client, ListMessages.folder.FullName);
                await Folder.OpenAsync(FolderAccess.ReadWrite);
                IList<MailKit.UniqueId> SearchUids;
                try
                {
                    SearchUids = await Folder.SearchAsync(TextSearchQuery.SubjectContains(Text));
                }
                catch (System.ArgumentException)
                {
                    new ListMessages().DownloadMessages(user, config, inboxPage, Folder.FullName);
                    SearchUids = null;
                }

                var msgs = Folder.Fetch(SearchUids, MessageSummaryItems.Envelope | MessageSummaryItems.Flags | MessageSummaryItems.GMailLabels);
                Message.Mails.Clear();
                new ListMessages().ShowMessages(msgs, inboxPage);
                client.Disconnect(true);
            }

        }


        public async Task ShowSeen(Inbox inboxPage,ConfigModel config,User user,short Mode)
        {
            using (ImapClient client = new ImapClient())
            {
                await client.ConnectAsync(config.ImapServer, config.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                var Folder = client.GetFolder(ListMessages.fold);
                Folder.Open(FolderAccess.ReadWrite);
                IList<UniqueId> SearchUids = null;
                if (Mode==1)
                SearchUids = await Folder.SearchAsync(SearchQuery.NotSeen );
                
                else if(Mode== 2)
                  SearchUids = await Folder.SearchAsync(SearchQuery.Seen);
                var a = SearchUids.Skip(SearchUids.Count-(ListMessages.loaded+ ListMessages.load+1)).Take(ListMessages.load);
                List<UniqueId> Unseen = a.ToList<UniqueId>();
                var msgs = Folder.Fetch(Unseen, MessageSummaryItems.Envelope | MessageSummaryItems.Flags | MessageSummaryItems.GMailLabels);
                List<IMessageSummary> msgsr= new List<IMessageSummary>();
                for(var i = msgs.Count - 1; i >= 0; i--)
                {
                  msgsr.Add(msgs[i]);
                }
                new ListMessages().ShowMessages(msgsr, inboxPage);
                client.Disconnect(true);
            }
        }

        
    }
}

