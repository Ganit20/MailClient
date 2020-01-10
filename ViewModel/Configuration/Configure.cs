using MailClient.Model;
using Newtonsoft.Json;
using System.IO;

namespace MailClient.ViewModel
{
    internal class Configure
    {
        public void SaveConfig(ConfigModel conf)
        {

            string config = JsonConvert.SerializeObject(conf);

            File.WriteAllText("config.cfg", config);

        }
        public ConfigModel LoadConfig()
        {
            ConfigModel config = null;
            if (File.Exists("config.cfg"))
            {
                using (StreamReader open = new StreamReader("config.cfg"))
                {
                    string cfg = open.ReadLine();
                    if (!string.IsNullOrEmpty(cfg))
                    {
                        config = JsonConvert.DeserializeObject<ConfigModel>(cfg);

                    }
                }
            }
            return config;

        }

    }
}

