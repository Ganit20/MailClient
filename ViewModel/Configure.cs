using MailClient.Model;
using MailClient.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MailClient.ViewModel
{
    class Configure
    {
       public void SaveConfig(ConfigModel conf)
        {

            var config = JsonConvert.SerializeObject(conf);

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

