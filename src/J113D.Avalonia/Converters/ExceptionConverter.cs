using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Converters
{
	/// <summary>
	/// Value convert for converting an exception to a text
	/// </summary>
	public class ExceptionConverter : IValueConverter
	{
		/// <inheritdoc/>
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if(value is string)
			{
				return value;
			}
			else if(value is Exception exception)
			{
				return exception.Message;
			}
			else if(value is Array array && array.Length > 0)
			{
				return Convert(array.GetValue(0), targetType, parameter, culture);
			}

			return "An unknown error occured";
		}

		/// <inheritdoc/>
		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
