using System.Text;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.Shared.UI.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;

namespace Physics.WaveInterference.ValuesTable
{
	public class InterferenceValuesTableDialogViewModel : ValuesTableDialogViewModelBase<InterferenceTableRow>
	{
		private DifficultyOption _difficulty;

		public InterferenceValuesTableDialogViewModel(InterferenceTableService tableService, DifficultyOption movementType)
			: base(tableService)
		{
			_difficulty = movementType;
			tableService.Owner = this;
			UpdateTable();
		}

		public float Time { get; set; } = 0f;

		public float DistanceInterval { get; set; } = 0.1f;

		internal void OnTimeChanged() => UpdateTable();

		internal void OnDistanceIntervalChanged() => UpdateTable();

		internal void Reset(InterferenceTableService tableService, DifficultyOption difficulty)
		{
			_tableService = tableService;
			_difficulty = difficulty;
			UpdateTable();
			OnPropertyChanged(nameof(ButtonVisibility));
		}

		public override void AdjustColumnHeaders(DataGridAutoGeneratingColumnEventArgs eventArgs)
		{
			if (eventArgs.Column.Header.ToString() == "X")
			{
				eventArgs.Column.Header = "x (m)";
			}

			if (eventArgs.Column.Header.ToString() == "First")
			{
				eventArgs.Column.Header = string.Format(Localizer.Instance.GetString("TableHeaderWave"), 1) + " (m)";
			}

			if (eventArgs.Column.Header.ToString() == "Second")
			{
				eventArgs.Column.Header = string.Format(Localizer.Instance.GetString("TableHeaderWave"), 2) + " (m)";
			}

			if (eventArgs.Column.Header.ToString() == "Interference")
			{
				eventArgs.Column.Header = Localizer.Instance.GetString("TableHeaderInterference") + " (m)";
			}
		}

		public void CopyToClipboard()
		{
			var clipboardContents = new StringBuilder();
			foreach (var data in Values)
			{
				clipboardContents.AppendLine(data.ToTabString());
			}

			var dataPackage = new DataPackage();
			dataPackage.SetText(clipboardContents.ToString());
			Clipboard.SetContent(dataPackage);
		}

		public Visibility ButtonVisibility => (_difficulty == DifficultyOption.Easy)
			? Visibility.Visible
			: Visibility.Collapsed;
	}
}
