using MailClient.Model;
using MailClient.ViewModel;
using MailKit;
using Microsoft.Win32;
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
            Messages.ItemsSource = Model.Message.Mails;
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadFolders(user, Config, this);
            });
            Task.Factory.StartNew(async () =>
            {
                await new ListMessages().DownloadMessages(user, Config, this);
            });
            Load.SelectedIndex = 0;
            DataContext = Model.Message.GetMailList();

            FontList.DataContext = Fonts.SystemFontFamilies;
            FontList.SelectedIndex=0;
            new Model.FontSize().addFontSize();
            FontSize.ItemsSource = Model.FontSize.FontSizeList;
            FontSize.SelectedIndex = 0;
        }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.Message choosed = (Model.Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().LoadMore(choosed, this, Config, UserData);
                Model.Message.Mails.Remove(choosed);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MailBody.Visibility = (Visibility)1;
            Back.Visibility = (Visibility)2;
            SelectAll.Visibility = (Visibility)0;
            NewMessage.Visibility = (Visibility)2;
        }

        private void Messages_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Model.Message choosed = (Model.Message)Messages.SelectedItem;
            if (choosed != null && choosed.Subject == "Load more...")
            {
                new ListMessages().LoadMore(choosed, this, Config, UserData);
            }
            else
            {
                new OpenMail().OpenText(choosed, this, UserData, Config);
            }
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

        private void SelectMessage(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)e.OriginalSource;
            var data = (Model.Message)checkBox.DataContext;
            if (checkBox.IsChecked == true)
            {
                Model.Message.SelectedMails.Add(data.UniqueID);
                Move.IsEnabled = true;
                Delete.IsEnabled = true;
            }
            else
            {
                Model.Message.SelectedMails.Remove(data.UniqueID);
                if (Model.Message.SelectedMails.Count == 0)
                {
                    Move.IsEnabled = false;
                    Delete.IsEnabled = false;
                }
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

        private void Favorite(object sender, RoutedEventArgs e)
        {
            var checkBox = (ToggleButton)e.OriginalSource;
            var data = (Model.Message)checkBox.DataContext;
            if (checkBox.IsChecked == true)
            {
                checkBox.Content = "\uE735";
                new SelectOperations().Favorite(UserData, Config, this, data.UniqueID);

            }
            else
            {
                Model.Message.SelectedMails.Remove(data.UniqueID);
                if (checkBox.IsChecked == false)
                {
                    checkBox.Content = "\uE734";
                    new SelectOperations().unFavorite(UserData, Config, this, data.UniqueID);
                }
            }
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
            Back.Visibility = (Visibility)0;
            SelectAll.Visibility = (Visibility)2;
            MailBody.Visibility = (Visibility)2;
            NewMessage.Visibility = (Visibility)0;
        }


        private void SendMail(object sender, RoutedEventArgs e)
        {
            if (To.Text != "" && Subject.Text != "" )
            {
                try{
                    
                    new SendMail().Send(this, UserData, Config, To.Text, Subject.Text, SendBody.Text);
                    
                    info inf = new info("Message Has Been Sent to: " + To.Text);
                    inf.ShowDialog();
                    Subject.Text = "";
                    To.Text = "";
                    SendBody.Text = "";
                    NewMessage.Visibility = (Visibility)2;
                    Back.Visibility = (Visibility)2;
                    SelectAll.Visibility = (Visibility)0;
                }
                catch(Exception exce)
                {
                    info inf = new info("Message Could not be Sent \n\r"  +exce.StackTrace);
                    inf.ShowDialog();
                }
            }
            else
            {
                info inf = new info("Fields Cannot be empty");
                inf.ShowDialog();
            }
        }

        private void SendBody_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Return)
            {
                SendBody.Text += "<BR>";
                SendBody.SelectionStart = SendBody.Text.Length;
                SendBody.SelectionLength = 0;
            }
        }

        private void AllignRight(object sender, RoutedEventArgs e)
        {
            var sel = SendBody.SelectionStart;
            var selleng = SendBody.SelectionLength;
            var html = "<div style='text-align: right;'> ";
            SendBody.Text = SendBody.Text.Insert(sel + selleng, "</div> ");
            SendBody.Text = SendBody.Text.Insert(sel,html);
            SendBody.SelectionStart = sel + html.Length + selleng;
            SendBody.SelectionLength = 0;
        }

        private void AllignCenter(object sender, RoutedEventArgs e)
        {
            var sel = SendBody.SelectionStart;
            var selleng = SendBody.SelectionLength;
            var html = "<div style='text-align: center;'> ";
            SendBody.Text = SendBody.Text.Insert(sel + selleng, "</div> ");
            SendBody.Text = SendBody.Text.Insert(sel, html);
            SendBody.SelectionStart = sel + html.Length + selleng;
            SendBody.SelectionLength = 0;
        }


        private void AllignLeft(object sender, RoutedEventArgs e)
        {
            var sel = SendBody.SelectionStart;
            var selleng = SendBody.SelectionLength;
            var html = "<div style='text-align: left;'> ";
            SendBody.Text = SendBody.Text.Insert(sel + selleng, "</div> ");
            SendBody.Text = SendBody.Text.Insert(sel, html);
            SendBody.SelectionStart = sel+ html.Length + selleng;
            SendBody.SelectionLength = 0;
        }
        private void Italic(object sender, RoutedEventArgs e)
        {
            var sel = SendBody.SelectionStart;
            var selleng = SendBody.SelectionLength;
            var html = "<i>";
            SendBody.Text = SendBody.Text.Insert(sel + selleng, "</i>");
            SendBody.Text = SendBody.Text.Insert(sel, html);
            SendBody.SelectionStart = sel + html.Length + selleng;
            SendBody.SelectionLength = 0;
        }
        private void Bold(object sender, RoutedEventArgs e)
        {
            var sel = SendBody.SelectionStart;
            var selleng = SendBody.SelectionLength;
            var html = "<b>";
            SendBody.Text = SendBody.Text.Insert(sel + selleng, "</b>");
            SendBody.Text = SendBody.Text.Insert(sel, html);
            SendBody.SelectionStart = sel + html.Length + selleng;
            SendBody.SelectionLength = 0;
        }
        private void UnderLine(object sender, RoutedEventArgs e)
        {
            var sel = SendBody.SelectionStart;
            var selleng = SendBody.SelectionLength;
            var html = "<U>";
            SendBody.Text = SendBody.Text.Insert(sel + selleng, "</U>");
            SendBody.Text = SendBody.Text.Insert(sel, html);
            SendBody.SelectionStart = sel + html.Length + selleng;
            SendBody.SelectionLength = 0;
        }
        private void Preview(object sender, RoutedEventArgs e)
        {

                 string font ="<body style='font-family:"+ FontList.SelectedItem.ToString()+";font-size:"+FontSize.SelectedItem+ "';>" + SendBody.Text + "</body>";
                
            Preview prevMail = new Preview(font);
            prevMail.ShowDialog();
        }

        private void HyperLink(object sender, RoutedEventArgs e)
        {

        }
    }
}
