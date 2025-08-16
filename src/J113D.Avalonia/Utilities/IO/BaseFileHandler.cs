using Avalonia.Controls;
using Avalonia.Platform.Storage;
using J113D.Avalonia.MessageBox;
using System;
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
		/// The file change tracker
		/// </summary>
		protected abstract IFileChangeTracker? FileChangeTracker { get; }

		/// <summary>
		/// The path to the loaded file
		/// </summary>
		public Uri? LoadedFilePath { get; private set; }

		/// <summary>
		/// Whether to ask for a reset confirmation
		/// </summary>
		protected virtual bool AskForResetConfirmation => true;

		private async Task FailedToLoadFileBox(string message)
		{
			_ = await Window.MessageBoxDialog(
				"Failed to load file!",
				$"Failed to load {FileTypeName}:\n{message}",
				MessageBoxButtons.Ok,
				MessageBoxIcon.Error);
		}

		/// <summary>
		/// Ask the user whether to save the data before proceeding. Saves data on confirmation.
		/// </summary>
		/// <returns>Whether the user wants proceed to the next action</returns>
		public async Task<bool> ResetConfirmation()
		{
			if(AskForResetConfirmation && FileChangeTracker?.HasFileChanged != false)
			{
				MessageBoxResult? result = await Window.MessageBoxDialog(
					"Warning!",
					"Unsaved changes will be lost!\nDo you want to save before?",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Warning);

				switch(result)
				{
					case MessageBoxResult.Yes:
					case MessageBoxResult.Ok:
						if(!await Save(false))
						{
							return false;
						}

						break;
					case MessageBoxResult.No:
						break;
					case MessageBoxResult.Abort:
					case MessageBoxResult.Cancel:
					case null:
					default:
						return false;
				}

				return true;
			}

			return true;
		}

		/// <summary>
		/// Open a save file dialog and save the file to the selected path
		/// </summary>
		/// <param name="newPath">Whether a new path should be chosen, even if one was already stored ("save as")</param>
		/// <returns>Whether the file was succesfully saved</returns>
		public async Task<bool> Save(bool newPath)
		{
			if(LoadedFilePath == null || newPath)
			{
				IStorageFile? file = await Window.StorageProvider.SaveFilePickerAsync(new()
				{
					Title = $"Save {FileTypeName} to File",
					ShowOverwritePrompt = true,
					FileTypeChoices = FileType
				});

				return file != null && await SaveNoDialog(file.Path);
			}
			else
			{
				return await SaveNoDialog(LoadedFilePath);
			}
		}

		/// <summary>
		/// Save the file without showing a dialog
		/// </summary>
		/// <param name="filepath">The path to save the file to</param>
		/// <returns>Whether the file was successfully saved</returns>
		public async Task<bool> SaveNoDialog(Uri filepath)
		{
			try
			{
				InternalSave(filepath);
			}
			catch(Exception e)
			{
				await FailedToLoadFileBox(e.Message);
				return false;
			}

			LoadedFilePath = filepath;
			FileChangeTracker?.StoreCurrentState(false);
			return true;
		}

		/// <summary>
		/// Open an open file dialog and open the selected file
		/// </summary>
		/// <returns>Whether a new file was opened</returns>
		public async Task<bool> Open()
		{
			if(!await ResetConfirmation())
			{
				return false;
			}

			IReadOnlyList<IStorageFile> files = await Window.StorageProvider.OpenFilePickerAsync(new()
			{
				Title = $"Load from {FileTypeName} File",
				AllowMultiple = false,
				FileTypeFilter = FileType
			});

			if(files == null || files.Count == 0)
			{
				return false;
			}

			return await OpenNoDialog(files[0].Path);
		}

		/// <summary>
		/// Open a file without showing a dialog
		/// </summary>
		/// <param name="filepath">The path to the file to open</param>
		/// <returns>Whether the file was successfully opened</returns>
		public async Task<bool> OpenNoDialog(Uri filepath)
		{
			try
			{
				InternalLoad(filepath);
			}
			catch(Exception e)
			{
				await FailedToLoadFileBox(e.Message);
				return false;
			}

			LoadedFilePath = filepath;
			FileChangeTracker?.StoreCurrentState(true);
			return true;
		}

		/// <summary>
		/// Reset the current data
		/// </summary>
		/// <returns>Whether the data was reset</returns>
		public async Task<bool> Reset()
		{
			if(!await ResetConfirmation())
			{
				return false;
			}

			InternalReset();
			LoadedFilePath = null;
			FileChangeTracker?.StoreCurrentState(true);
			return true;
		}

		/// <summary>
		/// Clear the file change tracker and loaded file path
		/// </summary>
		public void ForgetFilePath()
		{
			LoadedFilePath = null;
		}


		/// <summary>
		/// Reset implementation
		/// </summary>
		protected abstract void InternalReset();

		/// <summary>
		/// Save to file implementation
		/// </summary>
		/// <param name="filePath">Path to save to</param>
		protected abstract void InternalSave(Uri filePath);

		/// <summary>
		/// Open file implementation
		/// </summary>
		/// <param name="filePath">Path to the file to open</param>
		protected abstract void InternalLoad(Uri filePath);

	}
}
