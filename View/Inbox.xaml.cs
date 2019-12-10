using MailClient.Model;
using MailClient.ViewModel;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailClient.View
{
    /// <summary>
    /// Interaction logic for Inbox.xaml
    /// </summary>
    public partial class Inbox : Page
    {
        ImapClient Cl;
        MessageInbox Msgs;
        public Inbox(ImapClient client)
        {
            Cl = client;
            InitializeComponent();
            Msgs = new ListMessages().DownloadMessages(client,this);
        }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var choosed = Messages.SelectedItem.ToString();
            var choosed_id = choosed.Substring(0,choosed.IndexOf('.'));
            int.TryParse(choosed_id, out int msgID);
            var msg = Msgs.MessageList.Find(e => e.ID == msgID);
            new ListMessages().OpenMail(msg,this);
        }

      
    }
}
