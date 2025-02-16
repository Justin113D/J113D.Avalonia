namespace J113D.Avalonia.Utilities.IO
{
	/// <summary>
	/// A file change tracker interface
	/// </summary>
	public interface IFileChangeTracker
	{
		/// <summary>
		/// Check whether the file does not match the current state
		/// </summary>
		public bool HasFileChanged { get; }

		/// <summary>
		/// Store the current state
		/// </summary>
		public void StoreCurrentState();

		/// <summary>
		/// Reset the current state
		/// </summary>
		public void ResetCurrentState();

		/// <summary>
		/// Copy the current state to another file change tracker
		/// </summary>
		/// <param name="source">The file change tracker to copy to</param>
		public void CopyState(IFileChangeTracker source);
	}
}
