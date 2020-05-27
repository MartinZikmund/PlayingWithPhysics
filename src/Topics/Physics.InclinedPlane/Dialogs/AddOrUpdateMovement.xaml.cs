using Physics.InclinedPlane.Services;
using Physics.InclinedPlane.ViewModels;
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

namespace Physics.InclinedPlane.Dialogs
{
    public sealed partial class AddOrUpdateMovement : ContentDialog
    {
        public AddOrUpdateMovement(IVariantInputViewModel viewModel)
        {
            this.InitializeComponent();
            Model = viewModel;
            SetupNumberBoxFormattings();
        }

        private void SetupNumberBoxFormattings()
        {
            //StartXNumberBox.SetupFormatting(fractionDigits: 2);
            //StartYNumberBox.SetupFormatting(fractionDigits: 2);
            //GravityNumberBox.SetupFormatting(fractionDigits: 2);
            //MassNumberBox.SetupFormatting(fractionDigits: 2);
            //V0NumberBox.SetupFormatting(fractionDigits: 2);
            //AngleNumberBox.SetupFormatting(fractionDigits: 2);
            //GravityNumberBox.SmallChange = 0.1;
        }

        public IVariantInputViewModel Model { get; private set; }

        public IMotionSetup Setup { get; set; }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            Setup = await Model.CreateMotionSetupAsync();
            if (Setup == null)
            {
                args.Cancel = true;
            }
            deferral.Complete();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
