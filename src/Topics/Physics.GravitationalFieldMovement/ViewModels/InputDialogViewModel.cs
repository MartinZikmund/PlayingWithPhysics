using System.Linq;
using Physics.GravitationalFieldMovement.Logic;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.ViewModels;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI.Xaml.Controls;

namespace Physics.GravitationalFieldMovement.ViewModels;

public class InputDialogViewModel : ViewModelBase
{
	public InputDialogViewModel(DifficultyOption difficulty, InputConfiguration inputConfiguration)
	{
		IsAdvanced = difficulty == DifficultyOption.Advanced;

		if (inputConfiguration != null)
		{
			RzBigNumber = inputConfiguration.RzBigNumber;
			MzBigNumber = inputConfiguration.MzBigNumber;
			HBigNumber = inputConfiguration.HBigNumber;
			V0BigNumber = inputConfiguration.V0BigNumber;
			BetaDeg = inputConfiguration.BetaDeg;
			Phi0Deg = inputConfiguration.Phi0Deg;
		}
	}

	public bool IsAdvanced { get; }

	public BigNumber RzBigNumber { get; set; } = new BigNumber(6.38, 6);

	public BigNumber MzBigNumber { get; set; } = new BigNumber(5.97, 24);

	public BigNumber HBigNumber { get; set; } = new BigNumber(9.0, 5);

	public BigNumber V0BigNumber { get; set; } = new BigNumber(7.0, 3);

	public double BetaDeg { get; set; } = 0;

	public double Phi0Deg { get; set; } = 90;

	public bool HasErrors => !string.IsNullOrEmpty(ErrorMessage);

	public string ErrorMessage { get; private set; }

	public InputConfiguration Result { get; private set; }

	public PlanetPresetViewModel[] Presets { get; } = PlanetPresets.Presets.Select(x => new PlanetPresetViewModel(x)).ToArray();

	public PlanetPresetViewModel SelectedPreset { get; set; }

	internal void OnSelectedPresetChanged()
	{
		if (SelectedPreset == null)
		{
			return;
		}

		RzBigNumber = SelectedPreset.Preset.R;
		MzBigNumber = SelectedPreset.Preset.M;
	}

	public void SaveHandler(object sender, ContentDialogButtonClickEventArgs args)
	{
		if (!Save())
		{
			args.Cancel = true;
		}
	}

	public bool Save()
	{

		bool allValid = true;

		if (allValid)
		{
			Result = new InputConfiguration(
				RzBigNumber,
				MzBigNumber,
				HBigNumber,
				V0BigNumber,
				BetaDeg,
				Phi0Deg);
			return true;
		}
		else
		{
			ErrorMessage = Localizer.Instance.GetString("CannotParseInputNumbers");
			return false;
		}
	}
}
