using Physics.HomogenousParticle.Services;
using Physics.HomogenousParticle.ViewModels.Inputs;
using Windows.UI.Xaml.Controls;

namespace Physics.HomogenousParticle.Dialogs
{
    public sealed partial class AddOrUpdateMotionDialog : ContentDialog
    {
        public AddOrUpdateMotionDialog(IVariantInputViewModel viewModel)
        {
            this.InitializeComponent();
            Model = viewModel;
            SetupNumberBoxFormattings();
        }

        public IVariantInputViewModel Model { get; private set; }

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
