using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

namespace J113D.Avalonia.Theme
{
    public class J113DTheme : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="J113DTheme"/> class.
        /// </summary>
        /// <param name="sp">The parent's service provider.</param>
        public J113DTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
}
