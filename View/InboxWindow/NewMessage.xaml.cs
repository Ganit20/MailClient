using MailClient.Model;
using MailClient.ViewModel;
using Microsoft.Win32;
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

namespace MailClient.View.InboxWindow
{
    /// <summary>
    /// Interaction logic for NewMessage.xaml
    /// </summary>
    public partial class NewMessage : Page
    {
        public int id = 2;
        MailWindow Window;
        private readonly User UserData;
        private readonly ConfigModel Config;
        private Preview pouct = null;
        public NewMessage(MailWindow w,User user, ConfigModel config)
        {
            if(w.MainFrame.CanGoBack)
            {
                w.Back.IsEnabled = true;
            }
            w.frameStatus = 2;
            Window = w;
            Config = config;
            UserData = user;
            InitializeComponent();
            FontList.DataContext = Fonts.SystemFontFamilies;
            FontList.SelectedIndex = 0;
            FontSize.ItemsSource = Model.FontSize.FontSizeList;
            FontSize.SelectedIndex = 0;
            ClrPicker_Font.SelectedColor = Colors.Black;
            AttachmentList.ItemsSource = Message.AttachmentList;
        }
        private void SendMail(object sender, RoutedEventArgs e)
        {
            if (To.Text != "" && Subject.Text != "")
            {
                try
                {
                    string body = "<body style='" +
                "font-family:" + FontList.SelectedItem.ToString() + ";" +
                "font-size:" + FontSize.SelectedItem + "';" +
                "color:" + ClrPicker_Font.SelectedColorText + ";>"
                + SendBody.Text +
                "</body>";
                    new SendMail().Send(Window, UserData, Config, To.Text, Subject.Text, body);

                    info inf = new info("Message Has Been Sent to: " + To.Text);
                    inf.ShowDialog();
                    Subject.Text = "";
                    To.Text = "";
                    SendBody.Text = "";
                    Window.MainFrame.NavigationService.GoBack();

                }
                catch (Exception exce)
                {
                    info inf = new info("Message Could not be Sent \n\r");
                    inf.ShowDialog();
                }
            }
            else
            {
                info inf = new info("Fields Cannot be empty");
                inf.ShowDialog();
            }
        }
        private void SendBody_Changed(object sender, TextChangedEventArgs e)
        {
            if (FontList.SelectedItem != null)
            {
                string body = "<body style='" +
                     "font-family:" + FontList.SelectedItem.ToString() + ";" +
                     "font-size:" + FontSize.SelectedItem + ";" +
                     "color:" + ClrPicker_Font.SelectedColorText + ";'>"
                     + SendBody.Text +
                     "</body>";
                PreviewWeb.NavigateToString(body);
                if (pouct != null)
                {
                    pouct.Update(body);
                }
            }
        }
        private void HyperLink(object sender, RoutedEventArgs e)
        {
            HyperlinkInput hl = new HyperlinkInput();
            hl.ShowDialog();
            if (hl.DialogResult == true)
            {
                var hyperLink = hl.RText;
                SendBody.Text = SendBody.Text.Insert(SendBody.SelectionStart, hyperLink);
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
            new MailCreate().AddMarkup(this, "<div style='text-align:Right'>", "</div>");
        }

        private void AllignCenter(object sender, RoutedEventArgs e)
        {
            new MailCreate().AddMarkup(this, "<div style='text-align:Center'>", "</div>");
        }
        private void AllignLeft(object sender, RoutedEventArgs e)
        {
            new MailCreate().AddMarkup(this, "<div style='text-align:Left'>", "</div>");
        }
        private void Italic(object sender, RoutedEventArgs e)
        {
            new MailCreate().AddMarkup(this, "<I>", "</I>");
        }
        private void Bold(object sender, RoutedEventArgs e)
        {
            new MailCreate().AddMarkup(this, "<B>", "</B>");
        }
        private void UnderLine(object sender, RoutedEventArgs e)
        {
            new MailCreate().AddMarkup(this, "<U>", "<U>");
        }
        private void ClrPickedFont(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

        }
        private void Preview(object sender, RoutedEventArgs e)
        {
            if (PreviewButton.IsChecked == true)
            {
                PreviewWeb.Visibility = (Visibility)0;
                pout.Visibility = (Visibility)0;
            }
            else
            {
                PreviewWeb.Visibility = (Visibility)2;
                pout.Visibility = (Visibility)2;
                if (pouct != null)
                {
                    pouct.Close();
                }
            }
        }
        private void PreviewOut(object sender, RoutedEventArgs e)
        {
            string body = "<body style='" +
                "font-family:" + FontList.SelectedItem.ToString() + ";" +
                "font-size:" + FontSize.SelectedItem + ";" +
                "color:" + ClrPicker_Font.SelectedColorText + ";'>"
                + SendBody.Text +
                "</body>";
            pout.Visibility = (Visibility)2;
            PreviewWeb.Visibility = (Visibility)2;
            Preview p = new Preview(body);
            p.Show();
            pouct = p;
        }
        private void Picker_SelectionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void EmojiAdd(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SendBody.Text += EmojiPicker.Selection;
        }

        private void LoadAttachment(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
            Message.AttachmentList.Add(open.FileName);
        }

        private void DeleteAttachment(object sender, RoutedEventArgs e)
        {
            Message.AttachmentList.Remove(((System.Windows.FrameworkElement)e.OriginalSource).DataContext.ToString());
        }
    }

}
