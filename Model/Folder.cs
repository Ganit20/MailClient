using System.Collections.ObjectModel;

namespace MailClient.Model
{
    internal class Folder
    {
        public string Name { get; set; }
        public string id { get; set; }
        public static ObservableCollection<Folder> Folders = new ObservableCollection<Folder>();

    }
}
