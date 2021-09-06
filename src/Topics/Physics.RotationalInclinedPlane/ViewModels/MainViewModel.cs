using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.InclinedPlane.ValuesTable;
using Physics.RotationalInclinedPlane.Dialogs;
using Physics.RotationalInclinedPlane.Logic;
using Physics.RotationalInclinedPlane.Rendering;
using Physics.RotationalInclinedPlane.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<RotationalInclinedPlaneCanvasController>
	{
		private DifficultyOption _difficulty;
		private RotationalInclinedPlaneCanvasController _controller;
		private DispatcherTimer _timer = new DispatcherTimer()
		{
			Interval = new TimeSpan(0, 0, 0, 0, 100)
		};

		public MainViewModel()
		{
			_timer.Tick += _timer_Tick;
		}

		public ObservableCollection<MotionViewModel> Motions { get; } = new ObservableCollection<MotionViewModel>();

		public bool AddMotionCommandEnabled => Motions.Count < 5;

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<MotionViewModel>(ShowValuesTableAsync);

		public ICommand AddMotionCommand => GetOrCreateAsyncCommand(AddMotionAsync);

		public ICommand EditMotionCommand => GetOrCreateAsyncCommand<MotionViewModel>(EditValuesAsync);

		public ICommand DeleteMotionCommand => GetOrCreateAsyncCommand<MotionViewModel>(DeleteMotionAsync);

		public ICommand DuplicateMotionCommand => GetOrCreateAsyncCommand<MotionViewModel>(DuplicateMotionAsync);

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

		public void SetController(RotationalInclinedPlaneCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		private async Task ShowValuesTableAsync(MotionViewModel motionViewModel)
		{
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));
			var physicsService = new PhysicsService(motionViewModel.MotionInfo);
			var valuesTableService = new TableService(physicsService);
			var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, _difficulty);
			(appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
			// Attach the XAML content to the window.
			ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
			newWindow.Title = "Table";

			newWindow.TitleBar.BackgroundColor = (Color)Application.Current.Resources["AppThemeColor"];
			newWindow.TitleBar.ForegroundColor = Colors.White;
			newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
			newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
			newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
			newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
			newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
			newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
			newWindow.RequestSize(new Size(600, 400));
			var shown = await newWindow.TryShowAsync();
		}

		public async Task AddMotionAsync()
		{
			var inputViewModel = new InputDialogViewModel(_difficulty, Motions.Select(m => m.MotionInfo).ToArray());

			var dialog = new InputDialog(inputViewModel);
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				var setup = inputViewModel.CreateMotionSetup();
				var viewModel = new MotionViewModel(setup);
				Motions.Add(viewModel);
				ApplyInlinedPropertiesToAll(setup);
				RestartSimulation();
			}
		}

		public async Task EditValuesAsync(MotionViewModel motionViewModel)
		{
			var viewModel = new InputDialogViewModel(motionViewModel.MotionInfo, _difficulty, Motions.Except(new[] { motionViewModel }).Select(m => m.MotionInfo).ToArray());

			var dialog = new InputDialog(viewModel);
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				motionViewModel.MotionInfo = viewModel.CreateMotionSetup();
				ApplyInlinedPropertiesToAll(motionViewModel.MotionInfo);
				RestartSimulation();
			}
		}

		private async Task DuplicateMotionAsync(MotionViewModel motionViewModel)
		{
			var resourceLoader = ResourceLoader.GetForCurrentView();
			if (Motions.Count >= 5)
			{
				await new MessageDialog(resourceLoader.GetString("TooManyMotionsDescription"), resourceLoader.GetString("TooManyMotionsTitle")).ShowAsync();
				return;
			}
			var duplicateMotion = motionViewModel.MotionInfo.Clone();
			duplicateMotion.Label = $"{duplicateMotion.Label} ({resourceLoader.GetString("Copy")})";
			var viewModel = new InputDialogViewModel(motionViewModel.MotionInfo, _difficulty, Motions.Select(m => m.MotionInfo).ToArray());

			var dialog = new InputDialog(viewModel);
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				motionViewModel.MotionInfo = viewModel.CreateMotionSetup();
				ApplyInlinedPropertiesToAll(motionViewModel.MotionInfo);
				RestartSimulation();
			}
		}

		private void ApplyInlinedPropertiesToAll(MotionSetup source)
		{
			foreach (var motion in Motions)
			{
				var setup = new MotionSetup(
					motion.MotionInfo.Label,
					motion.MotionInfo.BodyType,
					motion.MotionInfo.Mass,
					source.Gravity,
					motion.MotionInfo.Radius,
					motion.MotionInfo.InclinedLength,
					motion.MotionInfo.InclinedAngle,
					source.HorizontalLength,
					motion.MotionInfo.Color);
				motion.MotionInfo = setup;
			}
		}

		public Task DeleteMotionAsync(MotionViewModel motionViewModel)
		{
			Motions.Remove(motionViewModel);
			RestartSimulation();
			return Task.CompletedTask;
		}

		protected virtual void RestartSimulation()
		{
			RaisePropertyChanged(nameof(AddMotionCommandEnabled));
			_controller.StartSimulation(Motions.Select(vm => vm.MotionInfo).ToArray());
			_timer.Start();
		}

		private void _timer_Tick(object sender, object e)
		{
			if (_timer.IsEnabled && _controller != null)
			{
				float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;

				foreach (var motion in Motions)
				{
					motion?.UpdateCurrentValues(timeElapsed);
				}
			}
		}

		public override void ViewDestroy(bool viewFinishing = true)
		{
			base.ViewDestroy(viewFinishing);
			_timer.Stop();
		}
	}
}
