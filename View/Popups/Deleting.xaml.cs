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
    /// Interaction logic for Deleting.xaml
    /// </summary>
    public partial class Deleting : Window
    {
        public bool result { get; private set; }
        public Deleting(string text)
        {
           
            InitializeComponent();
            Question.Text = text;
        }

        private void Yes(object sender, RoutedEventArgs e)
        {
            result = true;
            this.Close();
        }
        private void No(object sender, RoutedEventArgs e)
        {
            result = false;
            this.Close();
        }

    }
}
