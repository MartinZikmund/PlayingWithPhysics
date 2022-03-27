using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.ElectricParticle.Dialogs;
using Physics.ElectricParticle.Logic;
using Physics.ElectricParticle.Models;
using Physics.ElectricParticle.Rendering;
using Physics.ElectricParticle.ValuesTable;
using Physics.ElectricParticle.ViewModels.Inputs;
using Physics.ElectricParticle.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.ElectricParticle.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<ElectricParticleCanvasController>
	{
		private ElectricParticleCanvasController _controller;
		private readonly DispatcherTimer _timer = new DispatcherTimer();

		public MainViewModel()
		{
			_timer.Interval = TimeSpan.FromMilliseconds(10);
			_timer.Tick += _timer_Tick;
		}

		private void _timer_Tick(object sender, object e)
		{
			if (Motion != null)
			{
				Motion.UpdateCurrentValues(_controller.SimulationTime.UpdateCount);
			}
		}

		public Visibility RadiationVisibility { get; set; }
		public Visibility EasyDifficultyInputsVisibility { get; set; }
		public Visibility AdvancedDifficultyInputsVisibility { get; set; }

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

		private async Task ShowValuesTableAsync()
		{
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));
			var physicsService = new PhysicsService(Setup as ElectricParticleSimulationSetup);
			var valuesTableService = new TableService(physicsService);
			var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService);
			valuesTableViewModel.TimeInterval = (float)(physicsService.MaxT / 20);
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

		private DifficultyOption _difficulty;
		private int _selectedVariantIndex;
		private PhysicsService _physicsService;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
			if (_difficulty == DifficultyOption.Easy)
			{
				//Allow for basic variants enable inputs
				EasyDifficultyInputsVisibility = Visibility.Visible;
				AdvancedDifficultyInputsVisibility = Visibility.Collapsed;
				RadiationVisibility = Visibility.Collapsed;
			}
			else
			{
				//Allow for advanced variants, enable advanced inputs
				EasyDifficultyInputsVisibility = Visibility.Collapsed;
				AdvancedDifficultyInputsVisibility = Visibility.Visible;
			}
		}

		public int SelectedVariantIndex
		{
			get => _selectedVariantIndex;
			set
			{
				if (_selectedVariantIndex != value)
				{
					_selectedVariantIndex = value;
					if (_selectedVariantIndex > -1)
					{
						Motion = null;
						Setup = null;
						_controller.SetMotion(null);
						AddTrajectoryCommand?.Execute(null);
					}
				}
			}
		}

		InputVariant Variant => (InputVariant)(_difficulty == DifficultyOption.Advanced ? _selectedVariantIndex + 3 : _selectedVariantIndex);

		public IInputViewModel InputViewModel { get; set; }

		public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(async () =>
		{
			AddOrUpdateMovementDialog dialog;
			if (Motion != null && Variant == Motion.MotionInfo.Variant)
			{
				dialog = new AddOrUpdateMovementDialog(new MainInputViewModel(Variant, Motion.MotionInfo));
			}
			else
			{
				dialog = new AddOrUpdateMovementDialog(new MainInputViewModel(Variant));
			}

			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				Setup = dialog.Setup;
				Motion = new MotionViewModel(Setup);
				await StartSimulationAsync();
			}
		});

		public MotionViewModel Motion { get; set; }

		public ElectricParticleSimulationSetup Setup { get; set; }

		public Visibility ShowCurrentValues => (Setup != null) ? Visibility.Visible : Visibility.Collapsed;

		public void SetController(ElectricParticleCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		public ICommand RestartCommand => GetOrCreateAsyncCommand(RestartAsync);

		private async Task RestartAsync()
		{
			await StartSimulationAsync();
		}

		private async Task StartSimulationAsync()
		{
			if (_controller == null || Motion?.MotionInfo == null)
			{
				return;
			}

			_physicsService = new PhysicsService(Motion.MotionInfo);
			_timer.Start();
			await _controller.RunOnGameLoopAsync(() =>
			{
				SimulationPlayback.Play();
				_controller.SetMotion(Setup);
				_controller.StartSimulation();
			});
			FocusSimulationControls();
		}

		private void FocusSimulationControls()
		{
			((Window.Current.Content as Frame)?.Content as MainView)?.FocusSimulationControls();
		}
	}
}
