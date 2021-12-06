using System;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Localization;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class ScenePresetViewModel
	{
		public ScenePresetViewModel(ScenePreset preset)
		{
			Preset = preset;
		}

		public ScenePreset Preset { get; }

		public string Name => Localizer.Instance.GetString("ScenePreset_" + Preset.Name);

		public Uri Image => new Uri($"ms-appx:///Assets/Presets/{Preset.Name}.png");
	}
}
