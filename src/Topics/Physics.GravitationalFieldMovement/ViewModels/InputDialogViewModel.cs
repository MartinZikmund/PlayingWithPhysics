﻿using System.Linq;
using System.Text;
using Physics.GravitationalFieldMovement.Logic;
using Physics.GravitationalFieldMovement.Services;
using Physics.Shared.Helpers;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Physics.GravitationalFieldMovement.ViewModels;

public class InputDialogViewModel : ViewModelBase
{
	private readonly IAppPreferences _appPreferences;
	private bool _isInitializing = true;

	public InputDialogViewModel(DifficultyOption difficulty, InputConfiguration inputConfiguration, IAppPreferences appPreferences)
	{
		_appPreferences = appPreferences;
		_isInitializing = true;
		IsAdvanced = difficulty == DifficultyOption.Advanced;

		if (inputConfiguration != null)
		{
			RzBigNumber = inputConfiguration.RzBigNumber;
			if (appPreferences.LengthUnit == LengthUnit.Metric)
			{
				HBigNumber = inputConfiguration.HBigNumber;
			}
			else
			{
				HBigNumber = new BigNumber(MathHelpers.MetersToAstronomicalUnits((double)inputConfiguration.HBigNumber));
			}
			MzBigNumber = inputConfiguration.MzBigNumber;
			V0BigNumber = inputConfiguration.V0BigNumber;
			BetaDeg = inputConfiguration.BetaDeg;
			Phi0Deg = inputConfiguration.Phi0Deg;
		}
		else
		{
			var defaultHMeters = new BigNumber(9.0, 5);
			if (appPreferences.LengthUnit == LengthUnit.Metric)
			{
				HBigNumber = defaultHMeters;
			}
			else
			{
				HBigNumber = new BigNumber(MathHelpers.MetersToAstronomicalUnits((double)defaultHMeters));
			}
		}
		_isInitializing = false;
		ValidatePlanetPreset();
	}

	private BigNumber _mzBigNumberMinimum = new BigNumber(4.0, 9);
	private BigNumber _mzBigNumberMaximum = new BigNumber(2.0, 31);
	private BigNumber _rzBigNumberMinimum = new BigNumber(1.0, 2);
	private BigNumber _rzBigNumberMaximum = new BigNumber(7.0, 10);
	private BigNumber _mzRzBigNumberRatio = new BigNumber(3, 24);
	private BigNumber _v0BigNumberBoxMaximum = new BigNumber(1.0, 8);
	private BigNumber _rzBigNumber = new BigNumber(6.38, 6);
	private BigNumber _mzBigNumber = new BigNumber(5.97, 24);

	private bool _preventPresetChanges = false;

	public bool IsAdvanced { get; }

	public BigNumber RzBigNumber
	{
		get => _rzBigNumber;
		set
		{
			_rzBigNumber = value;
			ValidatePlanetPreset();
		}
	}

	public BigNumber MzBigNumber
	{
		get => _mzBigNumber;
		set
		{
			_mzBigNumber = value;
			ValidatePlanetPreset();
		}
	}

	public BigNumber HBigNumber { get; set; } = new BigNumber(9.0, 5);

	public BigNumber V0BigNumber { get; set; } = new BigNumber(7.0, 3);

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

		if (SelectedPreset == null)
		{
			var preset = Presets.FirstOrDefault(p => p.Preset.R == RzBigNumber && p.Preset.M == MzBigNumber);
			SelectedPreset = preset;
		}

		if (SelectedPreset == null && !_isInitializing)
		{
			var preset = Presets.FirstOrDefault(p => !p.Preset.IsReadOnly);
			preset.Preset.R = RzBigNumber;
			preset.Preset.M = MzBigNumber;
			SelectedPreset = preset;
		}

		//Check if new Rz and Mz are valid given the selected planet
		if (SelectedPreset is not null && !SelectedPreset.Preset.IsReadOnly)
		{
			SelectedPreset.Preset.R = RzBigNumber;
			SelectedPreset.Preset.M = MzBigNumber;
		}
	}

	public string LengthUnitText => _appPreferences.LengthUnit == LengthUnit.Metric ? "m" : "au";

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
		var savedSuccessfully = Save();
		args.Cancel = !savedSuccessfully;
	}

	public bool ValidateInput()
	{
		var allValid = true;

		var errorMessages = new StringBuilder();

		if (MzBigNumber < _mzBigNumberMinimum)
		{
			errorMessages.AppendLine(
				string.Format(Localizer.Instance.GetString("MzTooSmallErrorMessage"), _mzBigNumberMinimum.ToSimplifiedUpperIndexString()));
			allValid = false;
		}

		if (MzBigNumber > _mzBigNumberMaximum)
		{
			errorMessages.AppendLine(
				string.Format(Localizer.Instance.GetString("MzTooLargeErrorMessage"), _mzBigNumberMaximum.ToSimplifiedUpperIndexString()));
			allValid = false;
		}

		if (RzBigNumber < _rzBigNumberMinimum)
		{
			errorMessages.AppendLine(
				string.Format(Localizer.Instance.GetString("RzTooSmallErrorMessage"), _rzBigNumberMinimum.ToSimplifiedUpperIndexString()));
			allValid = false;
		}

		if (RzBigNumber > _rzBigNumberMaximum)
		{
			errorMessages.AppendLine(
				string.Format(Localizer.Instance.GetString("RzTooLargeErrorMessage"), _rzBigNumberMaximum.ToSimplifiedUpperIndexString()));
			allValid = false;
		}

		if ((MzBigNumber / RzBigNumber) > _mzRzBigNumberRatio)
		{
			errorMessages.AppendLine(
				string.Format(Localizer.Instance.GetString("MzRzRatioErrorMessage"), _mzRzBigNumberRatio.ToSimplifiedUpperIndexString()));
			allValid = false;
		}

		if (V0BigNumber > _v0BigNumberBoxMaximum)
		{
			errorMessages.AppendLine(
				string.Format(Localizer.Instance.GetString("V0TooLargeErrorMessage"), _v0BigNumberBoxMaximum.ToSimplifiedUpperIndexString()));
			allValid = false;
		}

		ErrorMessage = errorMessages.ToString().TrimEnd();

		return allValid;
	}

	public bool Save()
	{
		ErrorMessage = null;

		if (!ValidateInput())
		{
			return false;
		}

		Result = new InputConfiguration(
			RzBigNumber,
			MzBigNumber,
			_appPreferences.LengthUnit == LengthUnit.Metric ? HBigNumber : MathHelpers.AstronomicalUnitsToMeters((double)HBigNumber),
			V0BigNumber,
			BetaDeg,
			Phi0Deg);

		return true;
	}
}
