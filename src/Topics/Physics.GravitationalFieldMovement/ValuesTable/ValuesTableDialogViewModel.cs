using System.Text;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.Shared.UI.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Physics.GravitationalFieldMovement.ValuesTable;

public class ValuesTableDialogViewModel : ValuesTableDialogViewModelBase<TableRow>
{
	private DifficultyOption _difficulty;
	private float _time = 0f;
	private float _distanceInterval = 0.05f;
	private ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

	public ValuesTableDialogViewModel(TableService tableService)
		: base(tableService)
	{
		tableService.Owner = this;
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
			eventArgs.Column.Header = "x (m)";
		}

		if (eventArgs.Column.Header.ToString() == "Y")
		{
			eventArgs.Column.Header = "y (m)";
		}

		if (eventArgs.Column.Header.ToString() == "V")
		{
			eventArgs.Column.Header = "v (m.s⁻¹)";
		}

		if (eventArgs.Column.Header.ToString() == "T")
		{
			eventArgs.Column.Header = "t (s)";
		}

		if (eventArgs.Column.Header.ToString() == "H")
		{
			eventArgs.Column.Header = "h (m)";
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
