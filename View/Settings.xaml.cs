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
             ApplicationSettings oldSettings = new ApplicationSettings();
        Inbox inbox;
        int i = 0;
        public Settings(Inbox page)
        {
            oldSettings = ActualSettings.Actual;
            this.DataContext = ActualSettings.Actual;
            InitializeComponent();
            LoadDefNumber.ItemsSource = ConfigModel.LoadNumber;
        }

        private void FontTopChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            ApplicationSettings newSettings =new ApplicationSettings();
            newSettings = ActualSettings.Actual;
            string settings = JsonConvert.SerializeObject(newSettings);
            using(StreamWriter write = new StreamWriter("AppConfig.cfg"))
            {
                write.Write(settings);
            }
            this.Close();
        }
        
        private void LoadChange(object sender, RoutedEventArgs e)
        {
        }

        private void SetDefault(object sender, RoutedEventArgs e)
        {
            new SettingSetters().SetDefault(inbox);
            ActualSettings.Actual = new LoadSettings().GetDefaultSettings(); 
        }

        private void ColorChange(object sender, TextChangedEventArgs e)
        {
            if (GreenBox != null && BlueBox != null && RedBox != null)
            {
                Brush newcolor = new SolidColorBrush(Color.FromRgb(Convert.ToByte(RedBox.Text), Convert.ToByte(GreenBox.Text), Convert.ToByte(BlueBox.Text)));
                ActualSettings.Actual.color = newcolor;
            }
                }
    }
}
