using System.Linq;
using Physics.FluidFlow.Logic;
using Physics.Shared.ViewModels;

namespace Physics.FluidFlow.ViewModels
{
	public class SceneConfigurationViewModel : ViewModelBase
	{
		public SceneConfigurationViewModel(SceneConfiguration sceneConfiguration)
		{
			Fluid = new FluidDefinitionViewModel(sceneConfiguration.Fluid);
			Configuration = sceneConfiguration;
			InputConfiguration = InputConfigurations.Configurations.FirstOrDefault(c =>
				c.InputVariant == sceneConfiguration.InputVariant &&
				c.DiameterRelationType == sceneConfiguration.DiameterRelationType);
		}

		public FluidDefinitionViewModel Fluid { get; }

		public SceneConfiguration Configuration { get; }

		public InputConfiguration InputConfiguration { get; }

		public string HeightDecreaseInCm => (Configuration.HeightDecrease * 100).ToString("0");

		public string Diameter1InCm => (Configuration.Diameter1 * 100).ToString("0");

		public string Diameter2InCm => (Configuration.Diameter2 * 100).ToString("0");
	}
}
