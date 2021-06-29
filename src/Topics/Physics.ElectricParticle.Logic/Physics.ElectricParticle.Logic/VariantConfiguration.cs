namespace Physics.ElectricParticle.Logic
{
	public class VariantConfiguration
    {
		public InputVariant InputVariant { get; set; }

		public ParticleType ParticleType { get; set; }

		public FieldConfiguration Q { get; set; }

		public FieldConfiguration M { get; set; } = FieldConfiguration.CreateRestricted(1, 10000);

		public FieldConfiguration UPrimary { get; set; } = FieldConfiguration.CreateRestricted(100, 100000);

		public FieldConfiguration USecondary { get; set; } = FieldConfiguration.CreateInvisible();

		public FieldConfiguration Angle { get; set; } = FieldConfiguration.CreateRestricted(0, 359, 0);

		public FieldConfiguration V0 { get; set; } = FieldConfiguration.CreateRestricted(0, 1000);

		public FieldConfiguration ParticlePolarity { get; set; } = FieldConfiguration.CreateUnrestricted();
    }
}
