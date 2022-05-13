using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.DatedPageView
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
        event DateRangeSelectedEventHandler DateRangeSelected;

        /// <summary>
        /// Is called when the button for the next date range is pressed
        /// </summary>
        event EventHandler NextDateRangeSelected;

        /// <summary>
        /// Is called when the button for the previous date range is pressed
        /// </summary>
        event EventHandler PreviousDateRangeSelected;

        /// <summary>
        /// Updates the text displayed as current date range
        /// </summary>
        DateRange DisplayedDateRange { set; }
        /// <summary>
        /// Updates the text displayed as current page's name
        /// </summary>
        string PageNameTextKey { set; }

        /// <summary>
        /// Sets the allowed bounds for DateRange
        /// Set to null to disable
        /// </summary>
        DateRange? DateRangeBounds { set; }

        /// <summary>
        /// Stores all available date range types
        /// </summary>
        ICollection<DateRangeType> DateRangeTypes { get; }

        /// <summary>
        /// Sets the range type to be used by the calendar
        /// </summary>
        DateRangeType.RangeType SelectedRangeType { set; }

        /// <summary>
        /// Used to present active page
        /// </summary>
        IViewPresenter PageViewPresenter { get; }
    }
}
