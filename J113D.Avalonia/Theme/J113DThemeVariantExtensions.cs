using Avalonia;
using Avalonia.Styling;
using System;

namespace J113D.Avalonia.Theme
{
    public static class J113DThemeVariantExtensions
    {
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
                _ => ThemeVariant.Dark,
            };
        }
    }
}
