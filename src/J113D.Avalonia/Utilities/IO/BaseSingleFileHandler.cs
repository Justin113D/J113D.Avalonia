using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace J113D.Avalonia.Utilities.IO
{
	/// <summary>
	/// A filehandler class for opening and saving single files
	/// </summary>
	public abstract class BaseSingleFileHandler : BaseFileHandler
	{
		/// <summary>
		/// The data change state
		/// </summary>
		protected abstract IDataChangeState? DataChangeState { get; }

		/// <summary>
		/// The path to the loaded file
		/// </summary>
		public Uri? LoadedFilePath { get; private set; }

		/// <inheritdoc/>
		public sealed override bool HasUnsavedChanges => DataChangeState?.HasDataChanged != false;

		/// <inheritdoc/>
		protected sealed override async Task<bool> OnCloseConfirm()
		{
			return await Save(false);
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
				await FailedToSaveFileBox(e.Message);
				return false;
			}

			LoadedFilePath = filepath;
			DataChangeState?.StoreCurrentState(false);
			return true;
		}

		/// <summary>
		/// Open an open file dialog and open the selected file
		/// </summary>
		/// <returns>Whether a new file was opened</returns>
		public async Task<bool> Open()
		{
			if(!await CloseConfirmation())
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
			DataChangeState?.StoreCurrentState(true);
			return true;
		}

		/// <summary>
		/// Reset the current data
		/// </summary>
		/// <returns>Whether the data was reset</returns>
		public async Task<bool> Reset()
		{
			if(!await CloseConfirmation())
			{
				return false;
			}

			InternalReset();
			LoadedFilePath = null;
			DataChangeState?.StoreCurrentState(true);
			return true;
		}

		/// <summary>
		/// Clear the loaded file path
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
