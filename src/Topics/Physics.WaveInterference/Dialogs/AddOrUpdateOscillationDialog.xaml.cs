using Physics.WaveInterference.ViewModels;
using Physics.Shared.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.WaveInterference.Dialogs
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
			AmplitudeNumberBox.SetupFormatting(increment: 0.1, fractionDigits: 1, smallChange: 0.1);
			FrequencyNumberBox.SetupFormatting(fractionDigits: 2, smallChange: 0.1);
			PhaseInPiRadNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.001, largeChange: 0.005);
		}

		private void AddOrUpdateOscillationDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as AddOrUpdateOscillationViewModel;
		}

		public AddOrUpdateOscillationViewModel Model { get; private set; }
	}
}
