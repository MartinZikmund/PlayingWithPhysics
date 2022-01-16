using Physics.FluidFlow.Logic;
using Physics.Shared.UI.Localization;

namespace Physics.FluidFlow.ViewModels
{
	public class FluidDefinitionViewModel
	{
		public FluidDefinitionViewModel(FluidDefinition fluidDefinition)
		{
			FluidDefinition = fluidDefinition;
		}

		public FluidDefinition FluidDefinition { get; }

		public string LocalizedName => Localizer.Instance.GetString($"Fluid_{FluidDefinition.Name}");

		public float Density => FluidDefinition.Density;
	}
}
