using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.DatedPageView;

namespace WpfLibrary
{
    public class Date
    {
        private bool dateType = false;
        private DateRange currentDataRange = new DateRange(new DateTime(2022, 6, 1), new DateTime(2022, 6, 30));

        public Date(IDatedPageView date)
        {
            date.DateRangeTypeSelected += ClickTypeSelected;
            date.SelectedDateRangeChanged += ClickDateRangeChanged;
            date.NextDateRangeSelected += ClickNEXTDateSelected;
            date.PreviousDateRangeSelected += ClickPREVIOUSDateSelected;
            date.DateRangeTypes.Add(DateRangeType.YEAR);
            date.DateRangeBounds = new DateRange(new DateTime(1970, 1, 1), new DateTime(2100, 12, 31));
            date.SelectedDateRange = new DateRange(new DateTime(2022, 6, 1), new DateTime(2022, 6, 30));

        }
        public void ClickTypeSelected(object source, DateRangeTypeSelectedEventArgs a)//месяц/год изменился
        {
            ((IDatedPageView)source).SelectedRangeType = a.RangeType;
            if (!dateType){
                dateType = true;
                ((IDatedPageView)source).SelectedDateRange = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 12, 31));
                currentDataRange = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 12, 31));
            }
            else { 
                dateType = false;
                ((IDatedPageView)source).SelectedDateRange = new DateRange(new DateTime(2022, 6, 1), new DateTime(2022, 6, 30));
                currentDataRange = new DateRange(new DateTime(2022, 6, 1), new DateTime(2022, 6, 30));
            }
        }
        public void ClickDateRangeChanged(object source, EventArgs a)//сам промежуток поменялся
        {
            currentDataRange = ((IDatedPageView)source).SelectedDateRange;
        }
        public void ClickNEXTDateSelected(object source, EventArgs a)//стрелка вправо
        {
            if (dateType) {
                currentDataRange = new DateRange(currentDataRange.Start.AddYears(1), currentDataRange.End.AddYears(1));
                ((IDatedPageView)source).SelectedDateRange = currentDataRange;
            }
            else
            {
                currentDataRange = new DateRange(currentDataRange.Start.AddMonths(1), currentDataRange.End.AddMonths(1)); 
                ((IDatedPageView)source).SelectedDateRange = currentDataRange;
            }
        }
        public void ClickPREVIOUSDateSelected(object source, EventArgs a)//стрелка влево
        {
            if (dateType)
            {
                currentDataRange = new DateRange(currentDataRange.Start.AddYears(-1), currentDataRange.End.AddYears(-1));
                ((IDatedPageView)source).SelectedDateRange = currentDataRange;
            }
            else
            {
                currentDataRange = new DateRange(currentDataRange.Start.AddMonths(-1), currentDataRange.End.AddMonths(-1));
                ((IDatedPageView)source).SelectedDateRange = currentDataRange;
            }
        }
    }  
}
