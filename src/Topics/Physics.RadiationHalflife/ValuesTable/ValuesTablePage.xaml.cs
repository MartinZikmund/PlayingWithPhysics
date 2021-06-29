using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.RadiationHalflife.ValuesTable;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Controls;

namespace Physics.RadiationHalflife.Views
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
			var rounder = new IncrementNumberRounder();
			rounder.Increment = 0.01;
			rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

			var formatter = new DecimalFormatter();
			formatter.IntegerDigits = 1;
			formatter.FractionDigits = 2;
			formatter.NumberRounder = rounder;
			TimeIntervalNumberBox.NumberFormatter = formatter;
		}

		public ValuesTableDialogViewModel Model { get; set; }

		private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			Model?.AdjustColumnHeaders(e);
		}
	}
}
