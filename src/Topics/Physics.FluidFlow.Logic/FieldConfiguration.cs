namespace Physics.FluidFlow.Logic
{
	public class FieldConfiguration
	{
		private FieldConfiguration(bool isVisible) => IsVisible = isVisible;

		private FieldConfiguration(double minimum, double maximum, float? defaultValue, int descriptionType, float? step = null)
		{
			Minimum = minimum;
			Maximum = maximum;
			DefaultValue = defaultValue;
			DescriptionType = descriptionType;
			Step = step;
		}

		public static FieldConfiguration CreateInvisible() =>
			new FieldConfiguration(false);

		public static FieldConfiguration CreateRestricted(double minimum, double maximum, float? defaultValue = null, int descriptionType = 0, float? step = null) =>
			new FieldConfiguration(minimum, maximum, defaultValue, descriptionType, step);

		public static FieldConfiguration CreateUnrestricted(float? defaultValue = null) =>
			new FieldConfiguration(double.MinValue, double.MaxValue, defaultValue, 0);

		public bool IsVisible { get; set; } = true;

		public double Minimum { get; set; }

		public double Maximum { get; set; }

		public float? DefaultValue { get; set; }

		public float? Step { get; set; }

		public int DescriptionType { get; set; }
	}
}
