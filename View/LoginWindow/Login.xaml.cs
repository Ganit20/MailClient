using MailClient.Model;
using MailClient.ViewModel;
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

namespace MailClient.View.LoginWindow
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public static ConfigModel Config;
        public static User user;
        MainWindow Window;
        public Login(MainWindow w)
        {
            Window = w;
            InitializeComponent();
            user = new RememberMe().Load();

            if (user != null)
            {
                Email.Text = user.Mail;
                Password.Password = user.Password;
            }
            Config = new Configure().LoadConfig();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = new User()
            {
                Mail = Email.Text,
                Password = Password.Password
            };
            new ViewModel.Login().ToMail(user, Window,this);
            if (Remember.IsChecked.Value)
            {
                new RememberMe().Save(user);
            }
            
        }

        private void OpenConfig(object sender, RoutedEventArgs e)
        {
            Window.LoginPage.Navigate(new Config(Config,Window));
        }
    }
}
