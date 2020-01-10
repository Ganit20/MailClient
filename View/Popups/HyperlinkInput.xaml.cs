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
    /// Interaction logic for HyperlinkInput.xaml
    /// </summary>
    public partial class HyperlinkInput : Window
    {
        public HyperlinkInput()
        {
            InitializeComponent();
        }
        public string RText { get; set; }


        public  void Generate_Hyperlink(object sender, RoutedEventArgs e)
        {
            string hyperlink = "<a href='" + Hyperlink.Text + "'>" + Text.Text + "</a>";
            RText = hyperlink;
            DialogResult = true;
            this.Close();
            
        }
    }
}
