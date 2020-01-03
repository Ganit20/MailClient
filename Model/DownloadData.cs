using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace MailClient.Model
{
    class DownloadData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static string bytes { get; set; }
        private static double Percent;
           public double percent
        {
            get { return Percent; }
            set
            {
                Percent = value;
                RaisePropertyChanged("Percent");
            }
        }
        public static int Max = 100;
       
               

            private void RaisePropertyChanged(string propName)
        {
            try
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
            catch (Exception) { }
        }
    }
}
