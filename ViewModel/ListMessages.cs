using MailClient.Model;
using MailClient.View;
using MailKit.Net.Imap;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MailClient.ViewModel
{
    class ListMessages
    {
        public MessageInbox DownloadMessages(User user, ConfigModel conf, Inbox inboxPage)
        {
                using(var client = new ImapClient())
            {
                client.Connect(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                var inbox = client.Inbox;
                inbox.Open(MailKit.FolderAccess.ReadWrite);
                MessageInbox MessageList = new MessageInbox();
                int load = 26;
                for (int i = inbox.Count-1; i >inbox.Count-load; i--)
                {
                    var message = inbox.GetMessage(i);
                    MessageList.MessageList.Add(new Message {
                        ID = i,
                        Body = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        From = message.From.Mailboxes.ToString(), // message.From.ToString(), 
                        Subject = message.Subject,
                        Time = message.Date.ToString() }) ;
                    inboxPage.Dispatcher.Invoke(() =>
                    {
                        inboxPage.Messages.Items.Add($"{i}. {message.From} {message.Subject}");
                    });
                    
                }
                inboxPage.Dispatcher.Invoke(() =>
                {
                    inboxPage.Messages.Items.Add("Load more...");
                });
                    client.Disconnect(true);
                return MessageList;
            }
        }
        public async System.Threading.Tasks.Task DownloadFoldersAsync(User user,ConfigModel conf, Inbox InboxPage)
        {
            using(var client =new ImapClient())
            {
            client.Connect(conf.ImapServer, conf.ImapPort);
            client.Authenticate(user.Mail, user.Password);
            var ClientFolders = client.GetFolders(client.PersonalNamespaces[0]);
          
                for(int i = ClientFolders.Count-1; i > -1; i--)
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
            }
            } 

        
    }
}
