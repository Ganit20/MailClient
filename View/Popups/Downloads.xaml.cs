using MailClient.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
  
    public partial class Downloads : Window
    {

        public Downloads()
        {
            InitializeComponent();
            //update();
        }
        //void update()
        //{
        //    Dispatcher.Invoke(() =>
        //    {
        //        do
        //        {
        //            Progress.Value = Model.DownloadData.percent;
        //            Transferred.Text = Model.DownloadData.bytes;
        //        }
        //        while (Progress.Value != Progress.Maximum);
        //    });
            
        //}
    }
}
