using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Converters
{
	/// <summary>
	/// Value converter for converting a null string to an empty string
	/// </summary>
	public class NullToStringConverter : IValueConverter
	{
		/// <inheritdoc/>
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value ?? string.Empty;
		}

		/// <inheritdoc/>
		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value ?? string.Empty;
		}
	}
}
