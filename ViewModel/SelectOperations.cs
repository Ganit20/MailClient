using MailClient.Model;
using MailClient.View;
using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailClient.ViewModel
{
    class SelectOperations
    {
        public async Task Move(ConfigModel conf, User user,Inbox inboxPage)
        {
            var choosed = (Folder)inboxPage.Folders.SelectedItem;
            string folder = String.Empty;
            if(choosed== null )
            {
                folder = "inbox";
            }else
            {
                 folder = choosed.Name;
            }

            using (ImapClient client = new ImapClient())
            {
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                var Folder = client.GetFolder(folder);
                Folder.Open(MailKit.FolderAccess.ReadWrite);
                FolderChoose select = new FolderChoose();
                select.ShowDialog();
                var wchoosed = (Folder)select.FolderSelect.SelectedItem;
                var destiny = client.GetFolder(wchoosed.Name);
                Folder.MoveTo(Message.SelectedMails, destiny);
                new ListMessages().Refresh(user,conf,inboxPage);
            }
        }
        public async Task Delete(ConfigModel conf, User user,Inbox inbox)
        {
            using(ImapClient client = new ImapClient())
            {
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                var Folder = client.GetFolder(ListMessages.fold);
                Folder.Open(MailKit.FolderAccess.ReadWrite);
               
                if (client.Capabilities.HasFlag(ImapCapabilities.SpecialUse))
                {
                    Deleting content = new Deleting("Are you sure? Your message will be move into trash folder.");
                    content.ShowDialog();
                    bool result = content.result;
                    if (result == true)
                    {
                        var trash = client.GetFolder(SpecialFolder.Trash);
                        Folder.MoveTo(Message.SelectedMails, trash);
                        new ListMessages().Refresh(user, conf, inbox);
                    }
                }
                else
                {
                    Deleting content = new Deleting("Are you sure your IMAP Server do not support trash folder. Message wil be deleted ");
                    bool result = content.result;
                    if (result == true)
                    {
                        Folder.AddFlags(Message.SelectedMails, MailKit.MessageFlags.Deleted, false);
                        new ListMessages().Refresh(user, conf, inbox);
                    }     
                }
               
               
            }
        }
    }
}
