using Physics.ElectricParticle.ViewModels.Inputs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Physics.Shared.UI.Helpers;

namespace Physics.ElectricParticle.UserControls
{
	public sealed partial class MainInputControl : UserControl
    {
        public MainInputControl()
        {
            this.InitializeComponent();
            DataContextChanged += MainInputControl_DataContextChanged;
            PrimaryVoltageNumberBox.SetupFormatting(smallChange: 100, increment: 100, largeChange: 1000, fractionDigits: 0);
            PrimaryPlaneDistance.SetupFormatting(smallChange: 0.01, increment: 0.01);
            SecondaryVoltageNumberBox.SetupFormatting(smallChange: 100);
            SecondaryPlaneDistance.SetupFormatting(smallChange: 0.01, increment: 0.01);
            ChargeBaseNumberBox.SetupFormatting(smallChange: 0.1, increment: 0.1);
            MassPowerNumberBox.SetupFormatting(smallChange: 1, increment: 1);
            VelocityNumberBox.SetupFormatting(smallChange: 1, increment: 1);
            DeviationNumberBox.SetupFormatting(smallChange: 1, increment: 1);
        }

        private void MainInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (MainInputViewModel)args.NewValue;
        }

        public MainInputViewModel Model { get; private set; }
    }
}
