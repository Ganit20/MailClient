using MailClient.Model;
using MailClient.View;
using MailClient.ViewModel;
using Newtonsoft.Json;
using System.IO;
using System.Windows;


namespace MailClient
{ 
    public partial class MainWindow : Window
    {
        public static ConfigModel Config;
        public static User user;
        public MainWindow()
        {
            InitializeComponent();
            user = new RememberMe().Load();
            if(user != null){
                Email.Text = user.Mail;
                Password.Password = user.Password;
            }
            Config = new Configure().LoadConfig();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = new User()
            {
                Mail = Email.Text,
                Password = Password.Password
            };
            new Login().ToMail(user, this);
            if(Remember.IsChecked.Value)
            {
                new RememberMe().Save(user);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Config cfg = new Config(Config);
            cfg.ShowDialog();
        }
    }
}
