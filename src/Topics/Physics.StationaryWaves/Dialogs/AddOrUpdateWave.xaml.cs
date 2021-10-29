using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.Shared.UI.Helpers;
using Physics.StationaryWaves.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.StationaryWaves.Dialogs
{
	public sealed partial class AddOrUpdateWave : ContentDialog
	{
		public AddOrUpdateWave()
		{
			this.InitializeComponent();
			DataContextChanged += AddOrUpdateWave_DataContextChanged;
			AmplitudeNumberBox.SetupFormatting(0.1, 1, 1, 0.1);
		}

		private void AddOrUpdateWave_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as AddOrUpdateWaveViewModel;
		}

		public AddOrUpdateWaveViewModel Model { get; private set; }
		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}
