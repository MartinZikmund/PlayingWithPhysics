using Physics.GravitationalFieldMovement.Logic;
using Physics.GravitationalFieldMovement.Services;
using Physics.GravitationalFieldMovement.ViewModels;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Windows.UI.Xaml.Controls;

namespace Physics.GravitationalFieldMovement.Dialogs;

public sealed partial class InputDialog : ContentDialog
{
	public InputDialog(DifficultyOption difficulty, InputConfiguration input, IAppPreferences appPreferences)
	{
		InitializeComponent();
		Model = new InputDialogViewModel(difficulty, input, appPreferences);
		SetupNumberBoxes();
	}

	public InputDialogViewModel Model { get; }

	private void SetupNumberBoxes()
	{
		HBigNumberBox.MantisaBox.Minimum = 0.0;
		V0BigNumberBox.MantisaBox.Minimum = 0.0;
		ElevationAngleNumberBox.SetupFormatting(0.1, smallChange: 0.1, largeChange: 1);
		InitialCoordinateAngleNumberBox.SetupFormatting(0.1, smallChange: 0.1, largeChange: 1);
	}
}
