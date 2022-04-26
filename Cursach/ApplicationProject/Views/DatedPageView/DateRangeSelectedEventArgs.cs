using System;

namespace ApplicationProject.Views.DatedPageView
{
    public class DateRangeSelectedEventArgs : EventArgs
    {
        public DateRange Range { get; }

        public DateRangeSelectedEventArgs(DateRange range)
        {
            Range = range;
        }
    }
}
