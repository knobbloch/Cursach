using System;

namespace ApplicationProject.Views.DatedPageView
{
    public struct DateRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public DateRange(DateTime start, DateTime end)
        {
            if(start > end)
                throw new ArgumentException("A DateRange's start should preceed its end.");

            Start = start;
            End = end;
        }
    }
}
