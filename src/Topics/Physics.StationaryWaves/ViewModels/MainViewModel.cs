using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.StationaryWaves.Logic;
using Physics.StationaryWaves.Rendering;
using Physics.StationaryWaves.ValuesTable;
using Physics.StationaryWaves.Views;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.StationaryWaves.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<StationaryWavesCanvasController>
	{
		private StationaryWavesCanvasController _controller;
		private readonly IContentDialogHelper _contentDialogHelper;

		private readonly DispatcherTimer _timer = new DispatcherTimer();
		internal DifficultyOption Difficulty { get; private set; }

		public override void Prepare(SimulationNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

		public WaveInfoViewModel Wave { get; private set; }
		public bool IsWaveConfigured => Wave != null;
		public MainViewModel(IContentDialogHelper contentDialogHelper)
		{
			_contentDialogHelper = contentDialogHelper;
		}

		public DisplaySettingsViewModel DisplaySettings { get; } = new DisplaySettingsViewModel();

		public bool IsAdvanced => Difficulty == DifficultyOption.Advanced;

		public bool IsEasy => Difficulty == DifficultyOption.Easy;

		public float RightEndDistance { get; set; } = 2;

		public AdvancedBounceType AdvancedBounceType { get; set; } = AdvancedBounceType.Oscillating;

		public AdvancedBounceType[] AdvancedBounceTypes { get; } = Enum.GetValues(typeof(AdvancedBounceType))
			.OfType<AdvancedBounceType>()
			.ToArray();

		public BounceType EasyBounceType { get; set; } = BounceType.Oscillating;

		public BounceType[] EasyBounceTypes { get; } = Enum.GetValues(typeof(BounceType))
			.OfType<BounceType>()
			.ToArray();

		internal async void OnRightEndDistanceChanged() => await StartSimulationAsync();

		internal async void OnAdvancedBounceTypeChanged() => await StartSimulationAsync();

		internal async void OnEasyBounceTypeChanged() => await StartSimulationAsync();

		private async Task StartSimulationAsync()
		{
			if (_controller == null)
			{
				return;
			}
			//TODO:
			//_waveInterferencePhysicsService = new WaveInterferencePhysicsService(Waves.Select(o => o.WaveInfo).ToArray());
			_controller.SetDisplaySettings(DisplaySettings);
			_timer.Start();
			await _controller.RunOnGameLoopAsync(() =>
			{
				SimulationPlayback.Play();
				SimulationPlayback.PlaybackSpeed = 1f;
				SimulationPlayback.JumpSize = 0.2f;
				if (Difficulty == DifficultyOption.Easy)
				{
					_controller.StartSimulation(EasyBounceType, RightEndDistance);
				}
				else
				{
					_controller.StartSimulation((BounceType)(int)AdvancedBounceType, RightEndDistance * 2 * (float)Math.PI);
				}
			});
			FocusSimulationControls();
		}

		private void FocusSimulationControls()
		{
			((Window.Current.Content as Frame)?.Content as MainView)?.FocusSimulationControls();
		}

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

		private async Task ShowValuesTableAsync()
		{
			if (!(_controller?.Renderer?.WavePhysicsService is { } physicsService))
			{
				return;
			}

			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			string title = Localizer.Instance.GetString("StationaryWaves");

			var valuesTableService = new TableService(physicsService, Difficulty == DifficultyOption.Easy ? RightEndDistance: 1);
			float? initialTime = null;
			if (Difficulty == DifficultyOption.Easy && _controller != null)
			{
				initialTime = (float)_controller.Renderer?.GetAdjustedTotalTime();
			}
			var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, Difficulty, initialTime);
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

		public async void SetController(StationaryWavesCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_controller.SetVariantRenderer(Difficulty == DifficultyOption.Easy ?
				(StationaryWavesRenderer)new EasyWavesRenderer(_controller) :
				new AdvancedWavesRenderer(_controller));
			SimulationPlayback.SetController(_controller);
			await StartSimulationAsync();
		}
	}
}
