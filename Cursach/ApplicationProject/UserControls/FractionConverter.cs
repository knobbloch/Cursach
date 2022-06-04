using System;
using System.Windows.Data;
using System.Globalization;

namespace ApplicationProject.UserControls
{
    internal class FractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            double fraction = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);

            //Prevent infinity values for height manipulation
            if (v == double.PositiveInfinity || v == double.NegativeInfinity)
                return fraction;

            return v * fraction;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) / System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
        }
    }
}
