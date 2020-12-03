using Physics.CompoundOscillations.ViewModels;
using Physics.Shared.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.CompoundOscillations.Dialogs
{
	public sealed partial class AddOrUpdateOscillationDialog : ContentDialog
	{
		public AddOrUpdateOscillationDialog()
		{
			InitializeComponent();
			DataContextChanged += AddOrUpdateOscillationDialog_DataContextChanged;
			SetupNumberBoxFormattings();
		}

		public AddOrUpdateOscillationDialog(AddOrUpdateOscillationViewModel viewModel) : this()
		{
			DataContext = viewModel;
		}

		private void SetupNumberBoxFormattings()
		{
			AmplitudeNumberBox.SetupFormatting(fractionDigits: 1, smallChange: 0.1);
			FrequencyNumberBox.SetupFormatting(fractionDigits: 1, smallChange: 0.1);
			PhaseNumberBox.SetupFormatting(fractionDigits: 1, smallChange: 0.1);
		}

		private void AddOrUpdateOscillationDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as AddOrUpdateOscillationViewModel;
		}

		public AddOrUpdateOscillationViewModel Model { get; private set; }
	}
}
