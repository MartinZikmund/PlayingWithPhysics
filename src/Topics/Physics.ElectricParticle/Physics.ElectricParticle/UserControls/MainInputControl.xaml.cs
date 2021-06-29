using Physics.ElectricParticle.ViewModels.Inputs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Physics.Shared.UI.Helpers;

namespace Physics.ElectricParticle.UserControls
{
	public sealed partial class MainInputControl : UserControl
    {
        public MainInputControl()
        {
            this.InitializeComponent();
			DataContextChanged += MainInputControl_DataContextChanged;
			SetupNumberBoxes();
        }

        private void MainInputControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
			if (Model != null)
			{
				Model.VariantConfigurationChanged -= Model_VariantConfigurationChanged;
			}

            Model = (MainInputViewModel)args.NewValue;
			Model.VariantConfigurationChanged += Model_VariantConfigurationChanged;
			SetupNumberBoxes();
		}

		private void Model_VariantConfigurationChanged(object sender, System.EventArgs e)
		{
			SetupNumberBoxes();
		}

		private void SetupNumberBoxes()
		{
			PrimaryVoltageNumberBox.SetupFormatting(smallChange: 100, increment: 100, largeChange: 1000, fractionDigits: 0);
			PrimaryPlaneDistance.SetupFormatting(smallChange: 0.01, increment: 0.01);			

			SecondaryVoltageNumberBox.SetupFormatting(smallChange: 100);
			SecondaryPlaneDistance.SetupFormatting(smallChange: 0.01, increment: 0.01);

			ChargeBaseNumberBox.SetupFormatting(smallChange: 1, increment: 1);
			MassPowerNumberBox.SetupFormatting(smallChange: 1, increment: 1);			

			VelocityNumberBox.SetupFormatting(smallChange: 1, increment: 1);
			
			DeviationNumberBox.SetupFormatting(smallChange: 1, increment: 1);

			if (Model != null)
			{
				if (Model.VariantConfiguration?.Q?.Step != null)
				{
					var qStep = Model.VariantConfiguration.Q.Step.Value;
					ChargeBaseNumberBox.SetupFormatting(smallChange: qStep, largeChange: qStep, increment: 0.1);
				}

				if (Model.VariantConfiguration?.UPrimary?.Step != null)
				{
					var uStep = Model.VariantConfiguration.UPrimary.Step.Value;
					PrimaryVoltageNumberBox.SetupFormatting(smallChange: uStep, increment: uStep, largeChange: uStep, fractionDigits: 0);			
				}

				if (Model.VariantConfiguration?.USecondary?.Step != null)
				{
					var uStep = Model.VariantConfiguration.USecondary.Step.Value;
					SecondaryVoltageNumberBox.SetupFormatting(smallChange: uStep, increment: uStep, largeChange: uStep, fractionDigits: 0);
				}

				if (Model.VariantConfiguration?.V0?.Step != null)
				{
					var v0Step = Model.VariantConfiguration.V0.Step.Value;
					VelocityNumberBox.SetupFormatting(smallChange: v0Step, increment: v0Step, largeChange: v0Step);
				}
			}

			if (Model?.VariantConfiguration != null)
			{
				PrimaryVoltageNumberBox.Minimum = Model.VariantConfiguration.UPrimary.Minimum;
				PrimaryVoltageNumberBox.Maximum = Model.VariantConfiguration.UPrimary.Maximum;
				SecondaryVoltageNumberBox.Minimum = Model.VariantConfiguration.USecondary.Minimum;
				SecondaryVoltageNumberBox.Maximum = Model.VariantConfiguration.USecondary.Maximum;
				VelocityNumberBox.Minimum = Model.VariantConfiguration.V0.Minimum;
				VelocityNumberBox.Maximum = Model.VariantConfiguration.V0.Maximum;
				MassPowerNumberBox.Minimum = Model.VariantConfiguration.M.Minimum;
				MassPowerNumberBox.Maximum = Model.VariantConfiguration.M.Maximum;
			}
		}

		public MainInputViewModel Model { get; private set; }
    }
}
