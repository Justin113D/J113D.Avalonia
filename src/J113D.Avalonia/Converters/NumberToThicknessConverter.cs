using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace J113D.Avalonia.Converters
{
	/// <summary>
	/// Value convert for converting numbers to thickness
	/// </summary>
	public class NumberToThicknessConverter : IValueConverter
	{
		/// <summary>
		/// Whether the number should be used for the top thickness
		/// </summary>
		public bool Top { get; set; }

		/// <summary>
		/// Whether the number should be used for the bottom thickness
		/// </summary>
		public bool Bottom { get; set; }

		/// <summary>
		/// Whether the number should be used for the right thickness
		/// </summary>
		public bool Right { get; set; }

		/// <summary>
		/// Whether the number should be used for the left thickness
		/// </summary>
		public bool Left { get; set; }

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
