using System;
using System.IO;

namespace J113D.Avalonia.Utilities.IO
{
    /// <summary>
    /// File handler for writing and reading a single file
    /// </summary>
    public abstract class TextFileHandler : BaseFileHandler
    {
        protected override void InternalSave(Uri filePath)
        {
            File.WriteAllText(filePath.AbsolutePath, WriteText(filePath));
        }

        protected override void InternalLoad(Uri filePath)
        {
            ReadText(filePath, File.ReadAllText(filePath.AbsolutePath));
        }

        protected abstract string WriteText(Uri filePath);

        protected abstract void ReadText(Uri filePath, string text);
    }
}
