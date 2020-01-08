using System;
using System.Collections.Generic;
using System.Text;

namespace MailClient.Model
{
    class ApplicationSettings
    {
      public int TopToolbarFontSize { get; set; }
      public int DefaultLoadValue { get; set; }
        public bool OpenMailInfoBar { get; set; }
        public int DefaultNewMessageFontSize { get; set; }
        public bool CopyMailBodyToReply { get; set; }
       
    }
}
