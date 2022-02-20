namespace Physics.FluidFlow.Logic
{
	public class SceneConfiguration
	{
		public SceneConfiguration(
			InputVariant inputVariant,
			DiameterRelationType diameterRelationType,
			FluidDefinition fluid,
			float velocity,
			float diameter1,
			float diameter2,
			float length,
			float heightDecrease)
		{
			InputVariant = inputVariant;
			DiameterRelationType = diameterRelationType;
			Fluid = fluid;
			Velocity = velocity;
			Diameter1 = diameter1;
			Diameter2 = diameter2;
			Length = length;
			HeightDecrease = heightDecrease;
		}

		public InputVariant InputVariant { get; set; }

		public DiameterRelationType DiameterRelationType { get; set; }

		public FluidDefinition Fluid { get; set; }

		public float Velocity { get; set; }

		public float Diameter1 { get; set; }

		public float Diameter2 { get; set; }

		public float Length { get; set; }

		public float HeightDecrease { get; set; }
	}
}
