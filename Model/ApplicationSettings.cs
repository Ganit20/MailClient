using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace MailClient.Model
{
    public class ApplicationSettings : INotifyPropertyChanged
    {
        int topToolbarFontSize;
        public int TopToolbarFontSize {
            get
            {
                return topToolbarFontSize;
            }
            set
            {
                topToolbarFontSize = value;
                RaisePropertyChanged("TopToolbarFontSize");
            }
        }
        public int DefaultLoadValue { get; set; }
        public bool OpenMailInfoBar { get; set; }
        public int DefaultNewMessageFontSize { get; set; }
        public bool CopyMailBodyToReply { get; set; }
        private Brush Color;
        public Brush color { 
            get 
            { 
                return Color; 
            } 
            set
            {
               Color = value;
                RaisePropertyChanged("color");
            }
        } 
        public byte RedColor { get; set; }
        public byte GreenColor { get; set; }
        public byte BlueColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
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
   
