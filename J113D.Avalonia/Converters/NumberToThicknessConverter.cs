using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Converters
{
    public class NumberToThicknessConverter : IValueConverter
    {
        public bool Top { get; set; }
        public bool Bottom { get; set; }
        public bool Right { get; set; }
        public bool Left { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            double? number = value as double?;

            if(!number.HasValue || double.IsNaN(number.Value))
            {
                return new Thickness(0);
            }

            if(!Top && !Bottom && !Right && !Left)
            {
                return new Thickness(number.Value);
            }
            else
            {
                double left = 0, top = 0, right = 0, bottom = 0;

                if(Top)
                {
                    top = number.Value;
                }

                if(Bottom)
                {
                    bottom = number.Value;
                }

                if(Left)
                {
                    left = number.Value;
                }

                if(Right)
                {
                    right = number.Value;
                }

                return new Thickness(left, top, right, bottom);
            }


        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
