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
			float heightChange,
			float pressure)
		{
			InputVariant = inputVariant;
			DiameterRelationType = diameterRelationType;
			Fluid = fluid;
			Velocity = velocity;
			Diameter1 = diameter1;
			Diameter2 = diameter2;
			Length = length;
			HeightChange = heightChange;
			Pressure = pressure;
		}

		public InputVariant InputVariant { get; set; }

		public DiameterRelationType DiameterRelationType { get; set; }

		public FluidDefinition Fluid { get; set; }

		public float Velocity { get; set; }

		public string VelocityString => Velocity.ToString("0.###");

		public float Diameter1 { get; set; }

		public string Diameter1String => Diameter1.ToString("0.#");

		public float Diameter2 { get; set; }

		public string Diameter2String => Diameter2.ToString("0.#");

		public float Length { get; set; }

		public string LengthString => Length.ToString("0.#");

		public float HeightChange { get; set; }

		public string HeightChangeString => HeightChange.ToString("0.#");

		public float Pressure { get; set; }

		public string PressureString => Pressure.ToString("0.###");
	}
}
