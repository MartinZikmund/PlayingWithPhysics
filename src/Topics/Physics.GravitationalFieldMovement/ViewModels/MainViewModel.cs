using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.GravitationalFieldMovement.Rendering;
using System.Windows.Input;
using System.Threading.Tasks;
using Physics.GravitationalFieldMovement.Dialogs;
using Physics.GravitationalFieldMovement.Logic;
using Windows.UI.Xaml.Controls;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using Physics.GravitationalFieldMovement.Views;
using Physics.Shared.UI.Localization;
using Physics.GravitationalFieldMovement.ValuesTable;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI;

namespace Physics.GravitationalFieldMovement.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<GravitationalFieldMovementCanvasController>
	{
		private DifficultyOption _difficulty;
		private GravitationalFieldMovementCanvasController _controller;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

		public InputConfiguration Input { get; private set; }

		public double Dt { get; set; }
		public string Object { get; set; } = Localizer.Instance["Earth"];
		internal void OnDtChanged()
		{
			if (Input != null && _controller != null)
			{
				StartSimulation();
			}
		}

		public ICommand SetParametersCommand => GetOrCreateAsyncCommand(SetParametersAsync);

		public ICommand ShowDerivedParametersCommand => GetOrCreateAsyncCommand(ShowDerivedParametersAsync);

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);
		
		public void SetController(GravitationalFieldMovementCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		private async Task SetParametersAsync()
		{
			var dialog = new InputDialog(_difficulty, Input);
			if (await dialog.ShowAsync() == ContentDialogResult.Primary)
			{

				var input = dialog.Model.Result;
				Input = null;
				Dt = input.T / 30;//TODO
				Input = input;
				StartSimulation();
			}
		}

		private void StartSimulation()
		{
			if (_controller == null || Input == null)
			{
				return;
			}

			_controller.SetInputConfiguration(Input, Dt);
		}

		private async Task ShowDerivedParametersAsync()
		{
			if (Input == null)
			{
				return;
			}

			var outputBuilder = new StringBuilder();
			foreach (var property in typeof(InputConfiguration).GetProperties())
			{
				outputBuilder.Append(property.Name);
				outputBuilder.Append(": ");
				outputBuilder.Append(property.GetValue(Input));
				outputBuilder.AppendLine();
			}

			var dialog = new MessageDialog(outputBuilder.ToString(), "Output");
			await dialog.ShowAsync();
		}

		private async Task ShowValuesTableAsync()
		{
			if (Input is null)
			{
				return;
			}

			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			string title = Localizer.Instance.GetString("ShortAppName");
			var physicsService = new PhysicsService(Input, Dt);
			var valuesTableService = new TableService(physicsService);
			var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService);
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
	}
}
