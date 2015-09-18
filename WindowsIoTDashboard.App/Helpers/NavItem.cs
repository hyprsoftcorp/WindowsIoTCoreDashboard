using System;

namespace WindowsIoTDashboard.App.Helpers
{
    public class NavItem
    {
        public NavItem(object glyph, string text, Type page)
        {
            Glyph = glyph;
            Text = text;
            Page = (page == null ? typeof(MainPage) : page);
        }

        public object Glyph { get; private set; }
        public string Text { get; private set; }
        public Type Page { get; private set; }
    }
}
