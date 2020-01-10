using MailClient.Model;
using MailClient.View.Inbox;
using MailClient.View.InboxWindow;
using MailClient.ViewModel;
using MailKit;
using Microsoft.Win32;
using Model;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MailClient.View
{

    public partial class MailWindow : Window
    {
        private readonly User UserData;
        private readonly ConfigModel Config;
 
        private MessageList MsgList;
        public MailWindow(User user, ConfigModel config)
        {
                ActualSettings.Actual = new LoadSettings().Read();
            Config = config;
            UserData = user;
            InitializeComponent();
            MainFrame.Navigate(new MessageList(this,Config, UserData));
            Folders.ItemsSource = Folder.Folders;
            this.DataContext = ActualSettings.Actual;
            Load.SelectedIndex = ActualSettings.Actual.DefaultLoadValue;
            new Model.FontSize().addFontSize();      
            TopBar.DataContext = ActualSettings.Actual;
            Load.DataContext = ActualSettings.Actual;
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadFolders(user, Config, this);
            });
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadMessages(user, Config, this);
            });

        }
        

        

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Reply.IsEnabled = false;
            if (MainFrame.CanGoBack)
                MainFrame.NavigationService.GoBack();
            else
                Back.IsEnabled = false;
        }

        private void Folders_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            var f = (Folder)Folders.SelectedItem;
            new ListMessages().DownloadMessages(UserData, Config, this, f.Name);
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
                Model.Message.Mails.Clear();
                ListMessages.loaded = 0;
                new ListMessages().DownloadMessages(UserData, Config, this);

            }
            else if (state == 0 || state == 2)
            {
                SeenFC.IsChecked = false;
                Model.Message.Mails.Clear();
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
                Model.Message.Mails.Clear();
                ListMessages.loaded = 0;
                new ListMessages().DownloadMessages(UserData, Config, this);

            }
            else if (state == 0 || state == 1)
            {
                UnseenFC.IsChecked = false;
                Model.Message.Mails.Clear();
                ListMessages.ShowMode = 2;
                ListMessages.loaded = 0;
                new Search().ShowSeen(this, Config, UserData, ListMessages.ShowMode);
            }
        }



        private void Move_Click(object sender, RoutedEventArgs e)
        {
            new SelectOperations().Move(Config, UserData, this);
        }

        private void SelectAllHandler(object sender, RoutedEventArgs e)
        {

            if (SelectAll.IsChecked == true)
            {
                Model.Message.SelectedMails.Clear();
                SelectAll.Content = "\uE73A";
                foreach (var msg in Model.Message.Mails)
                {
                    Model.Message.SelectedMails.Add(msg.UniqueID);
                    msg.IsSelected = true;
                }
            }
            else if (SelectAll.IsChecked == false)
            {
                Model.Message.SelectedMails.Clear();
                SelectAll.Content = "\uE739";
                foreach (var msg in Model.Message.Mails)
                {
                    msg.IsSelected = false;
                }
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            new SelectOperations().Delete(Config, UserData, this);

        }

       
        public void OpenAttachment(object sender, RoutedEventArgs e)
        {

            var SelectPath = new SaveFileDialog();
            
            
            var a = (Button)e.OriginalSource;
            string b = a.Content.ToString();
            SelectPath.FileName =b ;
            var result = SelectPath.ShowDialog();
            if (result == true)
            {
                new OpenMail().Attachment(this, UserData, Config, b, SelectPath.FileName);

            }

        }

        private void NewMail_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NewMessage(this,UserData,Config));
        }


        private void Reply_Click(object sender, RoutedEventArgs e)
        {
           var id = MsgList.Messages.Items.IndexOf(MsgList.Messages.SelectedItem);
           var _messageToReply = Message.Mails[id];
            var c = new NewMessage(this, UserData, Config);
            MainFrame.Navigate(c);
            new ReplyMessage().Reply(c, _messageToReply,UserData,Config);
            
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Settings options = new Settings(this);
            options.ShowDialog();
        }
    }
}