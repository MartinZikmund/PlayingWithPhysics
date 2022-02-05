 using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Helpers;
using Physics.GravitationalFieldMovement.ValuesTable;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Controls;

namespace Physics.GravitationalFieldMovement.Views
{
	public sealed partial class ValuesTablePage : Page
	{
		public ValuesTablePage()
		{
			this.InitializeComponent();
		}

		public void Initialize(ValuesTableDialogViewModel viewModel)
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

		public ValuesTableDialogViewModel Model { get; set; }

		private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			Model?.AdjustColumnHeaders(e);
		}
	}
}
