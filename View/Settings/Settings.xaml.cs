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
        Button previous;
        public Settings(MailWindow page)
        {
            InitializeComponent();
        }

        private void changeColor(Button New)
        {
            New.Foreground = new SolidColorBrush(Color.FromRgb(0,0,255));
            if (previous != null)
            {
                previous.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            previous = New;
        }

        private void Graphic_click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Navigate(new GraphicSettings(this));
            changeColor((Button)sender);
        }

        private void info_click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Navigate(new InfoConfig());
            changeColor((Button)sender);
        }
    }
}
