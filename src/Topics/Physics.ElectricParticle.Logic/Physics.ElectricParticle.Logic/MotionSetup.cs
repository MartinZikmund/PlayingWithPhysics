namespace Physics.ElectricParticle.Logic
{
	public class MotionSetup : MotionSetupBase
	{
		public MotionSetup(
			InputVariant variant,
			PlaneSetup horizontalPlane,
			PlaneSetup verticalPlane,
			ParticleSetup particle,
			EnvironmentSetting environmentSetting,
			string color)
		{
			Variant = variant;
			HorizontalPlane = horizontalPlane;
			VerticalPlane = verticalPlane;
			Particle = particle;
			Environment = environmentSetting;
			Color = color;
		}

		public InputVariant Variant { get; set; }

		public PlaneSetup HorizontalPlane { get; set; }

		public PlaneSetup VerticalPlane { get; set; }

		public ParticleSetup Particle { get; set; }

		public EnvironmentSetting Environment { get; set; }
	}
}
