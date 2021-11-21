using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.HuygensPrinciple.ViewModels;
using Physics.Shared.UI.Infrastructure.Topics;
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

namespace Physics.HuygensPrinciple.Views
{
	public sealed partial class ScenePickerDialog : ContentDialog
	{
		public ScenePickerDialog(DifficultyOption difficulty)
		{
			this.InitializeComponent();
			ViewModel = new ScenePickerViewModel(difficulty);
		}

		public ScenePickerViewModel ViewModel { get; }

		public bool Ok { get; private set; } = false;

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			Ok = ViewModel.SelectedScene != null;
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}
