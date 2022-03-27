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

		public string T1 => (_physicsService.T1).ToString("0.#");

		public string T2 => (_physicsService.T2).ToString("0.#");

		public string W12 => (_physicsService.W12).ToString("0.#");

		public string DeltaU12 => (_physicsService.U12).ToString("0.#");

		public string DeltaVInDm => ((_inputConfiguration.V2 - _inputConfiguration.V1) * 1000).ToString("0.#");
	}
}
