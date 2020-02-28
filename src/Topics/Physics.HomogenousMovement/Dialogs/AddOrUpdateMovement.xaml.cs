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

        private void SetupFormatting(NumberBox numberBox, int decimalPlaces)
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
            SetupFormatting(StartXNumberBox, 2);
            SetupFormatting(StartYNumberBox, 2);
            SetupFormatting(GravityNumberBox, 2);
            SetupFormatting(MassNumberBox, 2);
            SetupFormatting(V0NumberBox, 2);
            SetupFormatting(AngleNumberBox, 2);
            GravityNumberBox.SmallChange = 0.1;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Model.SaveCommand?.Execute(args);
        }
    }
}
