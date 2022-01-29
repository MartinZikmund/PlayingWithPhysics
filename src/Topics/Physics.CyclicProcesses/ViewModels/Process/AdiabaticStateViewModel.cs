using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;

namespace Physics.CyclicProcesses.ViewModels.Process
{
	public class AdiabaticStateViewModel : ProcessStateViewModel
	{
		private readonly AdiabaticInputConfiguration _inputConfiguration;
		private readonly AdiabaticPhysicsService _physicsService;

		public AdiabaticStateViewModel(AdiabaticInputConfiguration inputConfiguration) : base(inputConfiguration)
		{
			_physicsService = new AdiabaticPhysicsService(inputConfiguration);
			_inputConfiguration = inputConfiguration;
		}

		public float T1 => _physicsService.T1;

		public float T2 => _physicsService.T2;

		public float W12 => _physicsService.W12;

		public float DeltaU12 => _physicsService.U12;

		public float DeltaVInDm => (_inputConfiguration.V2 - _inputConfiguration.V1) * 1000;
	}
}
