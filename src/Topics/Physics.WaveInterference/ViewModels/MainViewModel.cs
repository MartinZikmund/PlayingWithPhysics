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
		IReceiveController<CompoundOscillationsController>
	{
		private CompoundOscillationsController _controller;
		private readonly IContentDialogHelper _contentDialogHelper;

		private readonly DispatcherTimer _timer = new DispatcherTimer();

		internal DifficultyOption Difficulty { get; private set; }

		public MainViewModel(IContentDialogHelper contentDialogHelper)
		{
			_contentDialogHelper = contentDialogHelper;
			_timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			_timer.Tick += _timer_Tick;
		}

		public ICommand EditOscillationCommand => GetOrCreateAsyncCommand(EditOscillationAsync);

		public ICommand DeleteOscillationCommand => GetOrCreateAsyncCommand<WaveInfoViewModel>(DeleteTrajectoryAsync);

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<WaveInfoViewModel>(ShowValuesTableAsync);

		public ICommand ShowWaveInterferenceValuesTableCommand => GetOrCreateAsyncCommand(ShowWaveInterferenceValuesTableAsync);

		public string TimeElapsed => _controller?.GetSimulationDisplayTime().ToString("0.00") ?? "0.00";

		public override void Prepare(DifficultyNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

		public ObservableCollection<WaveInfoViewModel> Waves { get; set; } = new ObservableCollection<WaveInfoViewModel>();
		public WaveInfoViewModel Wave1 { get; set; }
		public WaveInfoViewModel Wave2 { get; set; }
		public void SetController(CompoundOscillationsController controller)
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
					dialogViewModel = new AddOrUpdateOscillationViewModel(Difficulty, Waves.Select(m => m.Label).ToArray());
				}
				else
				{
					dialogViewModel = new AddOrUpdateOscillationViewModel(Wave1.WaveInfo, Wave2.WaveInfo, SourceDistance, Difficulty, Waves.Select(m => m.Label).ToArray());
				}
				var dialog = new AddOrUpdateOscillationDialog();
				dialog.DataContext = dialogViewModel;
				var result = await dialog.ShowAsync();
				if (result == ContentDialogResult.Primary)
				{
					Waves.Clear();
					Wave1 = new WaveInfoViewModel(dialogViewModel.Result[0].Result);
					Wave2 = new WaveInfoViewModel(dialogViewModel.Result[1].Result);
					Waves.Add(Wave1);
					Waves.Add(Wave2);
					SourceDistance = dialogViewModel.SourceDistance;
					//OnOscillationsChange(dialogViewModel.Result);
					//await StartSimulationAsync();
				}
			}
			catch (Exception ex)
			{
				await _contentDialogHelper.ShowAsync("Error", ex.ToString());
			}
		}


		private async Task EditOscillationAsync()
		{
			var dialogViewModel = new AddOrUpdateOscillationViewModel(Wave1.WaveInfo, Wave2.WaveInfo, SourceDistance, Difficulty, Oscillations.Select(m => m.Label).ToArray());
			var dialog = new AddOrUpdateOscillationDialog(dialogViewModel);
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				//OnOscillationsChange(arg.WaveInfo);
				//await StartSimulationAsync();
			}
		}

		private async Task DeleteTrajectoryAsync(WaveInfoViewModel arg)
		{
			Waves.Remove(arg);
			//await StartSimulationAsync();
		}

		private async Task ShowValuesTableAsync(WaveInfoViewModel viewModel)
		{
			var physicsService = new WavePhysicsService(viewModel.WaveInfo);
			await ShowValuesTableAsync(physicsService, false);
		}

		private async Task ShowWaveInterferenceValuesTableAsync()
		{
			//var physicsService = new CompoundOscillationsPhysicsService(Oscillations.Where(o => o.IsEnabled).Select(o => o.WaveInfo).ToArray());
			var physicsServices = new WaveInterferencePhysicsService(new WavePhysicsService[] { new WavePhysicsService(Wave1.WaveInfo), new WavePhysicsService(Wave2.WaveInfo) });
			await ShowValuesTableAsync(physicsServices, true);
		}

		private async Task ShowValuesTableAsync(IWavePhysicsService physicsService, bool compound)
		{
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			string title = Wave1.Label;

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

		private void OnOscillationsChange(WaveInfo lastChanged)
		{
			if (Difficulty == DifficultyOption.Easy)
			{
				foreach (var oscillation in Oscillations)
				{
					oscillation.WaveInfo.Frequency = lastChanged.Frequency;
				}
			}
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
