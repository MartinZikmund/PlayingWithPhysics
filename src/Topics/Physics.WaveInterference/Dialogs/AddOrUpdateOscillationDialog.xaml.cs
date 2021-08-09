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
			SourceDistanceNumberBox.SetupFormatting(smallChange: 0.01, fractionDigits: 2, increment: 0.01);
		}

		public AddOrUpdateOscillationDialog(AddOrUpdateOscillationViewModel viewModel) : this()
		{
			DataContext = viewModel;
		}



		private void AddOrUpdateOscillationDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as AddOrUpdateOscillationViewModel;
		}

		public AddOrUpdateOscillationViewModel Model { get; private set; }
	}
}
