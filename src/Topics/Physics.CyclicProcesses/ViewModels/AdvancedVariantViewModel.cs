using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.CyclicProcesses.Rendering;
using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;
using Physics.CyclicProcesses.ViewModels.Process;

namespace Physics.CyclicProcesses.ViewModels
{
	public class AdvancedVariantViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<CyclicProcessesCanvasController>
	{
		private DifficultyOption _difficulty;
		private CyclicProcessesCanvasController _controller;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

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
	}
}
