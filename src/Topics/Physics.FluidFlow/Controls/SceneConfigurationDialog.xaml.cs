using Microsoft.UI.Xaml.Controls;
using Physics.FluidFlow.Logic;
using Physics.FluidFlow.Logic;
using Physics.FluidFlow.ViewModels;
using Physics.Shared.UI.Helpers;
using Windows.UI.Xaml.Controls;

namespace Physics.FluidFlow.Controls
{
	public sealed partial class SceneConfigurationDialog : ContentDialog
	{
		public SceneConfigurationDialog(InputVariant inputVariant)
		{
			InitializeComponent();
			Model = new SceneConfigurationDialogViewModel(inputVariant, null);
			Model.InputConfigurationChanged += Model_InputConfigurationChanged;
			SetupNumberBoxes();
		}

		public SceneConfigurationDialogViewModel Model { get; }

		private void Model_InputConfigurationChanged(object sender, System.EventArgs e)
		{
			SetupNumberBoxes();
		}

		private void SetupNumberBoxes()
		{
			SetupNumberBox(VelocityNumberBox, Model.VelocityConfiguration);
			SetupNumberBox(HeightDecreaseNumberBox, Model.InputConfiguration.HeightDecreaseConfiguration);
			SetupNumberBox(LengthNumberBox, Model.InputConfiguration.LengthConfiguration);
			SetupNumberBox(DiameterNumberBox, Model.DiameterConfiguration);
			SetupNumberBox(Diameter1NumberBox, Model.Diameter1Configuration);
			SetupNumberBox(Diameter2NumberBox, Model.Diameter2Configuration);
		}

		private void SetupNumberBox(NumberBox numberBox, FieldConfiguration fieldConfiguration)
		{
			if (fieldConfiguration.IsVisible)
			{
				var fieldStep = fieldConfiguration.Step ?? 0.1f;
				numberBox.SetupFormatting(
					smallChange: fieldStep,
					largeChange: fieldStep,
					increment: GetIncrement(fieldStep),
					fractionDigits: GetFractionDigits(fieldStep));

				numberBox.Minimum = fieldConfiguration.Minimum;
				numberBox.Maximum = fieldConfiguration.Maximum;

				numberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact;
				numberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline;
			}
		}

		private int GetFractionDigits(float step) => step == 1 ? 0 : 1;

		private double GetIncrement(float step) => step == 1 ? 1 : 0.1;
	}
}
