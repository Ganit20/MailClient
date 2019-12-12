using MailClient.Model;
using MailClient.ViewModel;
using MailKit;
using MailKit.Net.Imap;
using System.Linq;
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
        static User UserData;

        static ConfigModel Config;
        public Inbox(User user, ConfigModel config)
        {
            var dispatcher = this.Dispatcher;
            Config = config;
            UserData = user;
            InitializeComponent();
            Task.Factory.StartNew(async () =>
            {
              await  new ListMessages().DownloadFolders(user, Config, this);

            });
            Task.Factory.StartNew(async () =>
            {
             await   new ListMessages().DownloadMessagesAsync(user, Config, this);
            });
            DataContext = Message.GetMailList();
        }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var choosed = (Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().DownloadMessagesAsync(UserData, Config, this);
                Message.Mails.Remove(choosed);

            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Body.Visibility = (Visibility)1;
            Back.IsEnabled = false;

        }

        private void Messages_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var choosed = (Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().DownloadMessagesAsync(UserData, Config, this);
                Message.Mails.Remove(choosed);

            }
            else
            {
                new ListMessages().OpenMail(choosed, this);
            }
        }

        private void Folders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new ListMessages().DownloadMessagesAsync(UserData, Config, this, (IMailFolder)Folders.SelectedItem);
        }
    }
}
