using System;

namespace ApplicationProject.Views
{
    public class PageTabSelectedEventArgs : EventArgs
    {
        public enum PageTab
        {
            INCOME,
            EXPENDITURE
        }

        public PageTab Tab { get; }

        public PageTabSelectedEventArgs(PageTab tab)
        {
            Tab = tab;
        }
    }
}
