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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.ElectricParticle.UserControls
{
    public sealed partial class HorizontalVariantInputControl : UserControl
    {
        public HorizontalVariantInputControl()
        {
            this.InitializeComponent();
            DataContextChanged += HorizontalVariantInputControl_DataContextChanged;
        }

        private void HorizontalVariantInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (HorizontalVariantInputViewModel)args.NewValue;
        }

        public HorizontalVariantInputViewModel Model { get; private set; }
    }
}
