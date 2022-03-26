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

		public string T1 => (_physicsService.T1).ToString("0.#");

		public string T2 => (_physicsService.T2).ToString("0.#");

		public string W12 => (_physicsService.W12).ToString("0.#");

		public string Q12 => (_physicsService.Q12).ToString("0.#");

		public string DeltaVInDm => ((_inputConfiguration.V2 - _inputConfiguration.V1) * 1000).ToString("0.#");
	}
}
