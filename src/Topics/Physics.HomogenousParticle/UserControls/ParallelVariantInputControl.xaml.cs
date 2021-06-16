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
    public sealed partial class ParallelVariantInputControl : UserControl
    {
        public ParallelVariantInputControl()
        {
            this.InitializeComponent();
            DataContextChanged += ParalllelVariantInputControl_DataContextChanged;
            ChargeNumberBox.SetupFormatting(smallChange: 1, increment: 1);
            VelocityNumberBox.SetupFormatting(smallChange: 1, integerDigits: 1, fractionDigits: 0, increment: 1);
            OrientationNumberBox.SetupFormatting();
        }

        private void ParalllelVariantInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (ParallelVariantInputViewModel)args.NewValue;
        }

        public ParallelVariantInputViewModel Model { get; private set; }
    }
}
