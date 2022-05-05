using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace ApplicationProject.UserControls
{
    public class UniformTabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            TabControl tabControl = values[0] as TabControl;
            return Math.Max(0, (tabControl.ActualWidth / tabControl.Items.Count) - tabControl.Items.Count switch { 1 => 5, 2 => 2.1, 3 => 2.1, _ => 1 });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
