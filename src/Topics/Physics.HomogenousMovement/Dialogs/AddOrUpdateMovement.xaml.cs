using Microsoft.UI.Xaml.Controls;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.Helpers;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Controls;
using Physics.Shared.UI.Helpers;

namespace Physics.HomogenousMovement.Dialogs
{
	public sealed partial class AddOrUpdateMotionDialog : ContentDialog
	{
		public AddOrUpdateMotionDialog(AddOrUpdateMotionViewModel viewModel)
		{
			this.InitializeComponent();
			Model = viewModel;
			SetupNumberBoxFormattings();
		}

		public AddOrUpdateMotionViewModel Model { get; }

		private void SetupNumberBoxFormattings()
		{
			StartXNumberBox.SetupFormatting(fractionDigits: 2);
			StartYNumberBox.SetupFormatting(fractionDigits: 2);
			GravityNumberBox.SetupFormatting(fractionDigits: 2);
			MassNumberBox.SetupFormatting(fractionDigits: 2);
			V0NumberBox.SetupFormatting(fractionDigits: 2);
			AngleNumberBox.SetupFormatting(fractionDigits: 2);
			GravityNumberBox.SmallChange = 0.1;
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			Model.SaveCommand?.Execute(args);
		}
	}
}
