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
				Q = FieldConfiguration.CreateRestricted(1, 5, step: 1),
				M = FieldConfiguration.CreateRestricted(1, 10000, (int)WeightDescriptionType.TenToMinus17Power),
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVerticalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, (int)WeightDescriptionType.Nucleus),
				U = FieldConfiguration.CreateRestricted(5, 100, step: 5),
				ParticlePolarity = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVerticalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible(),
				U = FieldConfiguration.CreateRestricted(5, 100, step: 5),
				V0 = FieldConfiguration.CreateRestricted(10000, 1000000, step: 10000),
				ParticlePolarity = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(1, 5, step: 1),
				M = FieldConfiguration.CreateRestricted(1,10000, (int)WeightDescriptionType.TenToMinus17Power)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, (int)WeightDescriptionType.Nucleus),
				U = FieldConfiguration.CreateRestricted(5, 100, step: 5),
				ParticlePolarity = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible(),
				U = FieldConfiguration.CreateRestricted(5, 100, step: 5),
				V0 = FieldConfiguration.CreateRestricted(10000, 1000000, step: 10000),
				ParticlePolarity = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalWithGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(1, 5, step: 1),
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
				M = FieldConfiguration.CreateRestricted(1, 300, (int)WeightDescriptionType.Nucleus),
				ParticlePolarity = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalHorizontalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible(),
				ParticlePolarity = FieldConfiguration.CreateInvisible()
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
