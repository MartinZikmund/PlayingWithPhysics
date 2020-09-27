using DynamicData.Annotations;
using Physics.ElectricParticle.Logic;
using Physics.ElectricParticle.ViewModels;
using ReactiveUI;
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

namespace Physics.ElectricParticle.Dialogs
{
    public sealed partial class AddOrUpdateMovement : ContentDialog
    {
        public AddOrUpdateMovement()
        {
            this.InitializeComponent();
        }
        public AddOrUpdateMotionViewModel Model { get; private set; }
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

        public IMotionSetup Setup { get; set; }
    }
}
