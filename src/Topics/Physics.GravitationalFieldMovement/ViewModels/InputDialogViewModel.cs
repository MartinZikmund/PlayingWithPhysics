using Physics.GravitationalFieldMovement.Logic;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Physics.GravitationalFieldMovement.ViewModels;

public class InputDialogViewModel : ViewModelBase
{
	public InputDialogViewModel(DifficultyOption difficulty, InputConfiguration inputConfiguration)
	{
		IsAdvanced = difficulty == DifficultyOption.Advanced;

		if (inputConfiguration != null)
		{
			RzString = inputConfiguration.Rz.ToString();
			MzString = inputConfiguration.Mz.ToString();
			HString = inputConfiguration.H.ToString();
			V0String = inputConfiguration.V0.ToString();
			BetaDeg = inputConfiguration.BetaDeg;
			Phi0Deg = inputConfiguration.Phi0Deg;
		}
	}

	public bool IsAdvanced { get; }

	public string RzString { get; set; } = "6378000";

	public string MzString { get; set; } = "5.97E+24";

	public string HString { get; set; } = "900000";

	public string V0String { get; set; } = "7000";

	public double BetaDeg { get; set; } = 30;

	public double Phi0Deg { get; set; } = 90;

	public bool HasErrors => !string.IsNullOrEmpty(ErrorMessage);

	public string ErrorMessage { get; private set; }

	public InputConfiguration Result { get; private set; }

	public void Save(object sender, ContentDialogButtonClickEventArgs args)
	{
		bool allValid = true;
		allValid &= double.TryParse(RzString, out var rz);
		allValid &= double.TryParse(MzString, out var mz);
		allValid &= double.TryParse(HString, out var h);
		allValid &= double.TryParse(V0String, out var v0);

		if (allValid)
		{
			Result = new InputConfiguration(
				rz,
				mz,
				h,
				v0,
				BetaDeg,
				Phi0Deg);
		}
		else
		{
			ErrorMessage = Localizer.Instance.GetString("CannotParseInputNumbers");
			args.Cancel = true;
		}
	}
}
