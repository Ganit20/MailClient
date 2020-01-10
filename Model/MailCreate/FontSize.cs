using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MailClient.Model
{
    class FontSize
    {
        public static ObservableCollection<int> FontSizeList = new ObservableCollection<int>();
        public void addFontSize()
        {
            FontSizeList.Add(8);
            FontSizeList.Add(9);
            FontSizeList.Add(10);
            FontSizeList.Add(12);
            FontSizeList.Add(14);
            FontSizeList.Add(16);
            FontSizeList.Add(18);
            FontSizeList.Add(20);
            FontSizeList.Add(22);
            FontSizeList.Add(24);
            FontSizeList.Add(28);
            FontSizeList.Add(36);
            FontSizeList.Add(48);
            FontSizeList.Add(72);
        }
    }
}
