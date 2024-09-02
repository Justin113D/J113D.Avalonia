using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

namespace J113D.Avalonia.Theme
{
    public class SCRTheme : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SCRTheme"/> class.
        /// </summary>
        /// <param name="sp">The parent's service provider.</param>
        public SCRTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
}
