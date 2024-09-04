using Avalonia;
using Avalonia.Platform;
using System;

namespace J113D.Avalonia.Controls
{
    public class Window : global::Avalonia.Controls.Window
    {
        public static readonly StyledProperty<bool> HideMinimizeButtonProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(HideMinimizeButton));

        public static readonly StyledProperty<bool> HideCloseButtonProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(HideCloseButton));

        public static readonly StyledProperty<bool> HideRestoreButtonProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(HideRestoreButton));

        public bool HideMinimizeButton
        {
            get => GetValue(HideMinimizeButtonProperty);
            set => SetValue(HideMinimizeButtonProperty, value);
        }

        public bool HideCloseButton
        {
            get => GetValue(HideCloseButtonProperty);
            set => SetValue(HideCloseButtonProperty, value);
        }

        public bool HideRestoreButton
        {
            get => GetValue(HideRestoreButtonProperty);
            set => SetValue(HideRestoreButtonProperty, value);
        }

        protected override Type StyleKeyOverride => typeof(Window);

        public Window() : base()
        {

        }

        public Window(IWindowImpl impl) : base(impl)
        { 
        
        }
    }
}
