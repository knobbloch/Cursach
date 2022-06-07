using System;
using System.Collections.Generic;

namespace ApplicationProjectViews.DatedPageView
{
    public interface IDatedPageView : IBaseView
    {
        /// <summary>
        /// Is called when the date range type is changed for this page
        /// </summary>
        event DateRangeTypeSelectedEventHandler DateRangeTypeSelected;

        /// <summary>
        /// Is called when a date range is selected for this page
        /// </summary>
        event EventHandler SelectedDateRangeChanged;

        /// <summary>
        /// Is called when the next date range should be set as active
        /// </summary>
        event EventHandler NextDateRangeSelected;

        /// <summary>
        /// Is called when the previous date range should be set as active
        /// </summary>
        event EventHandler PreviousDateRangeSelected;

        /// <summary>
        /// Updates the displayed date range
        /// </summary>
        DateRange SelectedDateRange { set; }

        /// <summary>
        /// Sets the allowed bounds for DateRange
        /// Set to null to disable
        /// </summary>
        DateRange? DateRangeBounds { set; }

        /// <summary>
        /// All available date range for the user to select from
        /// </summary>
        ICollection<DateRangeType> DateRangeTypes { get; }

        /// <summary>
        /// Sets the range type to be used
        /// </summary>
        DateRangeType SelectedRangeType { set; }
    }
}
