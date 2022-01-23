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
				_inputConfigurationCache[SelectedProcessType] = result;
			}
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
