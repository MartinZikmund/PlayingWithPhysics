using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.LawOfConservationOfMomentum.ValuesTable;
using Physics.Shared.UI.Infrastructure.Topics;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.LawOfConservationOfMomentum.Views
{
	public sealed partial class ValuesTablePage : Page
	{
		private DifficultyOption _difficulty;
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

		//TODO: Translate button content
		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			//SwitchColumnsVisibility();
		}

		private void SwitchColumnsVisibility()
		{
			var xColumn = ValuesTable.Columns[1];
			var v0Column = ValuesTable.Columns[4];
			if (xColumn != null && v0Column != null)
			{
				Visibility newVisibility = Visibility.Visible;
				if (xColumn.Visibility == Visibility.Collapsed)
				{
					newVisibility = Visibility.Visible;
				}
				else
				{
					newVisibility = Visibility.Collapsed;
				}
				xColumn.Visibility = newVisibility;
				v0Column.Visibility = newVisibility;
			}
		}

		private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) =>
			Model?.AdjustColumnHeaders(e);
	}
}
