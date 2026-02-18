using Avalonia.Platform.Storage;
using J113D.Avalonia.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace J113D.Avalonia.Utilities.IO
{
	/// <summary>
	/// A filehandler class for opening and saving single files
	/// </summary>
	public abstract class BaseMultiFileHandler<TFileData> : BaseFileHandler where TFileData : notnull
	{
		private readonly Dictionary<TFileData, Uri?> _files = [];

		/// <inheritdoc/>
		public sealed override bool HasUnsavedChanges => _files.Keys.Any(CheckHasUnsavedChanges);

		/// <summary>
		/// Event handler for when filepath assigned to data has been updated
		/// </summary>
		/// <param name="filehandler">The file handler that invoked the event</param>
		/// <param name="data">The data being updated</param>
		/// <param name="uri">The new uri</param>
		public delegate void DataUriUpdatedEventHandler(BaseMultiFileHandler<TFileData> filehandler, TFileData data, Uri? uri);

		/// <summary>
		/// Event for when the uri atacked to data gets updated
		/// </summary>
		public event DataUriUpdatedEventHandler? DataUriUpdated;

		/// <summary>
		/// Returns the file path to data
		/// </summary>
		/// <param name="data">The data to the get filepath for</param>
		/// <exception cref="InvalidOperationException"></exception>
		public Uri? GetFilepath(TFileData data)
		{
			if(!_files.TryGetValue(data, out Uri? uri))
			{
				throw new InvalidOperationException("Data not added!");
			}

			return uri;
		}

		private void VerifyData(TFileData data)
		{
			_ = GetFilepath(data);
		}

		private void UpdateFile(TFileData data, Uri? uri, bool clearHistory)
		{
			_files[data] = uri;
			GetDataChangeState(data)?.StoreCurrentState(clearHistory);
			DataUriUpdated?.Invoke(this, data, uri);
		}


		/// <summary>
		/// Obtain the data change state for the given file data
		/// </summary>
		/// <param name="data">Data to retrieve the filetracker for</param>
		protected abstract IDataChangeState? GetDataChangeState(TFileData data);

		/// <summary>
		/// Check whether a file has unsaved changes
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool CheckHasUnsavedChanges(TFileData data)
		{
			VerifyData(data);
			return GetDataChangeState(data)?.HasDataChanged == true;
		}


		/// <summary>
		/// Adds file data to the handler
		/// </summary>
		/// <param name="data"></param>
		/// <param name="uri"></param>
		public void Add(TFileData data, Uri? uri)
		{
			if(_files.ContainsKey(data))
			{
				throw new InvalidOperationException("Data already added!");
			}

			UpdateFile(data, uri, true);
		}

		/// <summary>
		/// Closes opened file data
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<bool> Close(TFileData data)
		{
			VerifyData(data);

			if(AskForCloseConfirmation && CheckHasUnsavedChanges(data))
			{
				MessageBoxResult? result = await Window.MessageBoxDialog(
					"Warning!",
					"You have unsaved changes!\nDo you want to save?",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Warning);

				switch(result)
				{
					case MessageBoxResult.Yes:
					case MessageBoxResult.Ok:
						if(!await Save(data, false))
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
			}

			_files.Remove(data);
			return true;
		}

		/// <summary>
		/// Closes all opened file data
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<bool> CloseAll()
		{
			bool result = await CloseConfirmation();

			if(result)
			{
				_files.Clear();
			}

			return result;
		}

		/// <summary>
		/// Clear a loaded file path
		/// </summary>
		public void ForgetFilePath(TFileData data)
		{
			VerifyData(data);
			_files[data] = null;
		}

		/// <inheritdoc/>
		protected override async Task<bool> OnCloseConfirm()
		{
			return await SaveAll();
		}

		/// <summary>
		/// Saves all files
		/// </summary>
		/// <returns></returns>
		public async Task<bool> SaveAll()
		{
			foreach(TFileData data in _files.Keys)
			{
				if(!await Save(data, false))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Open a save file dialog and save the file to the selected path
		/// </summary>
		/// <param name="data">The data to save</param>
		/// <param name="newPath">Whether a new path should be chosen, even if one was already stored ("save as")</param>
		/// <returns>Whether the file was succesfully saved</returns>
		public async Task<bool> Save(TFileData data, bool newPath)
		{
			Uri? filepath = GetFilepath(data);

			if(filepath == null || newPath)
			{
				IStorageFile? file = await Window.StorageProvider.SaveFilePickerAsync(new()
				{
					Title = $"Save {FileTypeName} to File",
					ShowOverwritePrompt = true,
					FileTypeChoices = FileType
				});

				return file != null && await SaveNoDialog(data, file.Path);
			}
			else
			{
				return await SaveNoDialog(data, filepath);
			}
		}

		/// <summary>
		/// Save the file without showing a dialog
		/// </summary>
		/// <param name="data">The data to save</param>
		/// <param name="filepath">The path to save the file to</param>
		/// <returns>Whether the file was successfully saved</returns>
		public async Task<bool> SaveNoDialog(TFileData data, Uri filepath)
		{
			VerifyData(data);

			try
			{
				InternalSave(data, filepath);
			}
			catch(Exception e)
			{
				await FailedToSaveFileBox(e.Message);
				return false;
			}

			UpdateFile(data, filepath, false);
			return true;
		}

		/// <summary>
		/// Open an open file dialog and open the selected file(s)
		/// </summary>
		/// <returns>The opened file data</returns>
		public async Task<TFileData[]?> Open()
		{
			if(!await CloseConfirmation())
			{
				return null;
			}

			IReadOnlyList<IStorageFile> files = await Window.StorageProvider.OpenFilePickerAsync(new()
			{
				Title = $"Load from {FileTypeName} File",
				AllowMultiple = true,
				FileTypeFilter = FileType
			});

			if(files == null || files.Count == 0)
			{
				return null;
			}

			return await OpenNoDialog(files.Select(x => x.Path));
		}

		/// <summary>
		/// Open a file without showing a dialog
		/// </summary>
		/// <param name="filepaths">The paths to the files to open</param>
		/// <returns>Whether the file was successfully opened</returns>
		public async Task<TFileData[]> OpenNoDialog(IEnumerable<Uri> filepaths)
		{
			List<TFileData> result = [];

			foreach(Uri filepath in filepaths)
			{
				// check if file is already open
				if(_files.Values.Any(x => Uri.Compare(x, filepath, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0))
				{
					continue;
				}

				try
				{
					TFileData data = InternalLoad(filepath);
					Add(data, filepath);
					result.Add(data);
				}
				catch(Exception e)
				{
					await FailedToLoadFileBox(e.Message);
				}

			}

			return [.. result];
		}

		/// <summary>
		/// Create new data
		/// </summary>
		/// <returns>Whether the data was reset</returns>
		public TFileData New()
		{
			TFileData data = InternalNew();
			Add(data, null);
			return data;
		}


		/// <summary>
		/// New file implementation
		/// </summary>
		protected abstract TFileData InternalNew();

		/// <summary>
		/// Save to file implementation
		/// </summary>
		/// <param name="data">Data to save</param>
		/// <param name="filePath">Path to save to</param>
		protected abstract void InternalSave(TFileData data, Uri filePath);

		/// <summary>
		/// Open file implementation
		/// </summary>
		/// <param name="filePath">Path to the file to open</param>
		protected abstract TFileData InternalLoad(Uri filePath);
	}
}
