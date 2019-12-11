using MailClient.Model;
using MailClient.View;
using MailClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MailClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ConfigModel Config;
        public MainWindow()
        {
            InitializeComponent();
            Config = new Configure().LoadConfig();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            new Login().ToMail(new User() { Mail = Email.Text, Password = Password.Password }, this);
                
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Config cfg = new Config(Config);
            cfg.ShowDialog();
        }
    }
}
