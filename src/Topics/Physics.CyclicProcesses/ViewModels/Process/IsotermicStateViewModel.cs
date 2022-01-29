using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;

namespace Physics.CyclicProcesses.ViewModels.Process
{
	public class IsotermicStateViewModel : ProcessStateViewModel
	{
		private readonly IsotermicInputConfiguration _inputConfiguration;
		private readonly IsotermicPhysicsService _physicsService;

		public IsotermicStateViewModel(IsotermicInputConfiguration inputConfiguration) : base(inputConfiguration)
		{
			_physicsService = new IsotermicPhysicsService(inputConfiguration);
			_inputConfiguration = inputConfiguration;
		}

		public float W12 => _physicsService.W12;

		public float Q12 => _physicsService.W12;

		public float DeltaVInDm => (_inputConfiguration.V2 - _inputConfiguration.V1) * 1000;
	}
}
