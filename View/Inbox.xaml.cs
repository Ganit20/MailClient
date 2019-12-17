using MailClient.Model;
using MailClient.ViewModel;
using MailKit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MailClient.View
{
    /// <summary>
    /// Interaction logic for Inbox.xaml
    /// </summary>
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
            Load.ItemsSource = ConfigModel.LoadNumber;

            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadFolders(user, Config, this);
            });
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadMessagesAsync(user, Config, this, dispatcher);
            });
            Load.SelectedIndex = 0;
            DataContext = Message.GetMailList();
        }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Message choosed = (Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                new ListMessages().DownloadMessagesAsync(UserData, Config, this, dispatcher);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                new ListMessages().DownloadMessagesAsync(UserData, Config, this, dispatcher);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                Message.Mails.Remove(choosed);

            }
            else
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                new ListMessages().OpenMail(choosed, this, UserData, Config);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }
        }

        private void Folders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            new ListMessages().DownloadMessagesAsync(UserData, Config, this, dispatcher, (IMailFolder)Folders.SelectedItem);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }


        private void Load_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Load.Items.Count >= ListMessages.load - 2)
            {
                ListMessages.load = ConfigModel.LoadNumber[Load.SelectedIndex];
                new ListMessages().Refresh(UserData, Config, this, dispatcher);
            }
        }

        private void Refresh_Button(object sender, RoutedEventArgs e)
        {
            new ListMessages().Refresh(UserData, Config, this, dispatcher);
        }
    }
}
