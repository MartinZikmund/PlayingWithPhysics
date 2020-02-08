using Microsoft.UI.Xaml.Controls;
using Physics.HomogenousMovement.ViewModels;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Controls;

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

        private void SetupFromatting(NumberBox numberBox, int decimalPlaces)
        {
            var rounder = new IncrementNumberRounder();
            rounder.Increment = 0.1;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            var formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = decimalPlaces;
            formatter.NumberRounder = rounder;
            numberBox.NumberFormatter = formatter;
        }

        private void SetupNumberBoxFormattings()
        {
            SetupFromatting(StartXNumberBox, 2);
            SetupFromatting(StartYNumberBox, 2);
            SetupFromatting(GravityNumberBox, 2);
            SetupFromatting(MassNumberBox, 2);
            SetupFromatting(V0NumberBox, 2);
            SetupFromatting(AngleNumberBox, 2);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Model.SaveCommand?.Execute(args);
        }
    }
}
