using Microsoft.UI.Xaml.Controls;
using Physics.CyclicProcesses.Logic;
using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.ViewModels.Input;
using Windows.UI.Xaml.Controls;

namespace Physics.CyclicProcesses.Dialogs
{
	public sealed partial class InputDialog : ContentDialog
	{ 
		public InputDialog(ProcessType inputVariant, IInputConfiguration inputConfiguration)
		{
			InitializeComponent();
			Model = new InputDialogViewModel(inputVariant, inputConfiguration);
			SetupNumberBoxes();
		}

		public InputDialogViewModel Model { get; }

		private void SetupNumberBoxes()
		{
			//SetupNumberBox(VelocityNumberBox, Model.InputConfiguration.VelocityConfiguration);
			//SetupNumberBox(HeightDecreaseNumberBox, Model.InputConfiguration.HeightDecreaseConfiguration);
			//SetupNumberBox(LengthNumberBox, Model.InputConfiguration.LengthConfiguration);
			//SetupNumberBox(DiameterNumberBox, Model.InputConfiguration.DiameterConfiguration);
			//SetupNumberBox(Diameter1NumberBox, Model.InputConfiguration.Diameter1Configuration);
			//SetupNumberBox(Diameter2NumberBox, Model.InputConfiguration.Diameter2Configuration);
		}

		//private void SetupNumberBox(NumberBox numberBox)
		//{
		//	if (fieldConfiguration.IsVisible)
		//	{
		//		var fieldStep = fieldConfiguration.Step ?? 0.1f;
		//		numberBox.SetupFormatting(
		//			smallChange: fieldStep,
		//			largeChange: fieldStep,
		//			increment: GetIncrement(fieldStep),
		//			fractionDigits: GetFractionDigits(fieldStep));

		//		numberBox.Minimum = fieldConfiguration.Minimum;
		//		numberBox.Maximum = fieldConfiguration.Maximum;

		//		numberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact;
		//		numberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline;
		//	}
		//}

		private int GetFractionDigits(float step) => step == 1 ? 0 : 1;

		private double GetIncrement(float step) => step == 1 ? 1 : 0.1;
	}
}
