using Physics.InclinedPlane.ViewModels;
using Physics.Shared.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.InclinedPlane.UserControls
{
    public sealed partial class InclinedPlaneInputControl : UserControl
    {
        public InclinedPlaneInputControl()
        {
            this.InitializeComponent();
            DataContextChanged += AdvancedVariantInputControl_DataContextChanged;
            V0NumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
            InclinedPlaneLengthNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
            MassNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
            InclinedPlaneAngleNumberBox.SetupFormatting(increment: 0.1, fractionDigits: 1, smallChange: 1);
            DriftCoefficientNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
            HorizontalDriftCoefficientNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
            GravityNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
			HorizontalPlaneLengthNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
        }

        private void AdvancedVariantInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (InclinedPlaneInputViewModel)args.NewValue;
        }

        public InclinedPlaneInputViewModel Model { get; private set; }
}
}
