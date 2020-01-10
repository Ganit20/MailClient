using MailClient.View;
using MailClient.View.InboxWindow;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailClient.Model
{
    class MailCreate
    {
        public void AddAllign(NewMessage Page,string position)
        {
            var sel = Page.SendBody.SelectionStart;
            var selleng = Page.SendBody.SelectionLength;
            var html = "<div style='text-align: "+position + "'> ";
            Page.SendBody.Text = Page.SendBody.Text.Insert(sel + selleng, "</div> ");
            Page.SendBody.Text = Page.SendBody.Text.Insert(sel, html);
            Page.SendBody.SelectionStart = sel + html.Length + selleng;
            Page.SendBody.SelectionLength = 0;
        }
        public void AddMarkup(NewMessage Page, string Markup,string MarkupEnd)
        {
            var sel = Page.SendBody.SelectionStart;
            var selleng = Page.SendBody.SelectionLength;
            var html = Markup;
            Page.SendBody.Text = Page.SendBody.Text.Insert(sel + selleng, MarkupEnd);
            Page.SendBody.Text = Page.SendBody.Text.Insert(sel, html);
            Page.SendBody.SelectionStart = sel + html.Length + selleng;
            Page.SendBody.SelectionLength = 0;
        }
    }
}
