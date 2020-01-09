using MailClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;

namespace MailClient.ViewModel
{
    class LoadSettings
    {
        public ApplicationSettings Read()
        {
            if (File.Exists("AppConfig.cfg")){
                using (StreamReader Reader = new StreamReader("AppConfig.cfg"))
                {
                    var settings = Reader.ReadToEnd();
                    var DeSettings = JsonConvert.DeserializeObject<ApplicationSettings>(settings);
                    return DeSettings;
                } }else
            {
                var set = CreateSettings();
                return set;
            }
        }
        public ApplicationSettings GetDefaultSettings()
        {
            return new ApplicationSettings()
            {
                TopToolbarFontSize = 24,
                DefaultLoadValue = 1,
                OpenMailInfoBar = true,
                DefaultNewMessageFontSize = 14,
                CopyMailBodyToReply = true,
                color = new SolidColorBrush(Color.FromRgb(255, 203, 57)),
                RedColor = 255,
                GreenColor = 203,
                BlueColor=57

            };
        }
        public ApplicationSettings CreateSettings()
        {
            using (StreamWriter SaveSet = new StreamWriter("AppConfig.cfg"))
            {
                var def = GetDefaultSettings();
                SaveSet.Write(JsonConvert.SerializeObject(def));
                return def;
            }
        }
    }
}
