using MailClient.Model;
using MailClient.View;
using MailClient.ViewModel;
using System.Threading.Tasks;
using System.Windows;


namespace MailClient
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            LoginPage.Navigate(new View.LoginWindow.Login(this));
        }


    }
}
