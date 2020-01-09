using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MailClient.Model
{
    public class ApplicationSettings
    {
        public int TopToolbarFontSize { get; set; }
        public int DefaultLoadValue { get; set; }
        public bool OpenMailInfoBar { get; set; }
        public int DefaultNewMessageFontSize { get; set; }
        public bool CopyMailBodyToReply { get; set; }
        public Brush color { get; set; } 
        public byte RedColor { get; set; }
        public byte GreenColor { get; set; }
        public byte BlueColor { get; set; }
    }
}
   
