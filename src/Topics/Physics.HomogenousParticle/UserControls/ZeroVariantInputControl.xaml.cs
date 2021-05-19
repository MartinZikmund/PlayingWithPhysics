using Physics.HomogenousParticle.ViewModels.Inputs;
using Physics.Shared.Helpers;
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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Physics.Shared.UI.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.HomogenousParticle.UserControls
{
    public sealed partial class ZeroVariantInputControl : UserControl
    {
        public ZeroVariantInputControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += ZeroVariantInputControl_DataContextChanged;
            ChargeNumberBox.SetupFormatting(increment: 1, smallChange: 1);
            OrientationNumberBox.SetupFormatting(fractionDigits: 0);
        }

        private void ZeroVariantInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (ZeroVariantInputViewModel)args.NewValue;
        }

        public ZeroVariantInputViewModel Model { get; private set; }

        private void NumberBox_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {
            if (args.NewValue != 0 && args.NewValue < 2)
            {
                //VelocityNumberBox.Value = args.OldValue;
            }
        }
    }
}
