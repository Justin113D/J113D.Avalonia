using Avalonia.Controls;
using Avalonia.Platform.Storage;
using J113D.Avalonia.MessageBox;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace J113D.Avalonia.Utilities.IO
{
    public abstract class BaseFileHandler
    {
        public abstract IReadOnlyList<FilePickerFileType>? FileType { get; }

        protected abstract string FileTypeName { get; }

        protected abstract Window Window { get; }

        protected abstract IFileChangeTracker? FileChangeTracker { get; }

        public Uri? LoadedFilePath { get; private set; }

        private async Task FailedToLoadFileBox(string message)
        {
            _ = await Window.MessageBoxDialog(
                "Failed to load file!",
                $"Failed to load {FileTypeName}:\n{message}",
                MessageBoxButtons.Ok,
                MessageBoxIcon.Error);
        }

        public async Task<bool> ResetConfirmation()
        {
            if (FileChangeTracker?.HasFileChanged != false)
            {
                MessageBoxResult? result = await Window.MessageBoxDialog(
                    "Warning!",
                    "Unsaved changes will be lost!\nDo you want to save before?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                
                switch(result)
                {
                    case MessageBoxResult.Yes:
                        if(!await Save(false))
                        {
                            return false;
                        }

                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return false;
                }

                return true;
            }

            return true;
        }

        public async Task<bool> Save(bool newPath)
        {
            if (LoadedFilePath == null || newPath)
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
            FileChangeTracker?.StoreCurrentState();
            return true;
        }

        public async Task<bool> Open()
        {
            if (!await ResetConfirmation())
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
            FileChangeTracker?.StoreCurrentState();
            return true;
        }

        public async Task<bool> Reset()
        {
            if (!await ResetConfirmation())
            {
                return false;
            }

            InternalReset();
            LoadedFilePath = null;
            FileChangeTracker?.StoreCurrentState();
            return true;
        }

        public void Clear()
        {
            LoadedFilePath = null;
            FileChangeTracker?.ResetCurrentState();
        }


        protected abstract void InternalReset();

        protected abstract void InternalSave(Uri filePath);

        protected abstract void InternalLoad(Uri filePath);

    }
}
