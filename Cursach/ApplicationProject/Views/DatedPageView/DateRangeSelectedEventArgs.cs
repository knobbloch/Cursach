using System;

namespace ApplicationProject.Views.DatedPageView
{
    public delegate void DateRangeSelectedEventHandler(object source, DateRangeSelectedEventArgs e);

    public class DateRangeSelectedEventArgs : EventArgs
    {
        public DateRange Range { get; }

        public DateRangeSelectedEventArgs(DateRange range) : base()
        {
            Range = range;
        }
    }
}
