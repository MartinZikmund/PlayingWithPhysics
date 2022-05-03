using System.Linq;
using Physics.GravitationalFieldMovement.Logic;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.ViewModels;
using Windows.Foundation.Collections;
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
			ValidatePlanetPreset();
		}
	}

	private bool _preventPresetChanges = false;

	public bool IsAdvanced { get; }

	public BigNumber _rzBigNumber = new BigNumber(6.38, 6);
	public BigNumber RzBigNumber
	{
		get => _rzBigNumber;
		set
		{
			//1x10^2 < R_Z < 7x10^10
			if (value < _rzBigNumberMinimum)
			{
				value = _rzBigNumberMinimum;
			}

			if (value > _rzBigNumberMaximum)
			{
				value = _rzBigNumberMaximum;
			}


			//M_Z / R_Z < 3x10 ^ 24
			if ((MzBigNumber / value) >= _mzRzBigNumberDivision)
			{
				value = _rzBigNumber;
			}

			_rzBigNumber = value;
			ValidatePlanetPreset();
		}
	}

	private BigNumber _rzBigNumberMinimum = new BigNumber(1.0, 2);
	private BigNumber _rzBigNumberMaximum = new BigNumber(7.0, 10);
	private BigNumber _mzRzBigNumberDivision = new BigNumber(3, 24);

	private BigNumber _mzBigNumber = new BigNumber(5.97, 24);
	public BigNumber MzBigNumber
	{
		get => _mzBigNumber;
		set
		{
			//4x10^9 < M_Z < 2x10^31
			if (value < _mzBigNumberMinimum)
			{
				value = _mzBigNumberMinimum;
			}

			if (value > _mzBigNumberMaximum)
			{
				value = _mzBigNumberMaximum;
			}

			//M_Z / R_Z < 3x10 ^ 24
			if ((value / RzBigNumber) >= _mzRzBigNumberDivision)
			{
				value = _mzBigNumber;
			}

			_mzBigNumber = value;
			ValidatePlanetPreset();
		}
	}

	private BigNumber _mzBigNumberMinimum = new BigNumber(4.0, 9);
	private BigNumber _mzBigNumberMaximum = new BigNumber(2.0, 31);

	public BigNumber HBigNumber { get; set; } = new BigNumber(9.0, 5);

	private BigNumber _v0BigNumber = new BigNumber(7.0, 3);
	public BigNumber V0BigNumber
	{
		get => _v0BigNumber;
		set
		{
			//v_0 < 1x10^8
			if (value > _v0BigNumberBoxMaximum)
			{
				value = _v0BigNumberBoxMaximum;
			}

			_v0BigNumber = value;
		}
	}

	private BigNumber _v0BigNumberBoxMaximum = new BigNumber(1.0, 8);

	public double BetaDeg { get; set; } = 0;

	public double Phi0Deg { get; set; } = 90;

	public bool HasErrors => !string.IsNullOrEmpty(ErrorMessage);

	public string ErrorMessage { get; private set; }

	public InputConfiguration Result { get; private set; }

	public void ValidatePlanetPreset()
	{
		if (_preventPresetChanges)
		{
			return;
		}

		//Check if new Rz and Mz are valid given the selected planet
		var preset = Presets.FirstOrDefault(p => p.Preset.R == RzBigNumber && p.Preset.M == MzBigNumber);
		SelectedPreset = preset;
	}

	public PlanetPresetViewModel[] Presets { get; } = PlanetPresets.Presets.Select(x => new PlanetPresetViewModel(x)).ToArray();

	public PlanetPresetViewModel SelectedPreset { get; set; }

	internal void OnSelectedPresetChanged()
	{
		if (SelectedPreset == null)
		{
			return;
		}

		_preventPresetChanges = true;
		RzBigNumber = SelectedPreset.Preset.R;
		MzBigNumber = SelectedPreset.Preset.M;
		_preventPresetChanges = false;
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
