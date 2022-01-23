using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.CyclicProcesses.ValuesTable
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ValuesTablePage : Page
	{
		public ValuesTablePage()
		{
			InitializeComponent();
		}

		public void Initialize(IValuesTableDialogViewModel viewModel)
		{
			Model = viewModel;
			DataContext = Model;
			SetupFormatting();
		}

		private void SetupFormatting()
		{
			//TimeNumberBox.SetupFormatting(0.01, 1, 2, 0.01, 0.1);
			//DistanceIntervalNumberBox.SetupFormatting(0.01, 1, 2, 0.01, 0.1);
		}

		public IValuesTableDialogViewModel Model { get; set; }

		private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			Model?.AdjustColumnHeaders(e);
		}
	}
}
