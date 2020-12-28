namespace Physics.ElectricParticle.Logic
{
	public class FieldConfiguration
	{
		private FieldConfiguration(bool isVisible) => IsVisible = isVisible;

		private FieldConfiguration(float minimum, float maximum, float step, int descriptionType)
		{
			Minimum = minimum;
			Maximum = maximum;
			Step = step;
			DescriptionType = descriptionType;
		}

		public static FieldConfiguration CreateInvisible() =>
			new FieldConfiguration(false);

		public static FieldConfiguration CreateRestricted(float minimum, float maximum, float step, int descriptionType = 0) =>
			new FieldConfiguration(minimum, maximum, step, descriptionType);

		public bool IsVisible { get; set; } = true;

		public float? Minimum { get; set; }

		public float? Maximum { get; set; }

		public float? Step { get; set; }

		public int DescriptionType { get; set; }
	}
}
