using MailClient.Model;
using MailClient.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MailClient.View
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config(ConfigModel config)
        {
            InitializeComponent();
            if (config != null)
            {
                this.SmtpPort.Text = config.SmtpPort.ToString();
                this.ImapPort.Text = config.ImapPort.ToString();
                this.SmtpServer.Text = config.SmtpServer;
                this.ImapServer.Text = config.ImapServer;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ImapServer.Text) || String.IsNullOrEmpty(ImapPort.Text) || String.IsNullOrEmpty(SmtpPort.Text) || String.IsNullOrEmpty(SmtpServer.Text))
            {

                MainWindow.Config = new ConfigModel()
                {
                    ImapPort = int.Parse(ImapPort.Text),
                    ImapServer = ImapServer.Text,
                    SmtpPort = int.Parse(SmtpPort.Text),
                    SmtpServer = SmtpServer.Text
                };
                new Configure().SaveConfig(MainWindow.Config);
            }
            else
            {
                Error.Text = "Fields can not be Empty";
            }
        }
            private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
