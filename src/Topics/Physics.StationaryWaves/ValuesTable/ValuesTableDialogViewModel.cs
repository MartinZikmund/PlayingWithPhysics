using System.Text;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.Shared.UI.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Physics.StationaryWaves.ValuesTable
{
	public class ValuesTableDialogViewModel : ValuesTableDialogViewModelBase<TableRow>
	{
		private DifficultyOption _difficulty;
		private float _time = 0f;
		private float _distanceInterval = 0.05f;
		private ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

		public ValuesTableDialogViewModel(TableService tableService, DifficultyOption movementType)
			: base(tableService)
		{
			_difficulty = movementType;
			tableService.Owner = this;
			_time = movementType == DifficultyOption.Advanced ? 2.1f : 0f;
			UpdateTable();
		}

		public float Time
		{
			get => _time;

			set
			{
				_time = value;
			}
		}

		public float DistanceInterval
		{
			get => _distanceInterval;
			set
			{
				_distanceInterval = value;
			}
		}
		internal void OnTimeChanged() => UpdateTable();

		internal void OnDistanceIntervalChanged() => UpdateTable();

		internal void Reset(TableService tableService, DifficultyOption difficulty)
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
				eventArgs.Column.Header = "x (" + Localizer.Instance.GetString("MultipleOfLambda") + ")(m)";
			}

			if (eventArgs.Column.Header.ToString() == "Y1")
			{
				eventArgs.Column.Header = "y₁ (m)";
			}

			if (eventArgs.Column.Header.ToString() == "Y2")
			{
				eventArgs.Column.Header = "y₂ (m)";
			}

			if (eventArgs.Column.Header.ToString() == "Y")
			{
				eventArgs.Column.Header = "y₁ + y₂ (m)";
			}
		}

		public override void CopyToClipboard()
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
