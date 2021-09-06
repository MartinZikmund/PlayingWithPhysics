using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.LawOfConservationOfMomentum.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Physics.LawOfConservationOfMomentum.Dialogs
{
	public sealed partial class SetupCollisionDialog : ContentDialog
	{
		public SetupCollisionDialog()
		{
			this.InitializeComponent();
			DataContextChanged += SetupCollisionDialog_DataContextChanged;
		}

		public SetupCollisionDialog(SetupCollisionDialogViewModel viewModel) : this()
		{
			DataContext = viewModel;
		}

		private void SetupCollisionDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as SetupCollisionDialogViewModel;
		}

		public SetupCollisionDialogViewModel Model { get; private set; }

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{ 
		}
	}
}
