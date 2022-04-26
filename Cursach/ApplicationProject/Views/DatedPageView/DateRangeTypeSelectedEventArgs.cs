using System;

namespace ApplicationProject.Views.DatedPageView
{
    public class DateRangeTypeSelectedEventArgs : EventArgs
    {
        public DateRangeTypeSelectedEventArgs(DateRangeType type)
        {
            RangeType = type;
        }

        public DateRangeType RangeType { get; }
    }
}
