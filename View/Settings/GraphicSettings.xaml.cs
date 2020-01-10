using MailClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MailClient.View
{

    public partial class GraphicSettings : Page
    {
      
        ApplicationSettings oldSettings = new ApplicationSettings();
        MailWindow MainWindow;
        Settings Window;
        public GraphicSettings(Settings w)
        {
            Window = w;
            InitializeComponent();
        oldSettings = ActualSettings.Actual;
            this.DataContext = ActualSettings.Actual;
            LoadDefNumber.ItemsSource = ConfigModel.LoadNumber;
        }

    private void FontTopChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        Window.Close();
    }

    private void Save(object sender, RoutedEventArgs e)
    {
        ApplicationSettings newSettings = new ApplicationSettings();
        newSettings = ActualSettings.Actual;
        string settings = JsonConvert.SerializeObject(newSettings);
        using (StreamWriter write = new StreamWriter("AppConfig.cfg"))
        {
            write.Write(settings);
        }
        Window.Close();
    }

    private void LoadChange(object sender, RoutedEventArgs e)
    {
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
