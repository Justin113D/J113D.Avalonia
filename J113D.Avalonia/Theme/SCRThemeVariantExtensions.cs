using Avalonia;
using Avalonia.Styling;
using System;

namespace J113D.Avalonia.Theme
{
    public static class SCRThemeVariantExtensions
    {
        public static void ApplyTheme(this SCRThemeVariant themeVariant, Application? application = null)
        {
            application ??= Application.Current;

            if(application == null)
            {
                throw new InvalidOperationException("No active application!");
            }

            application.RequestedThemeVariant = themeVariant switch
            {
                SCRThemeVariant.Light => ThemeVariant.Light,
                _ => ThemeVariant.Dark,
            };
        }
    }
}
