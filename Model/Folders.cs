using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MailClient.Model
{
    class Folders
    {
        public string Name { get; set; }
        public int id { get; set; }
        public static ObservableCollection<Message> Folder = new ObservableCollection<Message>();

    }
}
