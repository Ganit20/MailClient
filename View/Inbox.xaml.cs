using MailClient.Model;
using MailClient.ViewModel;
using MailKit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MailClient.View
{

    public partial class Inbox : Page
    {
        private static User UserData;
        private static ConfigModel Config;
        private readonly Dispatcher dispatcher;
        public Inbox(User user, ConfigModel config)
        {
            dispatcher = Application.Current.Dispatcher;
            Config = config;
            UserData = user;
            InitializeComponent();

            Messages.ItemsSource = Message.GetMailList();
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadFolders(user, Config, this);
            });
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadMessages(user, Config, this, dispatcher);
            });
            Load.SelectedIndex = 0;
            DataContext = Message.GetMailList();
        }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Message choosed = (Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {

                new ListMessages().DownloadMessages(UserData, Config, this, dispatcher);
                Message.Mails.Remove(choosed);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MailBody.Visibility = (Visibility)1;
            Back.IsEnabled = false;

        }

        private void Messages_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Message choosed = (Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().DownloadMessages(UserData, Config, this, dispatcher);
                Message.Mails.Remove(choosed);

            }
            else
            {
                new OpenMail().OpenText(choosed, this, UserData, Config);
            }
        }

        private void Folders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new ListMessages().DownloadMessages(UserData, Config, this, dispatcher, (IMailFolder)Folders.SelectedItem);
        }


        private void Load_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                ListMessages.load = ConfigModel.LoadNumber[Load.SelectedIndex];
                new ListMessages().Refresh(UserData, Config, this, dispatcher);
            
        }

        private void Refresh_Button(object sender, RoutedEventArgs e)
        {
            new ListMessages().Refresh(UserData, Config, this, dispatcher);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            new Search().SearchMessageAsync(UserData, Config, this, Search.Text);
        }
    }
}
