using Avalonia;
using Avalonia.Platform;

namespace J113D.Avalonia.Controls
{
	/// <summary>
	/// An extended window class
	/// </summary>
	public class Window : global::Avalonia.Controls.Window
	{
		/// <summary>
		/// The <see cref="HideMinimizeButton"/> property
		/// </summary>
		public static readonly StyledProperty<bool> HideMinimizeButtonProperty =
			AvaloniaProperty.Register<Window, bool>(nameof(HideMinimizeButton));

		/// <summary>
		/// The <see cref="HideCloseButton"/> property
		/// </summary>
		public static readonly StyledProperty<bool> HideCloseButtonProperty =
			AvaloniaProperty.Register<Window, bool>(nameof(HideCloseButton));

		/// <summary>
		/// The <see cref="HideRestoreButton"/> property
		/// </summary>
		public static readonly StyledProperty<bool> HideRestoreButtonProperty =
			AvaloniaProperty.Register<Window, bool>(nameof(HideRestoreButton));

		/// <summary>
		/// Whether to hide the minimize button
		/// </summary>
		public bool HideMinimizeButton
		{
			get => GetValue(HideMinimizeButtonProperty);
			set => SetValue(HideMinimizeButtonProperty, value);
		}

		/// <summary>
		/// Whether to hide the close button
		/// </summary>
		public bool HideCloseButton
		{
			get => GetValue(HideCloseButtonProperty);
			set => SetValue(HideCloseButtonProperty, value);
		}

		/// <summary>
		/// Whether to hide the restore button
		/// </summary>
		public bool HideRestoreButton
		{
			get => GetValue(HideRestoreButtonProperty);
			set => SetValue(HideRestoreButtonProperty, value);
		}

		/// <summary>
		/// Create a new window
		/// </summary>
		public Window() : base() { }

		/// <summary>
		/// Create a new window
		/// </summary>
		/// <param name="impl">The window implementation to use</param>
		public Window(IWindowImpl impl) : base(impl) { }
	}
}
