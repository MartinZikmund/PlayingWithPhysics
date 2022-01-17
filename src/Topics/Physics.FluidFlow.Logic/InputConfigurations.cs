using Physics.ElectricParticle.Logic;

namespace Physics.FluidFlow.Logic
{
	public static class InputConfigurations
	{
		public static InputConfiguration[] Configurations { get; } = new[]
		{
			new InputConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.Equal)
			{
				DiameterConfiguration = FieldConfiguration.CreateRestricted(0.1, 200, 10, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 50, 10, step: 0.1f),
			},
			new InputConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.S1Larger)
			{
				Diameter1Configuration = FieldConfiguration.CreateRestricted(0.1, 200, 20, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 10, 5, step: 0.1f),
				Diameter2Configuration = FieldConfiguration.CreateRestricted(0.1, 100, 10, step: 0.1f),
			},
			new InputConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.S2Larger)
			{
				Diameter1Configuration = FieldConfiguration.CreateRestricted(0.1, 100, 20, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 50, 10, step: 0.1f),
				Diameter2Configuration = FieldConfiguration.CreateRestricted(0.1, 200, 10, step: 0.1f),
			},
			new InputConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.Equal)
			{
				DiameterConfiguration = FieldConfiguration.CreateRestricted(0.1, 200, 10, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 50, 10, step: 0.1f),
			},
			new InputConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.S1Larger)
			{
				Diameter1Configuration = FieldConfiguration.CreateRestricted(0.1, 200, 20, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 10, 5, step: 0.1f),
				Diameter2Configuration = FieldConfiguration.CreateRestricted(0.1, 100, 10, step: 0.1f),
			},
			new InputConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.S2Larger)
			{
				Diameter1Configuration = FieldConfiguration.CreateRestricted(0.1, 100, 20, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 50, 10, step: 0.1f),
				Diameter2Configuration = FieldConfiguration.CreateRestricted(0.1, 200, 10, step: 0.1f),
			},			
			new InputConfiguration(InputVariant.BernoulliEquationWithHeightDecrease, DiameterRelationType.S1Larger)
			{
				Diameter1Configuration = FieldConfiguration.CreateRestricted(0.1, 200, 20, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 10, 5, step: 0.1f),
				Diameter2Configuration = FieldConfiguration.CreateRestricted(0.1, 100, 10, step: 0.1f),
				HeightDecreaseConfiguration = FieldConfiguration.CreateRestricted(1, 100, 10, step: 1),
			},
			new InputConfiguration(InputVariant.BernoulliEquationWithHeightDecrease, DiameterRelationType.S2Larger)
			{
				Diameter1Configuration = FieldConfiguration.CreateRestricted(0.1, 100, 20, step: 0.1f),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 50, 10, step: 0.1f),
				Diameter2Configuration = FieldConfiguration.CreateRestricted(0.1, 200, 10, step: 0.1f),
				HeightDecreaseConfiguration = FieldConfiguration.CreateRestricted(1, 100, 10, step: 1),
			},
			new InputConfiguration(InputVariant.RealFluidMovement, DiameterRelationType.Equal)
			{
				DiameterConfiguration = FieldConfiguration.CreateRestricted(0.1, 1, 0.5f, step: 0.1f),
				LengthConfiguration = FieldConfiguration.CreateRestricted(1, 10, 5, step: 1),
				VelocityConfiguration = FieldConfiguration.CreateRestricted(0.1, 0.5, 0.2f, step: 0.1f)
			},
		};
	}
}
