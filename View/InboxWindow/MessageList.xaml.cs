using MailClient.Model;
using MailClient.View.InboxWindow;
using MailClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailClient.View.Inbox
{
    /// <summary>
    /// Interaction logic for MessageList.xaml
    /// </summary>
    public partial class MessageList : Page
    {
        public int id = 0;
        private readonly User UserData;
        private readonly ConfigModel Config;
        private MailWindow Window;
        public MessageList(MailWindow w,ConfigModel config,User user)
        {
            w.MsgList = this;
            w.frameStatus = 0;
            Config = config;
            UserData = user;
            Window = w;
            InitializeComponent();
            Messages.ItemsSource = Model.Message.Mails;
        }
        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.Message choosed = (Model.Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().LoadMore(choosed, Window, Config, UserData);
                Model.Message.Mails.Remove(choosed);
            }
        }
        private void Messages_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Model.Message choosed = (Model.Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().LoadMore(choosed, Window, Config, UserData);
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Window.Dispatcher.Invoke(() =>
                    {
                        var o = new OpenMessage(Window);
                        Window.MainFrame.Navigate(o);
                        new OpenMail().OpenText(choosed, o, UserData, Config);

                    });
                    
                });
            }
        }
        private void SelectMessage(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)e.OriginalSource;
            var data = (Model.Message)checkBox.DataContext;
            if (checkBox.IsChecked == true)
            {
                Model.Message.SelectedMails.Add(data.UniqueID);
                Window.Move.IsEnabled = true;
                Window.Delete.IsEnabled = true;
            }
            else
            {
                Model.Message.SelectedMails.Remove(data.UniqueID);
                if (Model.Message.SelectedMails.Count == 0)
                {
                    Window.Move.IsEnabled = false;
                    Window.Delete.IsEnabled = false;
                }
            }
        }
            private void Favorite(object sender, RoutedEventArgs e)
            {
                var checkBox = (ToggleButton)e.OriginalSource;
                var data = (Model.Message)checkBox.DataContext;
                if (checkBox.IsChecked == true)
                {
                    checkBox.Content = "\uE735";
                    new SelectOperations().Favorite(UserData, Config, data.UniqueID);

                }
                else
                {
                    Model.Message.SelectedMails.Remove(data.UniqueID);
                    if (checkBox.IsChecked == false)
                    {
                        checkBox.Content = "\uE734";
                        new SelectOperations().unFavorite(UserData, Config, data.UniqueID);
                    }
                }
            }
        }
    }

