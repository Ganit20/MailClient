using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MailClient.Model
{


    public class Message
    {
     public static ObservableCollection<Message> Mails = new ObservableCollection<Message>();


        public int ID { get; set; }
        public string From { get; set; }
        public string FullFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Time { get; set; }
        public string Hour { get; set; }
        public DateTimeOffset FullTime { get; set; }

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


