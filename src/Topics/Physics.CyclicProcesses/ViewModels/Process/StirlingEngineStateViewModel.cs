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

		public float P1InKPa => _physicsService.P1 / 1000;

		public float P2InKPa => _physicsService.P2 / 1000;

		public float P3InKPa => _physicsService.P3 / 1000;

		public float P4InKPa => _physicsService.P4 / 1000;

		public float QIn => _physicsService.QIn;

		public float QOut => _physicsService.QOut;

		public float W => _physicsService.W;

		public float EffectiveEfficiencyInPercent => _physicsService.EffectiveEfficiency * 100;

		public float TermicEfficiencyInPercent => _physicsService.TermicEfficiency * 100;

		public float DeltaVInDm => (_inputConfiguration.V2 - _inputConfiguration.V1) * 1000;
	}
}
