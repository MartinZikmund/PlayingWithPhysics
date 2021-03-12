using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.RadiationHalflife.ViewModels;
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

namespace Physics.RadiationHalflife.Dialogs
{
	public sealed partial class AddOrUpdateAnimationDialog : ContentDialog
	{
		public AddOrUpdateAnimationDialog()
		{
			this.InitializeComponent();
			DataContextChanged += AddOrUpdateAnimationDialog_DataContextChanged;
		}

		private void AddOrUpdateAnimationDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as AddOrUpdateAnimationViewModel;
		}

		public AddOrUpdateAnimationDialog(AddOrUpdateAnimationViewModel viewModel) : this()
		{
			DataContext = viewModel;
		}


		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		public AddOrUpdateAnimationViewModel Model { get; private set; }
	}
}
