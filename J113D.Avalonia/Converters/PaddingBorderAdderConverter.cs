using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Converters
{
    public class PaddingBorderAdderConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is not TemplatedControl control
                ? new Thickness()
                : (object)new Thickness(
                    control.Padding.Left + control.BorderThickness.Left,
                    control.Padding.Top + control.BorderThickness.Top,
                    control.Padding.Right + control.BorderThickness.Right,
                    control.Padding.Bottom + control.BorderThickness.Bottom);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
