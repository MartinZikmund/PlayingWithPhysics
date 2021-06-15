namespace Physics.Shared.UI.Models.Input
{
	public class GravityDefault
	{
		public GravityDefault(string name, float? value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; }

		public float? Value { get; }

		public bool HasValue => Value != null;

		public string FormattedValue => Value?.ToString("0.##") ?? string.Empty;
	}
}
