using System.Collections.ObjectModel;

namespace MailClient.Model
{
    public class ConfigModel
    {
        public int ImapPort { get; set; }
        public string ImapServer { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public static ObservableCollection<int> LoadNumber = new ObservableCollection<int>();
        public ConfigModel()
        { 
            LoadNumber.Add(0);
            LoadNumber.Add(10);
            LoadNumber.Add(25);
            LoadNumber.Add(50);
            LoadNumber.Add(75);
            LoadNumber.Add(100);
            LoadNumber.Add(200);
        }

    }
}