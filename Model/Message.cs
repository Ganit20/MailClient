using MailKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace MailClient.Model
{


    public class Message : INotifyPropertyChanged
    {
        public static ObservableCollection<Message> Mails = new ObservableCollection<Message>();

        public static List<UniqueId> SelectedMails = new List<UniqueId>();

        public event PropertyChangedEventHandler PropertyChanged;
       
        public UniqueId UniqueID { get; set; }
        public int ID { get; set; }
        public string From { get; set; }
        public string FullFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Time { get; set; }
        public string To { get; set; }
        public string Hour { get; set; }
        public bool Opened { get; set; }
        public List<string> Atachment { get; set; }
        public Brush MessageColor { get; set; }
        public DateTimeOffset FullTime { get; set; }
        public Visibility IsLoadMore { get; set; }
        private bool isSelected;
        private bool isFavorite;
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                isFavorite = value;
                RaisePropertyChanged("IsSelected");
            }

        }
        public bool IsSelected
        {
            get { return isSelected; } 
            set { isSelected = value;
                RaisePropertyChanged("IsSelected");
            }

        }
        private void RaisePropertyChanged(string propName)
        {
            try
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
            catch (Exception) { }
        }
        public static ObservableCollection<Message> GetMailList()
        {
            return Mails;
        }
        public void AddMail(Message mail)
        {
            Mails.Add(mail);
        }
        
    }


}


