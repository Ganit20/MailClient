using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MailClient.Model
{
    class ReadEmoji
    {
        public string[] Read()
        {
            string[] lines =  File.ReadAllLines("EmojiList/Emoji.mc");
            for(var i=0;i<lines.Length;i++)
            {
                lines[i] = lines[i].Insert(0, "&#");
                lines[i] = lines[i] + ";"; 
            }
            return lines;
        }

        }
}
