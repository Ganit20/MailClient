using MailClient.Model;
using MailClient.ViewModel;
using MailKit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MailClient.View
{

    public partial class Inbox : Page
    {
        private readonly User UserData;
        private readonly ConfigModel Config;
        private readonly Dispatcher dispatcher;
        public Inbox(User user, ConfigModel config)
        {
            dispatcher = Application.Current.Dispatcher;
            Config = config;
            UserData = user;
            InitializeComponent();
            Folders.ItemsSource = Folder.Folders;
            Messages.ItemsSource = Message.Mails;
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadFolders(user, Config, this);
            });
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadMessages(user, Config, this);
            });
            Load.SelectedIndex = 0;
            DataContext = Message.GetMailList();
        }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Message choosed = (Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().LoadMore(choosed, this, Config, UserData);
                Message.Mails.Remove(choosed);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MailBody.Visibility = (Visibility)1;
            Back.Visibility = (Visibility)2;
            SelectAll.Visibility = (Visibility)0;
        }

        private void Messages_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Message choosed = (Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().LoadMore(choosed,this,Config,UserData);
            }
            else
            {
                new OpenMail().OpenText(choosed, this, UserData, Config);
            }
        }

        private void Folders_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            var f = (Folder)Folders.SelectedItem;
            new ListMessages().DownloadMessages(UserData, Config, this,f.Name );
        }


        private void Load_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ConfigModel.LoadNumber[Load.SelectedIndex] != ListMessages.load)
            {
                ListMessages.load = ConfigModel.LoadNumber[Load.SelectedIndex];
                new ListMessages().Refresh(UserData, Config, this);
            }
        }

        private void Refresh_Button(object sender, RoutedEventArgs e)
        {
            new ListMessages().Refresh(UserData, Config, this);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            new Search().SearchMessageAsync(UserData, Config, this, Search.Text);
        }

        private void Unseen(object sender, RoutedEventArgs e)
        {
            var state = ListMessages.ShowMode;
            if (state == 1)
            {
                UnseenFC.IsChecked = false;
                ListMessages.ShowMode = 0;
                Message.Mails.Clear();
                ListMessages.loaded = 0;
                new ListMessages().DownloadMessages(UserData, Config, this);

            }
            else if(state==0 || state==2)
            {
                SeenFC.IsChecked = false;
                Message.Mails.Clear();
                ListMessages.ShowMode = 1;
                ListMessages.loaded = 0;
                new Search().ShowSeen(this, Config, UserData, ListMessages.ShowMode);
            }
            
        }

        private void Seen(object sender, RoutedEventArgs e)
        {
            var state = ListMessages.ShowMode;
            if (state == 2)
            {
                SeenFC.IsChecked = false;
                ListMessages.ShowMode = 0;
                Message.Mails.Clear();
                ListMessages.loaded = 0;
                new ListMessages().DownloadMessages(UserData, Config, this);

            }
            else if (state == 0 || state == 1)
            {
                UnseenFC.IsChecked = false;
                Message.Mails.Clear();
                ListMessages.ShowMode = 2;
                ListMessages.loaded = 0;
                new Search().ShowSeen(this, Config, UserData, ListMessages.ShowMode);
            }
        }

        private void SelectMessage(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)e.OriginalSource;
            var data = (Message)checkBox.DataContext;
            if (checkBox.IsChecked==true)
            {
                Message.SelectedMails.Add(data.UniqueID);
                Move.IsEnabled = true;
                Delete.IsEnabled = true;
            }else
            {
                Message.SelectedMails.Remove(data.UniqueID);
                if(Message.SelectedMails.Count==0)
                {
                    Move.IsEnabled = false;
                    Delete.IsEnabled = false;
                }
            }
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            new SelectOperations().Move(Config,UserData,this);
        }

        private void SelectAllHandler(object sender, RoutedEventArgs e)
        {

            if (SelectAll.IsChecked == true)
            {
                Message.SelectedMails.Clear();
                SelectAll.Content ="\uE73A";
                foreach (var msg in Message.Mails)
                {
                    Message.SelectedMails.Add(msg.UniqueID);
                    msg.IsSelected = true;
                }
            } else if(SelectAll.IsChecked==false)
            {
                Message.SelectedMails.Clear();
                SelectAll.Content = "\uE739";
                foreach (var msg in Message.Mails)
                {
                    msg.IsSelected = false;
                }
            }
          
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            new SelectOperations().Delete(Config, UserData,this);
            
        }
    }
}
