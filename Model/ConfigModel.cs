using System;
using System.Collections.Generic;
using System.Text;

namespace MailClient.Model
{
    public class ConfigModel
    {
        public int ImapPort { get; set; }
        public string ImapServer { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
    }
}
