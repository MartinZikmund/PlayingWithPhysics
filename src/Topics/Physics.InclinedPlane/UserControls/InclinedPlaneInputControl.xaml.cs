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
            InclinedPlaneLengthNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.1);
            //InclinedPlaneMassNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.1);
            InclinedPlaneAngleNumberBox.SetupFormatting(increment: 0.1, fractionDigits: 1, smallChange: 1);
            DriftCoefficientNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 2, smallChange: 0.1);
            //GravityNumberBox.SetupFormatting(increment: 0.0001, fractionDigits: 4, smallChange: 0.1);
        }

        private void AdvancedVariantInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (InclinedPlaneInputViewModel)args.NewValue;
        }

        public InclinedPlaneInputViewModel Model { get; private set; }
}
}
