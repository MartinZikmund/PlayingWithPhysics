using Microsoft.UI.Xaml.Controls;
using Physics.CyclicProcesses.Logic;
using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Input.Dialog;
using Physics.CyclicProcesses.ViewModels.Input;
using Physics.Shared.UI.Helpers;
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
			SetupNumberBox(NNumberBox, FieldConfiguration.CreateRestricted(0.1, 10, step: 0.1f));
			var tConfiguration = FieldConfiguration.CreateRestricted(1, 1000, step: 0.1f);
			SetupNumberBox(TNumberBox, tConfiguration);
			SetupNumberBox(T1NumberBox, tConfiguration);
			SetupNumberBox(T2NumberBox, tConfiguration);
			SetupNumberBox(T12NumberBox, tConfiguration);
			SetupNumberBox(T34NumberBox, tConfiguration);
			var pConfiguration = FieldConfiguration.CreateRestricted(1, 1000, step: 0.1f);
			SetupNumberBox(PNumberBox, pConfiguration);
			SetupNumberBox(P1NumberBox, pConfiguration);
			var vConfiguration = FieldConfiguration.CreateRestricted(1, 100, step: 0.1f);
			SetupNumberBox(VNumberBox, vConfiguration);
			SetupNumberBox(V1NumberBox, vConfiguration);
			SetupNumberBox(V2NumberBox, vConfiguration);
		}

		private void SetupNumberBox(NumberBox numberBox, FieldConfiguration fieldConfiguration)
		{
			if (fieldConfiguration.IsVisible)
			{
				var fieldStep = fieldConfiguration.Step ?? 0.1f;
				numberBox.SetupFormatting(
					smallChange: fieldStep,
					largeChange: fieldStep,
					increment: 0.1,
					fractionDigits: 1);

				numberBox.Minimum = fieldConfiguration.Minimum;
				numberBox.Maximum = fieldConfiguration.Maximum;

				numberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact;
				numberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline;
			}
		}
	}
}
