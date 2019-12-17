using MailClient.Model;
using MailClient.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

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
                SmtpPort.Text = config.SmtpPort.ToString();
                ImapPort.Text = config.ImapPort.ToString();
                SmtpServer.Text = config.SmtpServer;
                ImapServer.Text = config.ImapServer;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ImapServer.Text) || string.IsNullOrEmpty(ImapPort.Text) || string.IsNullOrEmpty(SmtpPort.Text) || string.IsNullOrEmpty(SmtpServer.Text))
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
