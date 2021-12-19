using Physics.StationaryWaves.Logic;

namespace Physics.StationaryWaves.Rendering
{
	public class AdvancedWavesRenderer : StationaryWavesRenderer
	{
		private IWavePhysicsService _wavePhysicsService;

		public AdvancedWavesRenderer(StationaryWavesCanvasController controller) : base(controller)
		{
		}

		protected override IWavePhysicsService WavePhysicsService => _wavePhysicsService;

		internal override void StartSimulation(BounceType bounceType, float width)
		{
			_wavePhysicsService = new AdvancedWavePhysicsService(bounceType);
		}
	}
}
