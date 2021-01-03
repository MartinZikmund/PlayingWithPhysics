namespace Physics.ElectricParticle.Logic
{
	public class FieldConfiguration
	{
		private FieldConfiguration(bool isVisible) => IsVisible = isVisible;

		private FieldConfiguration(double minimum, double maximum, int descriptionType)
		{
			Minimum = minimum;
			Maximum = maximum;
			DescriptionType = descriptionType;
		}

		public static FieldConfiguration CreateInvisible() =>
			new FieldConfiguration(false);

		public static FieldConfiguration CreateRestricted(double minimum, double maximum, int descriptionType = 0) =>
			new FieldConfiguration(minimum, maximum, descriptionType);

		public static FieldConfiguration CreateUnrestricted() =>
			new FieldConfiguration(double.MinValue, double.MaxValue, 0);

		public bool IsVisible { get; set; } = true;

		public double Minimum { get; set; }

		public double Maximum { get; set; }

		public int DescriptionType { get; set; }
	}
}
