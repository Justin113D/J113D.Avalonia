using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace J113D.Avalonia.Utilities.Enum
{
    public static class EnumUtils
    {
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

        public static IEnumerable<EnumDescription> ToDescriptions<T>() where T : struct, System.Enum
        {
            return System.Enum
                .GetValues<T>()
                .Select(x => ToDescription(x))
                .ToList();
        }

        public static EnumDescription ToDescription(this System.Enum value)
        {
            string description;
            string? help = null;

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

            if(description.IndexOf(';') is var index && index != -1)
            {
                help = description[(index + 1)..];
                description = description[..index];
            }

            return new EnumDescription(value, description, help);
        }
    }
}
