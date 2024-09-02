using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Converters
{
    public class NullToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value ?? string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value ?? string.Empty;
        }
    }
}
