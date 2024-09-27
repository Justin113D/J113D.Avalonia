namespace J113D.Avalonia.Utilities.Enum
{
    public record EnumDescription
    {
        public object Value { get; }

        public string Description { get; }

        public string? Help { get; }

        public EnumDescription(object value, string description, string? help)
        {
            Value = value;
            Description = description;
            Help = help;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
