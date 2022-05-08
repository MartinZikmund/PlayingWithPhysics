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
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Physics.Shared.Mathematics;
using Microsoft.Toolkit.Uwp.Helpers;
using SkiaSharp;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using SkiaSharp.Views.UWP;

namespace Physics.GravitationalFieldMovement.ViewModels
{
	public class DemoViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<GravitationalFieldMovementCanvasController>
	{
		private DifficultyOption _difficulty;
		private GravitationalFieldMovementCanvasController _controller;
		private double _height = 0.0d;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

		public DemoViewModel()
		{
			SelectedDemo = DemoList[0];
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

		public List<DemoItemViewModel> DemoList { get; } = new List<DemoItemViewModel>()
		{
			new DemoItemViewModel("1. kosmická rychlost", new InputConfiguration(new BigNumber(6.38, 6), new BigNumber(5.97, 24), 2, 7900.229, 0, 90), ColorHelper.ToColor(PlanetPresets.Presets[0].ColorHex).ToSKColor()),
			new DemoItemViewModel("Eliptická dráha Halleovy komety", new InputConfiguration(new BigNumber(6.96, 8), new BigNumber(1.99, 30), new BigNumber(5.25, 12), 912, 0, 90), SKColors.Red),
			new DemoItemViewModel("Hyperbolická trajektorie", new InputConfiguration(new BigNumber(6.96, 8), new BigNumber(1.99, 30), new BigNumber(5, 11), new BigNumber(4, 4), -75, 90), SKColors.Red)
		};

		public DemoItemViewModel SelectedDemo { get; set; }

		public void OnSelectedDemoChanged()
		{
			if (SelectedDemo?.Input == null)
			{
				return;
			}
			StartSimulation();
		}

		public ICommand SetParametersCommand => GetOrCreateAsyncCommand(SetParametersAsync);

		public ICommand ShowDerivedParametersCommand => GetOrCreateAsyncCommand(ShowDerivedParametersAsync);

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
			string[] units = { $" {GetFormattedYearLabel(span.Days / 365)}", " dní", " hodin", " minut", " sekund" };

			return string.Join(" ",
				from index in Enumerable.Range(0, units.Length)
				where parts[index] > 0
				select parts[index] + units[index]);
		}

		private string GetFormattedYearLabel(int year) =>
			year switch
			{
				1 => "rok",
				<= 4 => "roky",
				> 4 => "let"
			};

		public string TimeText { get; private set; }

		public string HeightText => Height.ToString("0.000");
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
			var dialog = new InputDialog(_difficulty, Input);
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
			if (_controller == null || SelectedDemo?.Input == null)
			{
				return;
			}

			//_controller.Planet = SelectedPreset;
			_controller.SimulationTime.Reset();
			_controller.SetInputConfiguration(SelectedDemo.Input, SelectedDemo.Input.Dt);
			_controller.PlanetPaint.Color = SelectedDemo.Color;
			_controller.Play();
		}

		private async Task ShowDerivedParametersAsync()
		{
			if (Input == null)
			{
				return;
			}

			var outputBuilder = new StringBuilder();
			foreach (var property in typeof(InputConfiguration).GetProperties())
			{
				outputBuilder.Append(property.Name);
				outputBuilder.Append(": ");
				outputBuilder.Append(property.GetValue(Input));
				outputBuilder.AppendLine();
			}

			var dialog = new MessageDialog(outputBuilder.ToString(), "Output");
			await dialog.ShowAsync();
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
}
