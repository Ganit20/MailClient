using MailClient.Model;
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
using System.Windows.Shapes;

namespace MailClient.View
{
    /// <summary>
    /// Interaction logic for FoldeChoose.xaml
    /// </summary>
    public partial class FolderChoose : Window
    {
        public FolderChoose()
        {
            InitializeComponent();
            FolderSelect.ItemsSource = Folder.Folders;
        }



        private void close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
