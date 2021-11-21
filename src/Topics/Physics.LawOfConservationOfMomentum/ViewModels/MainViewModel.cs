using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.LawOfConservationOfMomentum.Dialogs;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.LawOfConservationOfMomentum.Rendering;
using Physics.LawOfConservationOfMomentum.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation;
using Windows.UI.Xaml;
using Physics.LawOfConservationOfMomentum.ValuesTable;
using Windows.UI;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.Services.Sounds;

namespace Physics.LawOfConservationOfMomentum.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<LawOfConservationOfMomentumCanvasController>
	{
		private DifficultyOption _difficulty;
		private LawOfConservationOfMomentumCanvasController _controller;
		private readonly ISoundPlayer _soundPlayer;

		public MainViewModel(ISoundPlayer soundPlayer)
		{
			_soundPlayer = soundPlayer;
		}

		public CollisionType[] CollisionTypes { get; set; }

		public int SelectedCollisionTypeIndex { get; set; } = 0;

		public CollisionType SelectedCollisionType => (CollisionType)SelectedCollisionTypeIndex;

		public MotionViewModel Motion { get; set; }

		public MotionSetup Setup { get; set; }

		internal LawOfConservationOfMomentumCanvasController CreateController(ISkiaCanvas canvas) => new LawOfConservationOfMomentumCanvasController(canvas, _soundPlayer);

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

		public ICommand SetParametersCommand => GetOrCreateAsyncCommand(async () =>
		{
			SetupCollisionDialogViewModel viewModel;
			if (Motion != null && SelectedCollisionType == Motion.Setup.Type)
			{
				viewModel = new SetupCollisionDialogViewModel(SelectedCollisionType, Motion.Setup);
			}
			else
			{
				viewModel = new SetupCollisionDialogViewModel(SelectedCollisionType);
			}

			var dialog = new SetupCollisionDialog();
			dialog.DataContext = viewModel;
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				Setup = viewModel.Result;
				Motion = new MotionViewModel(Setup);
				await StartSimulationAsync();
			}
		});

		private async Task ShowValuesTableAsync()
		{
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));
			var physicsService = new PhysicsService(Setup);
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

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;

			var collisionTypes = Enum.GetValues(typeof(CollisionType)).OfType<CollisionType>();

			if (_difficulty == DifficultyOption.Easy)
			{
				CollisionTypes = collisionTypes.Except(new[] { CollisionType.ImperfectlyElastic }).ToArray();
			}
			else
			{
				CollisionTypes = collisionTypes.ToArray();
			}
		}

		public void SetController(LawOfConservationOfMomentumCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		protected async Task StartSimulationAsync()
		{
			_controller.StartSimulation(Setup);
			//_timer.Start();
		}
	}
}
