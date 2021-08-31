using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.WaveInterference.Dialogs;
using Physics.WaveInterference.Logic;
using Physics.WaveInterference.Rendering;
using Physics.WaveInterference.ValuesTable;
using Physics.WaveInterference.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.Shared.UI.Views.Interactions;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.WaveInterference.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<DifficultyNavigationModel>,
		IReceiveController<WaveInterferenceController>
	{
		private WaveInterferenceController _controller;
		private readonly IContentDialogHelper _contentDialogHelper;

		private readonly DispatcherTimer _timer = new DispatcherTimer();

		internal DifficultyOption Difficulty { get; private set; }

		public MainViewModel(IContentDialogHelper contentDialogHelper)
		{
			_contentDialogHelper = contentDialogHelper;
			_timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			_timer.Tick += _timer_Tick;
		}

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<WaveInfoViewModel>(ShowValuesTableAsync);

		public ICommand ShowWaveInterferenceValuesTableCommand => GetOrCreateAsyncCommand(ShowWaveInterferenceValuesTableAsync);

		public string TimeElapsed => _controller?.GetSimulationDisplayTime().ToString("0.00") ?? "0.00";

		public override void Prepare(DifficultyNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

		public ObservableCollection<WaveInfoViewModel> Waves { get; set; } = new ObservableCollection<WaveInfoViewModel>();

		public WaveInfoViewModel Wave1 => Waves.Count > 0 ? Waves[0] : null;

		public WaveInfoViewModel Wave2 => Waves.Count > 0 ? Waves[1] : null;

		public bool AreWavesConfigured => Waves.Count > 0;

		public void SetController(WaveInterferenceController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
			SimulationPlayback.PlaybackSpeed = 0.5f;
		}

		public ObservableCollection<WaveInfoViewModel> Oscillations { get; } = new ObservableCollection<WaveInfoViewModel>();

		public async void AddOscillation()
		{
			try
			{
				var resourceLoader = ResourceLoader.GetForCurrentView();
				AddOrUpdateOscillationViewModel dialogViewModel;
				if (Wave1 == null || Wave2 == null)
				{
					dialogViewModel = new AddOrUpdateOscillationViewModel(Difficulty);
				}
				else
				{
					dialogViewModel = new AddOrUpdateOscillationViewModel(Wave1.WaveInfo, Wave2.WaveInfo, SourceDistance, Difficulty);
				}
				var dialog = new AddOrUpdateOscillationDialog();
				dialog.DataContext = dialogViewModel;
				var result = await dialog.ShowAsync();
				if (result == ContentDialogResult.Primary)
				{
					Waves.Clear();
					Waves.Add(new WaveInfoViewModel(dialogViewModel.Result[0].Result));
					Waves.Add(new WaveInfoViewModel(dialogViewModel.Result[1].Result));
					SourceDistance = dialogViewModel.SourceDistance;
					await RaisePropertyChanged(nameof(AreWavesConfigured));
				}
			}
			catch (Exception ex)
			{
				await _contentDialogHelper.ShowAsync("Error", ex.ToString());
			}
		}

		private async Task ShowValuesTableAsync(WaveInfoViewModel viewModel)
		{
			var physicsService = new WavePhysicsService(viewModel.WaveInfo);
			await ShowValuesTableAsync(viewModel.WaveInfo, physicsService, false);
		}

		private async Task ShowWaveInterferenceValuesTableAsync()
		{
			//var physicsService = new CompoundOscillationsPhysicsService(Oscillations.Where(o => o.IsEnabled).Select(o => o.WaveInfo).ToArray());
			var physicsServices = new WaveInterferencePhysicsService(new WavePhysicsService[] { new WavePhysicsService(Wave1.WaveInfo), new WavePhysicsService(Wave2.WaveInfo) });
			await ShowValuesTableAsync(null, physicsServices, true);
		}

		private async Task ShowValuesTableAsync(WaveInfo waveInfo, IWavePhysicsService physicsService, bool compound)
		{
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			string title = waveInfo?.Label ?? Localizer.Instance.GetString("WaveInterference");

			var valuesTableService = new TableService(physicsService, compound);
			var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, Difficulty);
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
			newWindow.RequestSize(new Size(480, 300));
			var shown = await newWindow.TryShowAsync();
		}

		private async Task StartSimulationAsync()
		{
			if (_controller == null)
			{
				return;
			}
			_compoundOscillationService = new CompoundOscillationsPhysicsService(Oscillations.Select(o => o.WaveInfo).ToArray());
			_timer.Start();
			await _controller.RunOnGameLoopAsync(() =>
			{
				SimulationPlayback.Play();
				_controller.SetActiveOscillations(Oscillations.ToArray());
				_controller.StartSimulation();
			});
			FocusSimulationControls();
		}

		private void FocusSimulationControls()
		{
			((Window.Current.Content as Frame)?.Content as MainView)?.FocusSimulationControls();
		}

		public override void ViewDestroy(bool viewFinishing = true)
		{
			base.ViewDestroy(viewFinishing);
			_timer.Stop();
		}

		public CompoundOscillationsPhysicsService _compoundOscillationService = new CompoundOscillationsPhysicsService();

		public float SourceDistance { get; set; }
		public string CurrentCompoundY { get; set; }

		private void _timer_Tick(object sender, object e)
		{
			if (_timer.IsEnabled && _controller != null)
			{
				float timeElapsed = (float)_controller.GetSimulationDisplayTime();

				RaisePropertyChanged(nameof(TimeElapsed));

				foreach (var motion in Oscillations)
				{
					motion.UpdateCurrentValues(timeElapsed);
				}

				CurrentCompoundY = _compoundOscillationService.CalculateY(timeElapsed).ToString(" 0.00;-0.00; 0.00");
			}
		}
	}
}
