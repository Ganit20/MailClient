using System.Collections.ObjectModel;

namespace MailClient.Model
{
    internal class Folders
    {
        public string Name { get; set; }
        public int id { get; set; }
        public static ObservableCollection<Message> Folder = new ObservableCollection<Message>();

    }
}
