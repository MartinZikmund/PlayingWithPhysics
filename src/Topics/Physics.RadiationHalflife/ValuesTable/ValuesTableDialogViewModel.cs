using System.Text;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.Shared.UI.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;

namespace Physics.RadiationHalflife.ValuesTable
{
	public class ValuesTableDialogViewModel : ValuesTableDialogViewModelBase<TableRow>
	{
		private DifficultyOption _difficulty;
		private PhenomenonVariant _variant;
		private PhysicsService _physicsService;
		public ValuesTableDialogViewModel(ITableService<TableRow> tableService, DifficultyOption movementType, PhenomenonVariant variant, PhysicsService physicsService)
			: base(tableService)
		{
			_difficulty = movementType;
			_variant = variant;
			_physicsService = physicsService;
		}

		internal void Reset(TableService tableService, DifficultyOption difficulty)
		{
			_tableService = tableService;
			_difficulty = difficulty;
			UpdateTable();
			OnPropertyChanged(nameof(ButtonVisibility));
		}

		public override void AdjustColumnHeaders(DataGridAutoGeneratingColumnEventArgs eventArgs)
		{
			if (eventArgs.Column.Header.ToString() == "Time")
			{
				eventArgs.Column.Header = "t";
			}

			if (eventArgs.Column.Header.ToString() == "OtherValue")
			{
				if (_variant == PhenomenonVariant.RadioactiveLaw)
				{
					eventArgs.Column.Header = "N";
				}
				else
				{
					eventArgs.Column.Header = "A";
				}
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
