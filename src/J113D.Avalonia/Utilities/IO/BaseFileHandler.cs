using Avalonia.Controls;
using Avalonia.Platform.Storage;
using J113D.Avalonia.MessageBox;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace J113D.Avalonia.Utilities.IO
{
	/// <summary>
	/// A filehandler class for opening and saving files
	/// </summary>
	public abstract class BaseFileHandler
	{
		/// <summary>
		/// Which file types this handler should use
		/// </summary>
		public abstract IReadOnlyList<FilePickerFileType>? FileType { get; }

		/// <summary>
		/// The file type display name
		/// </summary>
		protected abstract string FileTypeName { get; }

		/// <summary>
		/// Which window to open the dialogs on
		/// </summary>
		protected abstract Window Window { get; }

		/// <summary>
		/// Whether to ask for a close confirmation
		/// </summary>
		public virtual bool AskForCloseConfirmation => true;

		/// <summary>
		/// Whether the file handler has any unsaved changes
		/// </summary>
		public abstract bool HasUnsavedChanges { get; }

		internal async Task FailedToLoadFileBox(string message)
		{
			_ = await Window.MessageBoxDialog(
				"Failed to load file!",
				$"Failed to load {FileTypeName}:\n{message}",
				MessageBoxButtons.Ok,
				MessageBoxIcon.Error);
		}

		internal async Task FailedToSaveFileBox(string message)
		{
			_ = await Window.MessageBoxDialog(
				"Failed to save file!",
				$"Failed to save {FileTypeName}:\n{message}",
				MessageBoxButtons.Ok,
				MessageBoxIcon.Error);
		}

		/// <summary>
		/// Ask the user whether to save the data before proceeding. Saves data on confirmation.
		/// </summary>
		/// <returns>Whether to close the app (or similar)</returns>
		public async Task<bool> CloseConfirmation()
		{
			if(!AskForCloseConfirmation || !HasUnsavedChanges)
			{
				return true;
			}

			MessageBoxResult? result = await Window.MessageBoxDialog(
				"Warning!",
				"You have unsaved changes!\nDo you want to save?",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Warning);

			switch(result)
			{
				case MessageBoxResult.Yes:
				case MessageBoxResult.Ok:
					return await OnCloseConfirm();
				case MessageBoxResult.No:
					return true;
				case MessageBoxResult.Abort:
				case MessageBoxResult.Cancel:
				case null:
				default:
					return false;
			}
		}

		/// <summary>
		/// Action performed when confirming the close message
		/// </summary>
		/// <returns>Whether to close the app (or similar)</returns>
		protected abstract Task<bool> OnCloseConfirm();
	}
}
