using System;
using System.Globalization;
using System.Windows.Data;

using ApplicationProjectViews.DatedPageView;

namespace ApplicationProject.UserControls
{
    internal class DateRangeTypeLocale : IValueConverter
    {
        protected const string DisplayNameMonthKey = "DATERANGE_TYPENAME_MONTH";
        protected const string DisplayNameYearKey = "DATERANGE_TYPENAME_YEAR";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Resources.Locale.ResourceManager.GetString((DateRangeType)value switch
            {
                DateRangeType.MONTH => DisplayNameMonthKey,
                DateRangeType.YEAR => DisplayNameYearKey,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            }, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
