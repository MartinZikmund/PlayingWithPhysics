namespace Physics.ElectricParticle.Logic
{
	public class VariantConfiguration
    {
		public InputVariant InputVariant { get; set; }

		public ParticleType ParticleType { get; set; }

		public FieldConfiguration Q { get; set; }

		public FieldConfiguration M { get; set; }

		public FieldConfiguration U { get; set; } = FieldConfiguration.CreateRestricted(100, 100000);

		public FieldConfiguration V0 { get; set; } = FieldConfiguration.CreateRestricted(0, 1000);

		public FieldConfiguration ParticlePolarity { get; set; } = FieldConfiguration.CreateUnrestricted();
    }
}
