using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Utilities.Enum
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is System.Enum enumValue
                ? enumValue.ToDescription()
                : throw new ArgumentException("Convert: Value must be an enum.");
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is EnumDescription enumDescription
                ? enumDescription.Value
                : throw new ArgumentException("ConvertBack: EnumDescription must be an enum.");
        }
    }
}
