namespace Physics.FluidFlow.Logic
{
	public class SceneConfiguration
	{
		public SceneConfiguration(FluidDefinition fluid, float speed, float diameter1, float diameter2, float length, float heightChange)
		{
			Fluid = fluid;
			Speed = speed;
			Diameter1 = diameter1;
			Diameter2 = diameter2;
			HeightChange = heightChange;
		}

		public FluidDefinition Fluid { get; set; }

		public float Speed { get; set; }

		public float Diameter1 { get; set; }

		public float Diameter2 { get; set; }

		public float Length { get; set; }

		public float HeightChange { get; set; }
	}
}
