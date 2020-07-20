using CSharpMath.Atom.Atoms;
using Physics.DragMovement.ViewModels;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.DragMovement.Dialogs
{
    public sealed partial class AddOrUpdateMovement : ContentDialog
    {
        public AddOrUpdateMovement(AddOrUpdateMotionViewModel viewModel)
        {
            this.InitializeComponent();
            Model = viewModel;
            SetupNumberBoxFormattings();
        }

        private void SetupNumberBoxFormattings()
        {
            StartXNumberBox.SetupFormatting(fractionDigits: 2);
            StartYNumberBox.SetupFormatting(fractionDigits: 2);
            MassNumberBox.SetupFormatting(fractionDigits: 2);
            AreaNumberBox.SetupFormatting(fractionDigits: 2);
            HateCoefficientNumberBox.SetupFormatting(fractionDigits: 2);
            HateCoefficientNumberBox.SmallChange = 0.1;
            SpeedNumberBox.SetupFormatting(fractionDigits: 2);
            ElevationAngleNumberBox.SetupFormatting(fractionDigits: 2);
        }

        public AddOrUpdateMotionViewModel Model { get; }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
