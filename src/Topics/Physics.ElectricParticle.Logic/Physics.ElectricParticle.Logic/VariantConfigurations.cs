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
				M = FieldConfiguration.CreateRestricted(1, 10000, 1, (int)WeightDescriptionType.TenToMinus17Power),
				UPrimary = FieldConfiguration.CreateRestricted(100, 100000, 8000),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 5),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 90)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVerticalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, 50, (int)WeightDescriptionType.Nucleus),
				UPrimary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				ParticlePolarity = FieldConfiguration.CreateInvisible(),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 1000),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 90)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVerticalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible(),
				UPrimary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				ParticlePolarity = FieldConfiguration.CreateInvisible(),
				V0 = FieldConfiguration.CreateRestricted(10000, 1000000, 500000, step: 10000),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 90)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(1, 5, 1, step: 1),
				M = FieldConfiguration.CreateRestricted(1, 10000, 1, (int)WeightDescriptionType.TenToMinus17Power),
				UPrimary = FieldConfiguration.CreateRestricted(0, 1000000, 8000),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 5),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 0)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, 50, (int)WeightDescriptionType.Nucleus),
				UPrimary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				ParticlePolarity = FieldConfiguration.CreateInvisible(),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 1000),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 0)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible(),
				UPrimary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				V0 = FieldConfiguration.CreateRestricted(10000, 1000000, 500000, step: 10000),
				ParticlePolarity = FieldConfiguration.CreateInvisible(),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 0)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyHorizontalWithGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(1, 5, step: 1),
				M = FieldConfiguration.CreateRestricted(1, 10000, 1, (int)WeightDescriptionType.TenToMinus17Power),
				UPrimary = FieldConfiguration.CreateRestricted(0, 1000000, 8000),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 10),
				Angle = FieldConfiguration.CreateInvisible()
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalHorizontalNoGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5, 5),
				M = FieldConfiguration.CreateRestricted(1, 10000, 1, (int)WeightDescriptionType.TenToMinus17Power),
				UPrimary = FieldConfiguration.CreateRestricted(100, 100000, 100),
				USecondary = FieldConfiguration.CreateRestricted(100, 100000, 8000),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 5),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 0)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalHorizontalNoGravity,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.CreateRestricted(1, 150),
				M = FieldConfiguration.CreateRestricted(1, 300, 50, (int)WeightDescriptionType.Nucleus),
				UPrimary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				USecondary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				ParticlePolarity = FieldConfiguration.CreateInvisible(),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 1000),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 0)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalHorizontalNoGravity,
				ParticleType = ParticleType.Electron,
				Q = FieldConfiguration.CreateInvisible(),
				M = FieldConfiguration.CreateInvisible(),
				UPrimary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				USecondary = FieldConfiguration.CreateRestricted(5, 100, 5, step: 5),
				ParticlePolarity = FieldConfiguration.CreateInvisible(),
				V0 = FieldConfiguration.CreateRestricted(10000, 1000000, 500000, step: 10000),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 0)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.AdvancedVerticalWithGravity,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5, 5),
				M = FieldConfiguration.CreateRestricted(1, 10000, 1, (int)WeightDescriptionType.TenToMinus17Power),
				UPrimary = FieldConfiguration.CreateRestricted(0, 1000000, 8000),
				V0 = FieldConfiguration.CreateRestricted(0, 1000, 5),
				Angle = FieldConfiguration.CreateRestricted(0, 359, 90)
			}
		};
	}
}
