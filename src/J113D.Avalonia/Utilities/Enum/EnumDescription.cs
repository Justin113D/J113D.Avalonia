namespace J113D.Avalonia.Utilities.Enum
{
	/// <summary>
	/// Enum value information
	/// </summary>
	public record EnumDescription
	{
		/// <summary>
		/// The enum value
		/// </summary>
		public object Value { get; }

		/// <summary>
		/// The description of the enum value
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Create a new enum description
		/// </summary>
		/// <param name="value">The enum value</param>
		/// <param name="description">The enum description</param>
		public EnumDescription(object value, string description)
		{
			Value = value;
			Description = description;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return Description;
		}
	}
}
