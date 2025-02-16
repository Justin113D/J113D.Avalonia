using System;
using System.IO;

namespace J113D.Avalonia.Utilities.IO
{
	/// <summary>
	/// File handler for writing and reading a single generic text file
	/// </summary>
	public abstract class TextFileHandler : BaseFileHandler
	{
		/// <inheritdoc/>
		protected override void InternalSave(Uri filePath)
		{
			File.WriteAllText(filePath.AbsolutePath, WriteText(filePath));
		}

		/// <inheritdoc/>
		protected override void InternalLoad(Uri filePath)
		{
			ReadText(filePath, File.ReadAllText(filePath.AbsolutePath));
		}

		/// <summary>
		/// Retrieve the text to write
		/// </summary>
		/// <param name="filePath">The file path that the text will be written to</param>
		/// <returns>The text to write</returns>
		protected abstract string WriteText(Uri filePath);

		/// <summary>
		/// Process text that was read from a file
		/// </summary>
		/// <param name="filePath">The file that was read</param>
		/// <param name="text">The text to process</param>
		protected abstract void ReadText(Uri filePath, string text);
	}
}
