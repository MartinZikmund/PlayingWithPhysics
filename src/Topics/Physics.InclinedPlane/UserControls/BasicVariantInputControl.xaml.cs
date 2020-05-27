using Physics.InclinedPlane.ViewModels;
using Physics.Shared.UI.Helpers;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.InclinedPlane.UserControls
{
    public sealed partial class BasicVariantInputControl : UserControl
    {
        public BasicVariantInputControl()
        {
            this.InitializeComponent();
            DataContextChanged += BasicVariantInputControl_DataContextChanged;
            InclinedPlaneElevationNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.1);
            InclinedPlaneMassNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.1);
            InclinedPlaneAngleNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.1);
            DriftCoefficientNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 2, smallChange: 0.1);
        }

        private void BasicVariantInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (BasicVariantInputViewModel)args.NewValue;
        }

        public BasicVariantInputViewModel Model { get; private set; }
    }
}
