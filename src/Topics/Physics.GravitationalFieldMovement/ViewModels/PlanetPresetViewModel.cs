using Physics.GravitationalFieldMovement.Logic;
using Physics.Shared.UI.Localization;

namespace Physics.GravitationalFieldMovement.ViewModels;

public class PlanetPresetViewModel
{
	public PlanetPresetViewModel(PlanetPreset preset)
	{
		Preset = preset;
	}

	public PlanetPreset Preset { get; }

	public string Name => Localizer.Instance.GetString(Preset.NameKey);
}
