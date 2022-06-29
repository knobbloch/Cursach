using System;
using System.Globalization;
using System.Windows.Data;

using ApplicationProjectViews;

namespace ApplicationProject.UserControls
{
    internal class ValueErrorTypeLocale : IValueConverter
    {
        protected const string ErrorEmptyValueKey = "VALUE_INPUT_ERROR_EMPTY_VALUE";
        protected const string ErrorDuplicateValueKey = "VALUE_INPUT_ERROR_DUPLICATE_VALUE";
        protected const string ErrorInvalidSymbolKey = "VALUE_INPUT_ERROR_INVALID_SYMBOL";
        protected const string ErrorValueOutOfBoundsKey = "VALUE_INPUT_ERROR_OUT_OF_BOUNDS";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ValueInputError error)
                return "";

            return Resources.Locale.ResourceManager.GetString(error.ErrorType switch
            {
                ValueInputError.ValueInputErrorType.EmptyValue => ErrorEmptyValueKey,
                ValueInputError.ValueInputErrorType.DuplicateValue => ErrorDuplicateValueKey,
                ValueInputError.ValueInputErrorType.InvalidSymbol => ErrorInvalidSymbolKey,
                ValueInputError.ValueInputErrorType.OutOfBoundsValue => ErrorValueOutOfBoundsKey,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            }) + error.ErrorInfo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}