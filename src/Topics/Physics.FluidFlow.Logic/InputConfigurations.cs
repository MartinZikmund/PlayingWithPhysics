using System;
using Physics.FluidFlow.Logic;

namespace Physics.FluidFlow.Logic
{
	public static class InputConfigurations
	{
		public static InputConfiguration[] Configurations { get; } = new[]
		{
			new InputConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.Equal)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
					FluidDefinitions.Oil
				},
				VelocityConfigurations = new System.Collections.Generic.Dictionary<FluidDefinition, FieldConfiguration>()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateInvisible()},
					{ FluidDefinitions.Oil, FieldConfiguration.CreateInvisible()},
				},
				DiameterConfigurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(1, 10, 5, step: 0.5f) },
					{ FluidDefinitions.Oil, FieldConfiguration.CreateRestricted(40, 105, 60, step: 5f) },
				}
			},
			new InputConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.S1Larger)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
					//FluidDefinitions.Oil
				},
				Diameter1Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(5, 10, 8, step: 0.5f) },
					{ FluidDefinitions.Oil, FieldConfiguration.CreateRestricted(71, 105, 80, step: 5f) },
				},
				Diameter2Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(1, 4.5, 3, step: 0.5f) },
					{ FluidDefinitions.Oil, FieldConfiguration.CreateRestricted(40, 71, 60, step: 5f) },
				},
				VelocityConfigurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(0.1, 1, 0.5f, step: 0.1f) },
					{ FluidDefinitions.Oil, FieldConfiguration.CreateRestricted(0.5, 1, 0.5f, step: 0.1f) },
				},
			},
			new InputConfiguration(InputVariant.ContinuityEquation, DiameterRelationType.S2Larger)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
				},
				Diameter1Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(1, 4.5, 2, step: 0.5f) },
				},
				Diameter2Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(5, 10, 5, step: 0.5f) },
				},
				VelocityConfigurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(20, 100, 50f, step: 1f) },
				},
			},
			//new InputConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.Equal)
			//{
			//	FluidDefinitions = new[]
			//	{
			//		FluidDefinitions.Water,
			//	},
			//	Diameter1Configurations = new ()
			//	{
			//		{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(1, 4.5, 2, step: 0.5f) },
			//	},
			//	Diameter2Configurations = new ()
			//	{
			//		{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(5, 10, 5, step: 0.5f) },
			//	},
			//	VelocityConfigurations = new ()
			//	{
			//		{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(20, 100, 50f, step: 1f) },
			//	},
			//},
			new InputConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.S1Larger)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
				},
				Diameter1Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(3, 10, 5, step: 0.5f) },
				},
				Diameter2Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(3, 10, 4, step: 0.5f) },
				},
				VelocityConfigurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(0.1, 1, 0.5f, step: 0.1f) },
				},
				PressureConfiguration = FieldConfiguration.CreateRestricted(250000, 500000, 250000f, step: 50000f),
			},
			new InputConfiguration(InputVariant.BernoulliEquationWithoutHeightDecrease, DiameterRelationType.S2Larger)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
				},
				Diameter1Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(1, 4.5, 2, step: 0.5f) },
				},
				Diameter2Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(5, 10, 5, step: 0.5f) },
				},
				VelocityConfigurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(20, 100, 50f, step: 1f) },
				},
				PressureConfiguration = FieldConfiguration.CreateRestricted(250000, 500000, 250000f, step: 50000f),
			},
			new InputConfiguration(InputVariant.BernoulliEquationWithHeightDecrease, DiameterRelationType.S1Larger)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
				},
				Diameter1Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(1, 4.5, 2, step: 0.5f) },
				},
				Diameter2Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(5, 10, 5, step: 0.5f) },
				},
				VelocityConfigurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(20, 100, 50f, step: 1f) },
				},
				HeightDecreaseConfiguration = FieldConfiguration.CreateRestricted(1, 100, 10, step: 1),
				PressureConfiguration = FieldConfiguration.CreateRestricted(250000, 500000, 250000f, step: 50000f),
			},
			new InputConfiguration(InputVariant.BernoulliEquationWithHeightDecrease, DiameterRelationType.S2Larger)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
				},
				Diameter1Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(1, 4.5, 2, step: 0.5f) },
				},
				Diameter2Configurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(5, 10, 5, step: 0.5f) },
				},
				VelocityConfigurations = new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(20, 100, 50f, step: 1f) },
				},
				HeightDecreaseConfiguration = FieldConfiguration.CreateRestricted(1, 100, 10, step: 1),
				PressureConfiguration = FieldConfiguration.CreateRestricted(250000, 500000, 250000f, step: 50000f),
			},
			new InputConfiguration(InputVariant.RealFluidMovement, DiameterRelationType.Equal)
			{
				FluidDefinitions = new[]
				{
					FluidDefinitions.Water,
				},
				DiameterConfigurations= new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(0.1, 1, 0.5f, step: 0.1f) },
				},
				LengthConfiguration = FieldConfiguration.CreateRestricted(1, 10, 5, step: 1),
				VelocityConfigurations= new ()
				{
					{ FluidDefinitions.Water, FieldConfiguration.CreateRestricted(0.1, 0.5, 0.2f, step: 0.1f) },
				}
			},
		};
	}
}
