namespace Physics.ElectricParticle.Logic
{
	public static class VariantConfigurations
	{
		public static VariantConfiguration[] All { get; } = new VariantConfiguration[]
		{
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVerticalNoGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5, 5),
				M = FieldConfiguration.CreateRestricted(1, 10000, (int)WeightDescriptionType.TenToMinus17Power),
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVerticalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, (int)WeightDescriptionType.Nukleus)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVerticalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5,5),
				M = FieldConfiguration.CreateRestricted(1,10000, (int)WeightDescriptionType.TenToMinus17Power)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, (int)WeightDescriptionType.Nukleus)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalWithGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5,5),
				M = FieldConfiguration.CreateRestricted(1, 10000, (int)WeightDescriptionType.TenToMinus17Power)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalHorizontalNoGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5, 5),
				M = FieldConfiguration.CreateRestricted(1, 10000, (int)WeightDescriptionType.TenToMinus17Power),
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalHorizontalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, (int)WeightDescriptionType.Nukleus)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalHorizontalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalWithGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5, 5),
				M = FieldConfiguration.CreateRestricted(1, 10000, (int)WeightDescriptionType.TenToMinus17Power)
			}
		};
	}
}
