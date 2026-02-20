using Avalonia.Controls;
using J113D.Avalonia.MessageBox;
using System.Threading.Tasks;

namespace J113D.Avalonia.Utilities.MessageBox
{
	/// <summary>
	/// Extension methods for message box dialogs
	/// </summary>
	public static class MessageBoxExtensions
	{
		/// <summary>
		/// Opens a new message box dialog
		/// </summary>
		/// <param name="window">The window to which the dialog should be attached</param>
		/// <param name="title">The title of the dialog</param>
		/// <param name="message">The message to display</param>
		/// <param name="buttons">The buttons to display</param>
		/// <param name="icon">The icon to use</param>
		/// <returns>The result of the message box interaction</returns>
		public static Task<MessageBoxResult?> MessageBoxDialog(this Window window, string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return new MessageBoxWindow(title, message, buttons, icon)
				.ShowDialog<MessageBoxResult?>(window);
		}
	}
}
