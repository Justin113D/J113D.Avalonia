using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace J113D.Avalonia.Utilities.Enum
{
	/// <summary>
	/// Utilities for dynamically using enums in avalonia
	/// </summary>
	public static class EnumUtils
	{
		/// <summary>
		/// Obtains enum descriptions from an enum type
		/// </summary>
		/// <param name="type">The enum type to get descriptions of</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static IEnumerable<EnumDescription> ToDescriptions(this Type type)
		{
			if(!type.IsEnum)
			{
				throw new ArgumentException($"{nameof(type)} must be an enum type");
			}

			return System.Enum
				.GetValues(type)
				.Cast<System.Enum>()
				.Select(ToDescription)
				.ToList();
		}

		/// <summary>
		/// Obtains enum descriptions from an enum type
		/// </summary>
		/// <typeparam name="T">The enum type to get descriptions of</typeparam>
		/// <returns></returns>
		public static IEnumerable<EnumDescription> ToDescriptions<T>() where T : struct, System.Enum
		{
			return System.Enum
				.GetValues<T>()
				.Select(x => ToDescription(x))
				.ToList();
		}

		/// <summary>
		/// Obtains the description of an enum value
		/// </summary>
		/// <param name="value">The enum value to obtain the description of</param>
		/// <returns></returns>
		public static EnumDescription ToDescription(this System.Enum value)
		{
			string description;

			IEnumerable<DescriptionAttribute> attributes = value.GetType().GetField(value.ToString())!.GetCustomAttributes<DescriptionAttribute>(false);

			if(attributes.Any())
			{
				description = attributes.First().Description;
			}
			else
			{
				TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
				description = ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
			}

			return new EnumDescription(value, description);
		}
	}
}
