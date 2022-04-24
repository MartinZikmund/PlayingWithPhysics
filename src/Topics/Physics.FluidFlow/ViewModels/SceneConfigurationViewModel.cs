using System.Collections.Generic;
using System.Linq;
using Physics.FluidFlow.Logic;
using Physics.Shared.ViewModels;

namespace Physics.FluidFlow.ViewModels;

public class SceneConfigurationViewModel : ViewModelBase
{
	public SceneConfigurationViewModel(SceneConfiguration sceneConfiguration)
	{
		Fluid = new FluidDefinitionViewModel(sceneConfiguration.Fluid);
		Configuration = sceneConfiguration;
		InputConfiguration = InputConfigurations.Configurations.FirstOrDefault(c =>
			c.InputVariant == sceneConfiguration.InputVariant &&
			c.DiameterRelationType == sceneConfiguration.DiameterRelationType);
		DisplayConfiguration = DisplayConfigurations.Configurations.FirstOrDefault(c =>
			c.InputVariant == sceneConfiguration.InputVariant &&
			c.DiameterRelationType == sceneConfiguration.DiameterRelationType);

		DiameterConfiguration = GetFieldConfiguration(InputConfiguration.DiameterConfigurations);
		Diameter1Configuration = GetFieldConfiguration(InputConfiguration.Diameter1Configurations);
		Diameter2Configuration = GetFieldConfiguration(InputConfiguration.Diameter2Configurations);
		VelocityConfiguration = GetFieldConfiguration(InputConfiguration.VelocityConfigurations);
	}

	private FieldConfiguration GetFieldConfiguration(Dictionary<FluidDefinition, FieldConfiguration> source)
	{
		if (source.TryGetValue(Fluid.FluidDefinition, out var configuration))
		{
			return configuration;
		}
		return FieldConfiguration.CreateInvisible();
	}

	public FluidDefinitionViewModel Fluid { get; }

	public SceneConfiguration Configuration { get; }

	public InputConfiguration InputConfiguration { get; }

	public DisplayConfiguration DisplayConfiguration { get; }

	public FieldConfiguration DiameterConfiguration { get; private set; }

	public FieldConfiguration Diameter1Configuration { get; private set; }

	public FieldConfiguration Diameter2Configuration { get; private set; }

	public FieldConfiguration VelocityConfiguration { get; private set; }

	public string HeightChangeInCm => (Configuration.HeightChange * 100).ToString("0.#");

	public string Diameter1InCm => (Configuration.Diameter1 * 100).ToString("0.#");

	public string Diameter2InCm => (Configuration.Diameter2 * 100).ToString("0.#");
}
