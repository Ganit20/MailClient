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
        public MessageInbox DownloadMessages(ImapClient c, Inbox inboxPage)
        {
            using (var client = c) 
            {
                
                var inbox = client.Inbox;
                inbox.Open(MailKit.FolderAccess.ReadWrite);
                Debug.Write(inbox.Count);
                MessageInbox MessageList = new MessageInbox();
                for (int i = 0; i <25; i++)
                {
                    var message = inbox.GetMessage(i);
                    MessageList.MessageList.Add(new Message { 
                        ID = i, 
                        Body = !string.IsNullOrEmpty(message.HtmlBody)? message.HtmlBody: message.TextBody,
                        From = message.From.ToString(), 
                        Subject = message.Subject, 
                        Time = message.Date.ToString() });;
                    inboxPage.Messages.Items.Add($"{i}. {message.From} {message.Subject}");
                }
                inboxPage.Messages.Items.Add("Load more...");
                return MessageList;
            } 
        }
        public void OpenMail(Message m, Inbox inboxPage)
        {
    
            inboxPage.Body.NavigateToString("<head><meta charset='UTF-8'></head>"+m.Body);     
        } 

        
    }
}
