﻿using Physics.HomogenousParticle.ViewModels.Inputs;
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
    public sealed partial class PerpendicularVariantInputControl : UserControl
    {
        public PerpendicularVariantInputControl()
        {
            this.InitializeComponent();
            DataContextChanged += PerpendicularVariantInputControl_DataContextChanged;
            ChargeNumberBox.SetupFormatting(smallChange: 1, increment: 1);
            VelocityNumberBox.SetupFormatting(smallChange: 1, fractionDigits: 4, increment: 0.0001);
            MassNumberBox.SetupFormatting(smallChange: 0.000001, fractionDigits: 6, increment: 0.000001);
            InductionNumberBox.SetupFormatting(smallChange: 0.001, fractionDigits: 3, increment: 0.001);
        }

        private void PerpendicularVariantInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (PerpendicularVariantInputViewModel)args.NewValue;
        }

        public PerpendicularVariantInputViewModel Model { get; private set; }
    }
}
