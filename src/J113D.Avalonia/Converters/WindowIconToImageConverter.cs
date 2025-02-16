using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace J113D.Avalonia.Converters
{
	/// <summary>
	/// Value converter for converting the window icon to an image
	/// </summary>
	public class WindowIconToImageConverter : IValueConverter
	{
		/// <summary>
		/// A static instance of the converter
		/// </summary>
		public static readonly WindowIconToImageConverter Instance = new();

		/// <inheritdoc/>
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if(value is not WindowIcon icon)
			{
				throw new ArgumentException("Value is not a window icon", nameof(value));
			}

			using(MemoryStream stream = new())
			{
				icon.Save(stream);
				stream.Position = 0;
				return new Bitmap(stream);
			}
		}

		/// <inheritdoc/>
		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
