using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;

namespace Physics.CyclicProcesses.ViewModels.Process
{
	public class IsobaricStateViewModel : ProcessStateViewModel
	{
		private readonly IsobaricInputConfiguration _inputConfiguration;
		private readonly IsobaricPhysicsService _physicsService;

		public IsobaricStateViewModel(IsobaricInputConfiguration inputConfiguration) : base(inputConfiguration)
		{
			_physicsService = new IsobaricPhysicsService(inputConfiguration);
			_inputConfiguration = inputConfiguration;
		}

		public float T1 => _physicsService.T1;

		public float T2 => _physicsService.T2;

		public float W12 => _physicsService.W12;

		public float Q12 => _physicsService.Q12;

		public float DeltaVInDm => (_inputConfiguration.V2 - _inputConfiguration.V1) * 1000;
	}
}
