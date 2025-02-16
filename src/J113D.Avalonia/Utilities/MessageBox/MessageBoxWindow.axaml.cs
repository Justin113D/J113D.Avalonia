using Avalonia.Controls;
using Avalonia.Interactivity;

namespace J113D.Avalonia.MessageBox
{
	/// <summary>
	/// A message box window
	/// </summary>
	public partial class MessageBoxWindow : J113D.Avalonia.Controls.Window
	{

		/// <summary>
		/// Creates a new empty message box window
		/// </summary>
		public MessageBoxWindow() { }

		/// <summary>
		/// Creates a new message box window
		/// </summary>
		/// <param name="title">The window title</param>
		/// <param name="message">The window message</param>
		/// <param name="buttons">Which buttons to provide</param>
		/// <param name="icon">Which icon to use</param>
		public MessageBoxWindow(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			InitializeComponent();
			Classes.Add(buttons.ToString());
			Classes.Add(icon.ToString());
			Title = title;
			MessageText.Text = message;
		}

		private void OnOk(object? sender, RoutedEventArgs e)
		{
			Close(MessageBoxResult.Ok);
		}

		private void OnYes(object? sender, RoutedEventArgs e)
		{
			Close(MessageBoxResult.Yes);
		}

		private void OnNo(object? sender, RoutedEventArgs e)
		{
			Close(MessageBoxResult.No);
		}

		private void OnCancel(object? sender, RoutedEventArgs e)
		{
			Close(MessageBoxResult.Cancel);
		}

		private void OnAbort(object? sender, RoutedEventArgs e)
		{
			Close(MessageBoxResult.Abort);
		}
	}
}