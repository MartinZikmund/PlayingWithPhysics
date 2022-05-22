using System.Text;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.GravitationalFieldMovement.Services;
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
	private readonly IAppPreferences _appPreferences;

	public ValuesTableDialogViewModel(TableService tableService, IAppPreferences appPreferences)
		: base(tableService)
	{
		tableService.Owner = this;
		UpdateTable();
		_appPreferences = appPreferences;
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
		var lengthUnit = _appPreferences.LengthUnit == Logic.LengthUnit.Metric ? "m" : "AU";

		if (eventArgs.Column.Header.ToString() == "X")
		{
			eventArgs.Column.Header = $"x ({lengthUnit})";
		}

		if (eventArgs.Column.Header.ToString() == "Y")
		{
			eventArgs.Column.Header = $"y ({lengthUnit})";
		}

		if (eventArgs.Column.Header.ToString() == "H")
		{
			eventArgs.Column.Header = $"h ({lengthUnit})";
		}

		if (eventArgs.Column.Header.ToString() == "Phi")
		{
			eventArgs.Column.Header = $"φ (°)";
		}		

		if (eventArgs.Column.Header.ToString() == "R")
		{
			eventArgs.Column.Header = $"r (m)";
		}

		if (eventArgs.Column.Header.ToString() == "V")
		{
			eventArgs.Column.Header = "v (m.s⁻¹)";
		}

		if (eventArgs.Column.Header.ToString() == "T")
		{
			eventArgs.Column.Header = "t (s)";
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
