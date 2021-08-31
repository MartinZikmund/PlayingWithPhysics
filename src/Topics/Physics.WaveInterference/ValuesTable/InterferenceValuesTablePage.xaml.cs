 using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Helpers;
using Physics.WaveInterference.ValuesTable;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Controls;

namespace Physics.WaveInterference.Views
{
	public sealed partial class InterferenceValuesTablePage : Page
	{
		public InterferenceValuesTablePage()
		{
			this.InitializeComponent();
		}

		public void Initialize(InterferenceValuesTableDialogViewModel viewModel)
		{
			Model = viewModel;
			DataContext = Model;
			SetupFormatting();
		}

		private void SetupFormatting()
		{
			TimeNumberBox.SetupFormatting(0.1, 1, 1, 0.1, 1);
			DistanceIntervalNumberBox.SetupFormatting(0.1, 1, 1, 0.1, 1);
		}

		public InterferenceValuesTableDialogViewModel Model { get; set; }

		private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			Model?.AdjustColumnHeaders(e);
		}
	}
}
