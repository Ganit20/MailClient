using MailClient.Model;
using MailClient.View;
using MailClient.View.InboxWindow;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
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
        static UniqueId LastOpenId;

        public async Task OpenText(Message m, OpenMessage Page, User user, ConfigModel conf)
        {
            
            using (ImapClient client = new ImapClient())
            {
                List<string> atc = new List<string>();
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                IMailFolder Folder = client.GetFolder(ListMessages.fold);
                await Folder.OpenAsync(FolderAccess.ReadWrite);
                IList<IMessageSummary> msg = Folder.Fetch(new[] { m.ID }, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Envelope);
                LastOpenId = msg.First().UniqueId;
                var bodyHTML = (TextPart)Folder.GetBodyPart(msg.First().UniqueId, msg.First().HtmlBody);
                Page.Dispatcher.Invoke(() =>
                {
                    Page.Attachments.Children.Clear();
                });
                Folder.SetFlags(m.ID, MessageFlags.Seen, true);
                m.MessageColor = new SolidColorBrush(Colors.White);
                foreach (BodyPartBasic attachment in msg.First().Attachments)
                {
                    Button bt = new Button
                    {
                        Content = attachment.FileName,
                    };
                    Page.Dispatcher.Invoke(() =>
                    {
                        bt.Click += new RoutedEventHandler(new MailWindow(user, conf).OpenAttachment);
                        Page.Attachments.Children.Add(bt);
                    });
                }

                if (m != null)
                {
                    Page.Dispatcher.Invoke(() =>
                    {
                        Page.Info.Text = ($"Subject: {msg.First().Envelope.Subject} \n\rFrom {msg.First().Envelope.From} at {msg.First().Envelope.Date} to {msg.First().Envelope.To}");
                        Page.Body.NavigateToString("<html><head><meta charset='UTF-8'></head>" + bodyHTML.Text + "</html>");
                        Page.MailBody.Visibility = 0;
                    });
                }
                client.Disconnect(true);
            }

        }
        public async Task Attachment(MailWindow inboxPage, User user, ConfigModel conf, string Atch, string destination)
        {
            using (ImapClient client = new ImapClient())
            {
                await client.ConnectAsync(conf.ImapServer, conf.ImapPort);
                client.Authenticate(user.Mail, user.Password);
                IMailFolder Folder = client.GetFolder(ListMessages.fold);
                await Folder.OpenAsync(FolderAccess.ReadWrite);
                IList<IMessageSummary> atc = Folder.Fetch(new[] { LastOpenId }, MessageSummaryItems.Body);
                var multipart = (BodyPartMultipart)atc.First().Body;
                var attachment = multipart.BodyParts.OfType<BodyPartBasic>().FirstOrDefault(x => x.FileName == Atch);
                TransferProgress progress = new TransferProgress();
                    Downloads FileDownload = new Downloads();
                    FileDownload.Show();
                var file = Folder.GetBodyPart(LastOpenId, attachment,default,progress);
                
                

                using (var stream = File.Create(destination))
                {
                    if (file is MessagePart)
                    {
                        var part = (MessagePart)file;

                        part.Message.WriteTo(stream);
                    }
                    else
                    {
                        var part = (MimePart)file;

                        part.Content.DecodeTo(stream);
                    }
                }
            }
        }

        }
    }

