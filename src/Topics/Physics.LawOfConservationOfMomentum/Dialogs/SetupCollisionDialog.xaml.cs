using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.LawOfConservationOfMomentum.ViewModels;
using Physics.Shared.UI.Helpers;
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
			SetupNumberBoxes();
		}

		public SetupCollisionDialog(SetupCollisionDialogViewModel viewModel) : this()
		{
			DataContext = viewModel;
			SetupNumberBoxes();
		}

		private void SetupCollisionDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			Model = args.NewValue as SetupCollisionDialogViewModel;
			SetupNumberBoxes();
		}

		public SetupCollisionDialogViewModel Model { get; private set; }

		private void SetupNumberBoxes()
		{
			MassOneNumberBox.SetupFormatting(smallChange: 0.1, increment: 0.1, fractionDigits: 1);
			MassTwoNumberBox.SetupFormatting(smallChange: 0.1, increment: 0.1, fractionDigits: 1);
			VelocityOneNumberBox.SetupFormatting(smallChange: 1, increment: 1, fractionDigits: 0);
			VelocityTwoNumberBox.SetupFormatting(smallChange: 1, increment: 1, fractionDigits: 0);
			CoefficientOfRestitutionNumberBox.SetupFormatting(smallChange: 0.01, increment: 0.01, fractionDigits: 2);
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{ 
		}
	}
}
