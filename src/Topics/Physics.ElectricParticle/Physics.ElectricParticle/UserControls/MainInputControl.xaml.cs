using Physics.ElectricParticle.ViewModels.Inputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Physics.Shared.UI.Helpers;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.ElectricParticle.UserControls
{
    public sealed partial class MainInputControl : UserControl
    {
        public MainInputControl()
        {
            this.InitializeComponent();
            DataContextChanged += MainInputControl_DataContextChanged;
            PrimaryVoltageNumberBox.SetupFormatting(smallChange: 100, increment: 100);
            PrimaryPlaneDistance.SetupFormatting(smallChange: 0.01, increment: 0.01);
            SecondaryVoltageNumberBox.SetupFormatting(smallChange: 100);
            SecondaryPlaneDistance.SetupFormatting(smallChange: 0.01, increment: 0.01);
            ChargeBaseNumberBox.SetupFormatting(smallChange: 0.1, increment: 0.1);
            MassBaseNumberBox.SetupFormatting(smallChange: 0.1, increment: 0.1);
            ChargePowerNumberBox.SetupFormatting(smallChange: 1, increment: 1);
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
