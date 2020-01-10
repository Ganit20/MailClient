using MailClient.Model;
using MailClient.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {

        public Settings(MailWindow page)
        {
            InitializeComponent();
        }

        private void Page(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Navigate(new GraphicSettings(this));
        }
    }
}
