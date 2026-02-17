namespace J113D.Avalonia.Utilities.IO
{
	/// <summary>
	/// A change tracker interface
	/// </summary>
	public interface IDataChangeState
	{
		/// <summary>
		/// Check whether the data does not match the current state
		/// </summary>
		public bool HasDataChanged { get; }

		/// <summary>
		/// Store the current change state
		/// </summary>
		public void StoreCurrentState(bool clearHistory);
	}
}
