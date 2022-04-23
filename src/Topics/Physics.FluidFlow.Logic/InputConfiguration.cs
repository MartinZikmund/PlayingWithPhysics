using System.Collections.Generic;
using System.Linq;

namespace Physics.FluidFlow.Logic;

public class InputConfiguration
{
	public InputConfiguration(InputVariant inputVariant, DiameterRelationType diameterRelationType)
	{
		InputVariant = inputVariant;
		DiameterRelationType = diameterRelationType;
	}

	public InputVariant InputVariant { get; }

	public DiameterRelationType DiameterRelationType { get; }

	public FluidDefinition[] FluidDefinitions { get; set; }

	public bool ShowDiameterTypePicker => InputConfigurations.Configurations.Where(c => c.InputVariant == InputVariant).Count() > 1;

	public Dictionary<FluidDefinition, FieldConfiguration> VelocityConfigurations { get; set; } = new();

	public Dictionary<FluidDefinition, FieldConfiguration> DiameterConfigurations { get; set; } = new();

	public Dictionary<FluidDefinition, FieldConfiguration> Diameter1Configurations { get; set; } = new();

	public Dictionary<FluidDefinition, FieldConfiguration> Diameter2Configurations { get; set; } = new();

	public FieldConfiguration LengthConfiguration { get; set; } = FieldConfiguration.CreateInvisible();

	public FieldConfiguration HeightChangeConfiguration { get; set; } = FieldConfiguration.CreateInvisible();

	public FieldConfiguration PressureConfiguration { get; set; } = FieldConfiguration.CreateInvisible();
}
