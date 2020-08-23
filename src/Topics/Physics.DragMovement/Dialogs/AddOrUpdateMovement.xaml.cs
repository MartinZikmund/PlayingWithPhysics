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
            DiameterNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 2, smallChange: 0.001);
            DensityNumberBox.SetupFormatting(fractionDigits: 2);
            GravityCoefficientNumberBox.SetupFormatting(fractionDigits: 2);
            EnvironmentDensityNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 2, smallChange: 0.001);
            MassNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 2, smallChange: 0.001);
            AreaNumberBox.SetupFormatting(increment: 0.01, fractionDigits: 2, smallChange: 0.01);
            ResistanceCoefficientNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.001);
            SpeedNumberBox.SetupFormatting(fractionDigits: 2);
            ElevationAngleNumberBox.SetupFormatting(fractionDigits: 2);
        }

        public AddOrUpdateMotionViewModel Model { get; }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Model?.SaveCommand.Execute(args);
        }
    }
}
