using System;

namespace ApplicationProjectViews.DatedPageView
{
    public delegate void DateRangeTypeSelectedEventHandler(object source, DateRangeTypeSelectedEventArgs e);

    public class DateRangeTypeSelectedEventArgs : EventArgs
    {
        public DateRangeTypeSelectedEventArgs(DateRangeType type) : base()
        {
            RangeType = type;
        }

        public DateRangeType RangeType { get; init; }
    }
}
