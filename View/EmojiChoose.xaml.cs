using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MailClient.View
{
    /// <summary>
    /// Interaction logic for EmojiChoose.xaml
    /// </summary>
    public partial class EmojiChoose : Window
    {
        public EmojiChoose(string[] emoji)
        {
            InitializeComponent();
            LoadEmoji(emoji);
        }
        void LoadEmoji(string[] emoji)
        {
            int row = 0;
            int column = 0;
            List<Button> b = new List<Button>();
           for(var i=0; i<emoji.Length;i++)
            {
                Button em = new Button();
                em.Content = emoji[i];
                em.Click += ClickEmoji;
                em.Height=25;
                em.Width = 25;
                b.Add(em);
                EmojiListData.Items.Add(em);
            }
            
        }
                private void ClickEmoji(object sender, RoutedEventArgs e)
        {
        }
    }
}
