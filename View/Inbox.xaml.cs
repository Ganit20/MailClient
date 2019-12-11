using MailClient.Model;
using MailClient.ViewModel;
using MailKit.Net.Imap;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MailClient.View
{
    /// <summary>
    /// Interaction logic for Inbox.xaml
    /// </summary>
    public partial class Inbox : Page
    {
        User UserData;
        MessageInbox Msgs;
        ConfigModel Config;
        public Inbox(User user,ConfigModel config)
        {
            var dispatcher = this.Dispatcher;
            Config = config;
            UserData = user;
            InitializeComponent();
            Task.Factory.StartNew(() =>
            {
                new ListMessages().DownloadFoldersAsync(user,Config, this);

            });
            Task.Factory.StartNew(() =>
            {
                Msgs = new ListMessages().DownloadMessages(user, Config, this);
            });
            }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Messages.SelectedItem.ToString() == "Load more...")
            {

            } else {
                var choosed = Messages.SelectedItem.ToString();
                var choosed_id = choosed.Substring(0, choosed.IndexOf('.'));
                int.TryParse(choosed_id, out int msgID);
                var msg = Msgs.MessageList.Find(e => e.ID == msgID);
                new ListMessages().OpenMail(msg, this);
                Body.Visibility = 0;
                Back.IsEnabled = true;
            } }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Body.Visibility = (Visibility)1;
            Back.IsEnabled = false;
            
        }
    }
}
