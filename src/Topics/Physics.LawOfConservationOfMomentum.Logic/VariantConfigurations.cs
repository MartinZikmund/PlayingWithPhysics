namespace Physics.LawOfConservationOfMomentum.Logic
{
	public static class VariantConfigurations
	{
		private static FieldConfiguration CreateSpeedConfiguration() => FieldConfiguration.CreateRestricted(1, 30, 1, step: 0.1f);

		private static FieldConfiguration CreateWeightConfiguration() => FieldConfiguration.CreateRestricted(0.1, 10, 1, step: 0.1f);

		public static VariantConfiguration[] All { get; } = new VariantConfiguration[]
		{
			new VariantConfiguration()
			{
				Subtype = CollisionSubtype.V2ZeroM2BiggerThanM1,
				V1 = CreateSpeedConfiguration(),
				M1 = FieldConfiguration.CreateInvisible(),
				V2 = FieldConfiguration.CreateInvisible(),
				M2 = FieldConfiguration.CreateInvisible(),
			},
			new VariantConfiguration()
			{
				Subtype = CollisionSubtype.V2Zero,
				V1 = CreateSpeedConfiguration(),
				V2 = FieldConfiguration.CreateInvisible(),
				M1 = CreateWeightConfiguration(),
				M2 = CreateWeightConfiguration(),
			},
			new VariantConfiguration()
			{
				Subtype = CollisionSubtype.SpeedsSameDirection,
				V1 = CreateSpeedConfiguration(),
				M1 = CreateWeightConfiguration(),
				V2 = CreateSpeedConfiguration(),
				M2 = CreateWeightConfiguration(),
			},
			new VariantConfiguration()
			{
				Subtype = CollisionSubtype.SpeedsOppositeDirection,
				V1 = CreateSpeedConfiguration(),
				M1 = CreateWeightConfiguration(),
				V2 = CreateSpeedConfiguration(),
				M2 = CreateWeightConfiguration(),
			},
		};
	}
}
