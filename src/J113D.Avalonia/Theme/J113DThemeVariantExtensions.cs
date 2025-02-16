using Avalonia;
using Avalonia.Styling;
using System;

namespace J113D.Avalonia.Theme
{
	/// <summary>
	/// Extension methods for <see cref="J113DThemeVariant"/> values
	/// </summary>
	public static class J113DThemeVariantExtensions
	{
		/// <summary>
		/// Applies a <see cref="J113DThemeVariant"/> to an app. Applies it to the current app if none was specified.
		/// </summary>
		/// <param name="themeVariant">The theme to apply</param>
		/// <param name="application">The application to apply the theme to. Uses the current application if null</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void ApplyTheme(this J113DThemeVariant themeVariant, Application? application = null)
		{
			application ??= Application.Current;

			if(application == null)
			{
				throw new InvalidOperationException("No active application!");
			}

			application.RequestedThemeVariant = themeVariant switch
			{
				J113DThemeVariant.Light => ThemeVariant.Light,
				J113DThemeVariant.Dark => ThemeVariant.Dark,
				_ => ThemeVariant.Dark,
			};
		}
	}
}
