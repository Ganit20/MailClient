using MailClient.Model;
using MailClient.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailClient.ViewModel
{
    class SettingSetters
    {
        public void SetDefault(Inbox inbox)
        {

            //SetTopToolBarFontSize(inbox, DefaultSettings.TopToolbarFontSize);
            ActualSettings.Actual = new LoadSettings().GetDefaultSettings();
        }

        public void SetDefaultLoadValue(Inbox inbox,int setting)
        {
            //inbox.Load.SelectedIndex = setting;
        }       
    }
}
