using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.CyclicProcesses.Rendering;
using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;
using Physics.CyclicProcesses.ViewModels.Process;
using Physics.CyclicProcesses.Logic;
using System.Linq;
using Windows.ApplicationModel.VoiceCommands;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using Physics.CyclicProcesses.Dialogs;
using Windows.UI.Xaml.Controls;
using Windows.UI.WindowManagement;
using Physics.CyclicProcesses.ValuesTable;
using Physics.Shared.UI.Localization;
using Windows.UI.Xaml.Hosting;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.Foundation;

namespace Physics.CyclicProcesses.ViewModels
{
	public class AdvancedVariantViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<CyclicProcessesCanvasController>
	{
		private DifficultyOption _difficulty;
		private CyclicProcessesCanvasController _controller;
		private Dictionary<ProcessType, IInputConfiguration> _inputConfigurationCache = new Dictionary<ProcessType, IInputConfiguration>();

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

		public ICommand SetParametersCommand => GetOrCreateAsyncCommand(SetParametersAsync);

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

		public ProcessType[] ProcessTypes { get; } = Enum.GetValues(typeof(ProcessType)).OfType<ProcessType>().ToArray();

		public int SelectedProcessTypeIndex { get; set; }

		public ProcessType SelectedProcessType => SelectedProcessTypeIndex >= 0 ? ProcessTypes[SelectedProcessTypeIndex] : default;

		public IInputConfiguration Input { get; set; }

		public IPhysicsService PhysicsService { get; set; }

		public ProcessStateViewModel ProcessState { get; set; }		

		public void SetController(CyclicProcessesCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		public async Task SetParametersAsync()
		{
			_inputConfigurationCache.TryGetValue(SelectedProcessType, out var inputConfiguration);

			var inputDialog = new InputDialog(SelectedProcessType, inputConfiguration);
			if (await inputDialog.ShowAsync() == ContentDialogResult.Primary)
			{
				var result = inputDialog.Model.Result;
				Input = result;
				PhysicsService = PhysicsServiceFactory.GetPhysicsService(Input);
				ProcessState = ProcessStateViewModel.Create(Input);
				_inputConfigurationCache[SelectedProcessType] = result;
			}
		}

		private async Task ShowValuesTableAsync()
		{
			if (Input == null)
			{
				return;
			}

			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			string title = Localizer.Instance.GetString("ProcessType_" + SelectedProcessType.ToString("g"));

			IValuesTableDialogViewModel tableViewModel = null;
			if (PhysicsService is IBasicProcessPhysicsService basicProcessPhysicsService)
			{
				var valuesTableService = new BasicProcessTableService(basicProcessPhysicsService);
				tableViewModel = new BasicProcessTableDialogViewModel(valuesTableService);
			}
			(appWindowContentFrame.Content as ValuesTablePage).Initialize(tableViewModel);
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

		internal async void OnSelectedProcessTypeIndexChanged()
		{
			if (SelectedProcessTypeIndex < 0)
			{
				return;
			}

			await SetParametersAsync();
		}
	}
}
