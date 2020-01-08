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
            int i = 0;
            if (Emoji.EmojiList.Count == 0)
                foreach (var line in lines)
            {
                    
                switch(i)
                    {
                        case 0:          
                                Emoji.EmojiList.Add(line);
                            break;
                        case 1:
                            Emoji.EmojiList2.Add(line);
                            break;
                        case 2:
                            Emoji.EmojiList3.Add(line);
                            break;
                        case 3:
                            Emoji.EmojiList4.Add(line);
                            break;
                        case 4:
                            Emoji.EmojiList5.Add(line);
                            break;
                          
                    }
                    i++;
                    if (i > 4) i = 0;

            }
            new Emoji();
            return lines;
        }

        }
}
