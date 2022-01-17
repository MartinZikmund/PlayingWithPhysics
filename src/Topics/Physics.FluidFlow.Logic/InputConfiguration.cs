using Physics.ElectricParticle.Logic;

namespace Physics.FluidFlow.Logic
{
	public class InputConfiguration
	{
		public InputConfiguration(InputVariant inputVariant, DiameterRelationType diameterRelationType)
		{
			InputVariant = inputVariant;
			DiameterRelationType = diameterRelationType;
		}

		public InputVariant InputVariant { get; }

		public DiameterRelationType DiameterRelationType { get; }

		public FieldConfiguration VelocityConfiguration { get; set; } = FieldConfiguration.CreateInvisible();

		public FieldConfiguration DiameterConfiguration { get; set; } = FieldConfiguration.CreateInvisible();

		public FieldConfiguration Diameter1Configuration { get; set; } = FieldConfiguration.CreateInvisible();

		public FieldConfiguration Diameter2Configuration { get; set; } = FieldConfiguration.CreateInvisible();

		public FieldConfiguration LengthConfiguration { get; set; } = FieldConfiguration.CreateInvisible();

		public FieldConfiguration HeightDecreaseConfiguration { get; set; } = FieldConfiguration.CreateInvisible();
	}
}
