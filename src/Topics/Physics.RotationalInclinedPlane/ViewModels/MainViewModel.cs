using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.RotationalInclinedPlane.Rendering;
using Physics.RotationalInclinedPlane.Logic;
using Physics.RotationalInclinedPlane.Dialogs;
using System.Windows.Input;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI;
using Windows.Foundation;
using Physics.RotationalInclinedPlane.Views;
using Physics.InclinedPlane.ValuesTable;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<RotationalInclinedPlaneCanvasController>
	{
		private DifficultyOption _difficulty;
		private RotationalInclinedPlaneCanvasController _controller;

		public MotionSetup Setup { get; set; }

		public MotionViewModel Motion { get; set; }

		public Visibility ShowCurrentValuesGrid => (Setup != null) ? Visibility.Visible : Visibility.Collapsed;

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

		public ICommand EditValuesCommand => GetOrCreateAsyncCommand(EditValuesAsync);

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

		public async Task EditValuesAsync()
		{
			InputDialogViewModel viewModel;
			if (Setup != null)
			{
				viewModel = new InputDialogViewModel(Setup, _difficulty);
			}
			else
			{
				viewModel = new InputDialogViewModel(_difficulty);
			}

			var dialog = new InputDialog(viewModel);
			await dialog.ShowAsync();
		}
	}
}
