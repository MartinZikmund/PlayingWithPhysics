using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.GravitationalFieldMovement.Rendering;
using System.Windows.Input;
using System.Threading.Tasks;
using Physics.GravitationalFieldMovement.Dialogs;
using Physics.GravitationalFieldMovement.Logic;
using Windows.UI.Xaml.Controls;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using Physics.GravitationalFieldMovement.Views;
using Physics.Shared.UI.Localization;
using Physics.GravitationalFieldMovement.ValuesTable;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI;
using System.Linq;
using Physics.GravitationalFieldMovement.Services;
using Physics.Shared.Helpers;
using Physics.Shared.Mathematics;
using Windows.UI.Text;

namespace Physics.GravitationalFieldMovement.ViewModels;

public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<GravitationalFieldMovementCanvasController>
{
	private readonly IAppPreferences _appPreferences;
	
	private DifficultyOption _difficulty;
	protected GravitationalFieldMovementCanvasController _controller;
	private double _height = 0.0d;

	public MainViewModel(IAppPreferences appPreferences)
	{
		_appPreferences = appPreferences;
	}

	public override void Prepare(SimulationNavigationModel parameter)
	{
		_difficulty = parameter.Difficulty;
	}

	public InputConfiguration Input { get; private set; }

	public bool InputSet => Input != null;

	public double Dt { get; set; } = 1;

	public string Object =>
		IsKnownObject
		? Localizer.Instance[SelectedPreset.NameKey]
		: null;

	internal void OnDtChanged()
	{
		if (Input != null && _controller != null)
		{
			StartSimulation();
		}
	}

	public ICommand SetParametersCommand => GetOrCreateAsyncCommand(SetParametersAsync);

	public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

	public DispatcherTimer DispatcherTime { get; set; }

	public void SetController(GravitationalFieldMovementCanvasController controller)
	{
		if (controller is null)
		{
			throw new ArgumentNullException(nameof(controller));
		}

		DispatcherTime = new DispatcherTimer();
		DispatcherTime.Tick += TimerTick;
		DispatcherTime.Interval = TimeSpan.FromSeconds(1 / 30.0);
		DispatcherTime.Start();

		_controller = controller;
		SimulationPlayback.SetController(_controller);
		Input = InputConfiguration.Default;
		StartSimulation();
		SimulationPlayback.Pause();
	}

	private void TimerTick(object sender, object e)
	{
		if (_controller.CurrentPoint != null)
		{
			TimeText = GetFormattedTime((double)_controller.CurrentPoint.Time);
			Velocity = _controller.CurrentPoint.V;
			Height = _controller.CurrentPoint.H;
		}
	}

	//Source: https://rosettacode.org/wiki/Convert_seconds_to_compound_duration#C.23
	private string GetFormattedTime(double seconds)
	{
		if (seconds < 0) throw new ArgumentOutOfRangeException(nameof(seconds));
		if (seconds == 0) return "0 s";

		TimeSpan span = TimeSpan.FromSeconds(seconds);
		int[] parts = { span.Days / 365, span.Days % 365, span.Hours, span.Minutes, span.Seconds };
		string[] units = { $" {GetFormattedYearLabel(span.Days / 365)}", $" {GetFormattedDayLabel(span.Days % 365)}", " h", " m", " s" };

		return string.Join(", ",
			from index in Enumerable.Range(0, units.Length)
			where parts[index] > 0
			select parts[index] + units[index]);
	}

	private string GetFormattedYearLabel(int year) =>
		year switch
		{
			1 => Localizer.Instance.GetString("TimePartYear1"),
			<= 4 => Localizer.Instance.GetString("TimePartYear2"),
			> 4 => Localizer.Instance.GetString("TimePartYear5")
		};

	private string GetFormattedDayLabel(int days) =>
		days switch
		{
			1 => Localizer.Instance.GetString("TimePartDay1"),
			<= 4 => Localizer.Instance.GetString("TimePartDay2"),
			> 4 => Localizer.Instance.GetString("TimePartDay5")
		};

	public string TimeText { get; private set; }

	public int SelectedLengthUnitIndex
	{
		get => (int)LengthUnit;
		set
		{
			if (value >= 0)
			{
				LengthUnit = (LengthUnit)value;
			}
			RaisePropertyChanged(nameof(SelectedLengthUnitIndex));
		}
	}

	public LengthUnit[] LengthUnits { get; } = Enum.GetValues(typeof(LengthUnit)).OfType<LengthUnit>().ToArray();

	public LengthUnit LengthUnit
	{
		get => _appPreferences.LengthUnit;
		set => _appPreferences.LengthUnit = value;
	}

	public string HeightText
	{
		get
		{
			if (LengthUnit == LengthUnit.Metric)
			{
				return $"{Height.ToString("0.000")} m";
			}
			else
			{
				var heightInAu = MathHelpers.MetersToAstronomicalUnits(Height);
				return $"{new BigNumber(heightInAu)} AU";
			}
		}
	}

	public double Height
	{
		get => _height;

		private set
		{
			if (value < 0)
			{
				value = 0;
			}
			_height = value;
		}
	}
	public string VelocityText => Math.Round(Velocity).ToString();

	public double Velocity { get; private set; } = 0.0d;

	public string RadiusText
	{
		get
		{
			if (Input == null)
			{
				return "0";
			}

			var radiusInKilometers = (double)Input.RzBigNumber / 1000;
			if (radiusInKilometers >= 1000)
			{
				return Math.Round(radiusInKilometers).ToString();
			}
			return Math.Round(radiusInKilometers, 3).ToString();
		}
	}

	private async Task SetParametersAsync()
	{
		var dialog = new InputDialog(_difficulty, Input, _appPreferences);
		if (await dialog.ShowAsync() == ContentDialogResult.Primary)
		{

			var input = dialog.Model.Result;
			Input = null;
			Dt = input.Dt;
			Input = input;
			if (_difficulty == DifficultyOption.Advanced)
			{
				var preset = dialog.Model.SelectedPreset;
				SelectedPreset = preset?.Preset ?? null;
			}
			else
			{
				SelectedPreset = PlanetPresets.Presets[0];
			}
			_controller.SimulationTime.Reset();
			StartSimulation();
		}
	}

	private PlanetPreset _selectedPreset = null;

	private PlanetPreset SelectedPreset
	{
		get
		{
			return _difficulty == DifficultyOption.Easy ? PlanetPresets.Presets[0] : _selectedPreset;
		}
		set => _selectedPreset = value;
	}

	private void StartSimulation()
	{
		if (_controller == null || Input == null)
		{
			return;
		}

		_controller.Planet = SelectedPreset;
		_controller.SetInputConfiguration(Input, Dt);
		_controller.Play();
	}

	private async Task ShowValuesTableAsync()
	{
		if (Input is null)
		{
			return;
		}

		var newWindow = await AppWindow.TryCreateAsync();
		var appWindowContentFrame = new Frame();
		appWindowContentFrame.Navigate(typeof(ValuesTablePage));

		string title = Localizer.Instance.GetString("ShortAppName");
		var physicsService = new PhysicsService(Input, Dt);
		var valuesTableService = new TableService(physicsService, _controller?.CurrentPoint?.Time);
		var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService);
		(appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
		// Attach the XAML content to the window.
		ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
		newWindow.Title = title;

		newWindow.TitleBar.BackgroundColor = (Color)Application.Current.Resources["AppThemeColor"];
		newWindow.TitleBar.ForegroundColor = Colors.White;
		newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
		newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
		newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
		newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
		newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
		newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
		newWindow.RequestSize(new Size(640, 400));
		var shown = await newWindow.TryShowAsync();
	}

	public bool IsKnownObject => SelectedPreset != null;

	public FontWeight ObjectFontWeight => IsKnownObject ? FontWeights.Normal : FontWeights.Bold;
}
