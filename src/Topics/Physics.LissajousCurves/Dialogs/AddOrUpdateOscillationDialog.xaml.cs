﻿using Physics.LissajousCurves.ViewModels;
using Physics.Shared.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.LissajousCurves.Dialogs
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
			AmplitudeNumberBox.SetupFormatting(fractionDigits: 2, smallChange: 0.1, increment: 0.01);
			FrequencyNumberBox.SetupFormatting(fractionDigits: 3, smallChange: 0.001, increment: 0.001);
			PhaseInPiRadNumberBox.SetupFormatting(fractionDigits: 1, smallChange: 1);
		}

		private void AddOrUpdateOscillationDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as AddOrUpdateOscillationViewModel;
		}

		public AddOrUpdateOscillationViewModel Model { get; private set; }
	}
}
