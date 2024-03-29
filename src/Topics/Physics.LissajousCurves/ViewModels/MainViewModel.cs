﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.LissajousCurves.Dialogs;
using Physics.LissajousCurves.Logic;
using Physics.LissajousCurves.Rendering;
using Physics.LissajousCurves.Views;
using Physics.LissajousCurves.ValuesTable;
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

namespace Physics.LissajousCurves.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<DifficultyNavigationModel>,
		IReceiveController<LissajousCurvesController>
	{
		private LissajousCurvesController _controller;
		private readonly IContentDialogHelper _contentDialogHelper;

		internal DifficultyOption Difficulty { get; private set; }

		public MainViewModel(IContentDialogHelper contentDialogHelper)
		{
			_contentDialogHelper = contentDialogHelper;
		}
		public int DrawLength
		{
			get
			{
				if (_controller != null)
					return _controller.DrawLength;
				return 10;
			}
			set
			{
				_controller.DrawLength = value;
			}
		}

		public bool NoTrajectory
		{
			get
			{
				return _controller.NoTrajectory;
			}
			set
			{
				_controller.NoTrajectory = value;
			}
		}
		public bool NoAces
		{
			get
			{
				return _controller.NoAces;
			}
			set
			{
				_controller.NoAces = value;
			}
		}

		public OscillationInfoViewModel HorizontalOscillation { get; private set; } = new OscillationInfoViewModel(new OscillationInfo("X", 1, 1, 0, "#FF0000"));

		public OscillationInfoViewModel VerticalOscillation { get; private set; } = new OscillationInfoViewModel(new OscillationInfo("Y", 1, 1, 0, "#0000FF"));

		public ICommand EditOscillationCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(EditOscillationAsync);

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(ShowValuesTableAsync);

		public ICommand ShowCompoundOscillationValuesTableCommand => GetOrCreateAsyncCommand(ShowCompoundOscillationValuesTableAsync);

		public string TimeElapsed => _controller?.SimulationTime.TotalTime.TotalSeconds.ToString("0.00") ?? "0.00";

		public override void Prepare(DifficultyNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

		public void SetController(LissajousCurvesController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
			_controller.SetActiveOscillations(HorizontalOscillation.OscillationInfo, VerticalOscillation.OscillationInfo);
			_controller.StartSimulation();
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
					//TODO:
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
				arg.OscillationInfo = dialogViewModel.Result;
				//TODO:
				await StartSimulationAsync();
				//UpdateMotionAppWindow(arg);
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
			//TODO:
			//await CloseAppViewForMotionAsync(arg);
			await StartSimulationAsync();
		}

		private async Task ShowValuesTableAsync(OscillationInfoViewModel viewModel)
		{
			var physicsService = new OscillationPhysicsService(viewModel.OscillationInfo);
			await ShowValuesTableAsync(physicsService, viewModel.Label);
		}

		private async Task ShowCompoundOscillationValuesTableAsync()
		{
			var physicsService = new CompoundOscillationsPhysicsService(Oscillations.Where(o => o.IsVisible).Select(o => o.OscillationInfo).ToArray());
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
			await _controller.RunOnGameLoopAsync(() =>
			{
				SimulationPlayback.Play();
				_controller.SetActiveOscillations(HorizontalOscillation.OscillationInfo, VerticalOscillation.OscillationInfo);
				_controller.StartSimulation();
			});
		}
	}
}
