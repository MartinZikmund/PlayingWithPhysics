using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;

namespace Physics.CyclicProcesses.ViewModels.Process
{
	public class IsochoricStateViewModel : ProcessStateViewModel
	{
		private readonly IsochoricInputConfiguration _inputConfiguration;
		private readonly IsochoricPhysicsService _physicsService;

		public IsochoricStateViewModel(IsochoricInputConfiguration inputConfiguration) : base(inputConfiguration)
		{
			_physicsService = new IsochoricPhysicsService(inputConfiguration);
			_inputConfiguration = inputConfiguration;
		}

		public float Q12 => _physicsService.Q12;
	}
}
