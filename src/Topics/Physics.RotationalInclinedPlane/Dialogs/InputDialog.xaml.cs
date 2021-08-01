using Physics.RotationalInclinedPlane.ViewModels;
using Physics.Shared.UI.Helpers;
using Windows.UI.Xaml.Controls;

namespace Physics.RotationalInclinedPlane.Dialogs
{
	public sealed partial class InputDialog : ContentDialog
	{
		public InputDialog(InputDialogViewModel viewModel)
		{
			InitializeComponent();
			Model = viewModel;
			InclinedPlaneLengthNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
			RadiusNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.001);
			MassNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
			InclinedPlaneAngleNumberBox.SetupFormatting(increment: 0.1, fractionDigits: 1, smallChange: 1);
			GravityNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
		}

		public InputDialogViewModel Model { get; }

		private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			var deferral = args.GetDeferral();
			args.Cancel = !await Model.ValidateAsync();
			deferral.Complete();
		}
	}
}
