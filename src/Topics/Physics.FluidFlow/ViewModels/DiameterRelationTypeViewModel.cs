using Physics.FluidFlow.Logic;
using Physics.Shared.UI.Localization;

namespace Physics.FluidFlow.ViewModels
{
	public class DiameterRelationTypeViewModel
	{
		public DiameterRelationTypeViewModel(DiameterRelationType type, InputVariant inputVariant)
		{
			Type = type;
			var localizationKey = $"DiameterRelationType_{type}";
			if (inputVariant == InputVariant.BernoulliEquationWithHeightChange)
			{
				localizationKey += "_WithHeightChange";
			}
			Label = Localizer.Instance.GetString(localizationKey);
		}

		public string Label { get; }

		public DiameterRelationType Type { get; }
	}
}
