using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.StationaryWaves.Dialogs;
using Physics.StationaryWaves.Logic;
using Physics.StationaryWaves.Rendering;
using Physics.StationaryWaves.ValuesTable;
using Physics.StationaryWaves.Views;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
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

		public async void AddWave()
		{
			{
				try
				{
					var resourceLoader = ResourceLoader.GetForCurrentView();
					AddOrUpdateWaveViewModel dialogViewModel = new AddOrUpdateWaveViewModel(Difficulty);
					var dialog = new AddOrUpdateWave();
					dialog.DataContext = dialogViewModel;
					var result = await dialog.ShowAsync();
					if (result == ContentDialogResult.Primary)
					{
						Wave = new WaveInfoViewModel(dialogViewModel.ResultWaveInfo);
						Windows.Storage.ApplicationData.Current.LocalSettings.Values["ValueTable_Time"] = null;
						Windows.Storage.ApplicationData.Current.LocalSettings.Values["ValueTable_DistanceInterval"] = null;
						//await RaisePropertyChanged(nameof(AddWaveEnabled));
						await StartSimulationAsync();
					}
				}
				catch (Exception ex)
				{
					await _contentDialogHelper.ShowAsync("Error", ex.ToString());
				}
			}
		}

		private async Task StartSimulationAsync()
		{
			if (_controller == null)
			{
				return;
			}
			//TODO:
			//_waveInterferencePhysicsService = new WaveInterferencePhysicsService(Waves.Select(o => o.WaveInfo).ToArray());
			_timer.Start();
			await _controller.RunOnGameLoopAsync(() =>
			{
				SimulationPlayback.Play();
				_controller.SetActiveWaves(Wave.WaveInfo);
				_controller.StartSimulation();
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
			var physicsService = new AdvancedWavePhysicsService(Wave.WaveInfo);
			await ShowValuesTableAsync(Wave.WaveInfo, physicsService, false);
		}

		private async Task ShowValuesTableAsync(WaveInfo waveInfo, IWavePhysicsService physicsService, bool compound)
		{
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			string title = waveInfo?.Label ?? Localizer.Instance.GetString("StationaryWaves");

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
			newWindow.RequestSize(new Size(640, 400));
			var shown = await newWindow.TryShowAsync();
		}

		public void SetController(StationaryWavesCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}
	}
}
