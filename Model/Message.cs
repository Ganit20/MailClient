using System;
using System.Collections.Generic;
using System.Text;

namespace MailClient.Model
{
    class Message
    {
        public int ID { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Time { get; set; }
        
    }
}
