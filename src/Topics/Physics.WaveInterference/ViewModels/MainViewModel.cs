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

		public ICommand EditOscillationCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(EditOscillationAsync);

		public ICommand DuplicateOscillationCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(DuplicateOscillationAsync);

		public ICommand DeleteOscillationCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(DeleteTrajectoryAsync);

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(ShowValuesTableAsync);

		public ICommand ShowCompoundOscillationValuesTableCommand => GetOrCreateAsyncCommand(ShowCompoundOscillationValuesTableAsync);

		public string TimeElapsed => _controller?.GetSimulationDisplayTime().ToString("0.00") ?? "0.00";

		public override void Prepare(DifficultyNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

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

		public ObservableCollection<OscillationInfoViewModel> Oscillations { get; } = new ObservableCollection<OscillationInfoViewModel>();

		public async void AddOscillation()
		{
			try
			{
				var resourceLoader = ResourceLoader.GetForCurrentView();
				var dialogViewModel = new AddOrUpdateOscillationViewModel(Difficulty, Oscillations.Select(m => m.Label).ToArray());
				var dialog = new AddOrUpdateOscillationDialog();
				dialog.DataContext = dialogViewModel;
				var result = await dialog.ShowAsync();
				if (result == ContentDialogResult.Primary)
				{
					Oscillations.Add(new OscillationInfoViewModel(dialogViewModel.Result));
					OnOscillationsChange(dialogViewModel.Result);
					await StartSimulationAsync();
				}
			}
			catch (Exception ex)
			{
				await _contentDialogHelper.ShowAsync("Error", ex.ToString());
			}
		}


		private async Task EditOscillationAsync(OscillationInfoViewModel arg)
		{
			var dialogViewModel = new AddOrUpdateOscillationViewModel(arg.OscillationInfo, Difficulty, Oscillations.Select(m => m.Label).ToArray());
			var dialog = new AddOrUpdateOscillationDialog(dialogViewModel);
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				OnOscillationsChange(arg.OscillationInfo);
				await StartSimulationAsync();
			}
		}

		private async Task DuplicateOscillationAsync(OscillationInfoViewModel arg)
		{
			var duplicateMotion = arg.OscillationInfo.Clone();
			duplicateMotion.Label =
				$"{duplicateMotion.Label} ({Localizer.Instance.GetString("Copy")})";
			var dialogViewModel = new AddOrUpdateOscillationViewModel(duplicateMotion, Difficulty, Oscillations.Select(m => m.Label).ToArray());
			var dialog = new AddOrUpdateOscillationDialog(dialogViewModel);
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				Oscillations.Add(new OscillationInfoViewModel(dialogViewModel.Result));
				await StartSimulationAsync();
			}
		}

		private async Task DeleteTrajectoryAsync(OscillationInfoViewModel arg)
		{
			Oscillations.Remove(arg);
			await StartSimulationAsync();
		}

		private async Task ShowValuesTableAsync(OscillationInfoViewModel viewModel)
		{
			var physicsService = new OscillationPhysicsService(viewModel.OscillationInfo);
			await ShowValuesTableAsync(physicsService, viewModel.Label);
		}

		private async Task ShowCompoundOscillationValuesTableAsync()
		{
			var physicsService = new CompoundOscillationsPhysicsService(Oscillations.Where(o => o.IsEnabled).Select(o => o.OscillationInfo).ToArray());
			await ShowValuesTableAsync(physicsService, Localizer.Instance.GetString("CompoundOscillation"));
		}

		private async Task ShowValuesTableAsync(IOscillationPhysicsService physicsService, string title)
		{
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			var valuesTableService = new TableService(physicsService);
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
			_compoundOscillationService = new CompoundOscillationsPhysicsService(Oscillations.Select(o => o.OscillationInfo).ToArray());
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

		private void OnOscillationsChange(OscillationInfo lastChanged)
		{
			if (Difficulty == DifficultyOption.Easy)
			{
				foreach (var oscillation in Oscillations)
				{
					oscillation.OscillationInfo.Frequency = lastChanged.Frequency;
				}
			}
		}

		public override void ViewDestroy(bool viewFinishing = true)
		{
			base.ViewDestroy(viewFinishing);
			_timer.Stop();
		}

		public CompoundOscillationsPhysicsService _compoundOscillationService = new CompoundOscillationsPhysicsService();

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
