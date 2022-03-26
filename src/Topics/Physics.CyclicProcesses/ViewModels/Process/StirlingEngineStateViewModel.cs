using System;
using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;

namespace Physics.CyclicProcesses.ViewModels.Process
{
	public class StirlingEngineStateViewModel : ProcessStateViewModel
	{
		private readonly StirlingEngineInputConfiguration _inputConfiguration;
		private readonly StirlingEnginePhysicsService _physicsService;

		public StirlingEngineStateViewModel(StirlingEngineInputConfiguration inputConfiguration) : base(inputConfiguration)
		{
			_physicsService = new StirlingEnginePhysicsService(inputConfiguration);
			_inputConfiguration = inputConfiguration;
		}

		public string P1InkPa => (_physicsService.P1 / 1000).ToString("0.#");

		public string P2InkPa => (_physicsService.P2 / 1000).ToString("0.#");

		public string P3InkPa => (_physicsService.P3 / 1000).ToString("0.#");

		public string P4InkPa => (_physicsService.P4 / 1000).ToString("0.#");

		public string QIn => (_physicsService.QIn).ToString("0.#");

		public string QOut => (_physicsService.QOut).ToString("0.#");

		public string W => (_physicsService.W).ToString("0.#");

		public string EffectiveEfficiencyInPercent => ((float)Math.Round(_physicsService.EffectiveEfficiency * 100, 1)).ToString("0.#");

		public string TermicEfficiencyInPercent => ((float)Math.Round(_physicsService.TermicEfficiency * 100, 1)).ToString("0.#");

		public string DeltaVInDm => ((_inputConfiguration.V2 - _inputConfiguration.V1) * 1000).ToString("0.#");
	}
}
