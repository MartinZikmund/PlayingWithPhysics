using Physics.ElectricParticle.Logic;
using Physics.ElectricParticle.ViewModels.Inputs;
using Windows.UI.Xaml.Controls;

namespace Physics.ElectricParticle.Dialogs
{
	public sealed partial class AddOrUpdateMovementDialog : ContentDialog
    {
        public AddOrUpdateMovementDialog(IInputViewModel viewModel)
        {
            this.InitializeComponent();
            Model = viewModel;
        }
        public IInputViewModel Model { get; private set; }
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

        public ElectricParticleSimulationSetup Setup { get; set; }
    }
}
