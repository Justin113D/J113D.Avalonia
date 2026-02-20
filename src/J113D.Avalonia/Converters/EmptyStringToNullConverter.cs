using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Converters
{
	/// <summary>
	/// Value converter for converting a whitespace string to null
	/// </summary>
	public class EmptyStringToNullConverter : IValueConverter
	{
		/// <inheritdoc/>
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return  value ?? string.Empty;
		}

		/// <inheritdoc/>
		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if(value == null)
			{
				return null;
			}
			else if(value is string stringvalue)
			{
				return string.IsNullOrWhiteSpace(stringvalue) ? null : value;
			}
			else
			{
				return value;
			}
		}
	}
}
