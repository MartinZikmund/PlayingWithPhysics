using Physics.StationaryWaves.Logic;

namespace Physics.StationaryWaves.Rendering
{
	public class EasyWavesRenderer : StationaryWavesRenderer
    {
		private IWavePhysicsService _wavePhysicsService;

		public EasyWavesRenderer(StationaryWavesCanvasController controller) : base(controller)
		{
		}

		protected override IWavePhysicsService WavePhysicsService => _wavePhysicsService;

		internal override void StartSimulation(BounceType bounceType, float width)
		{
			_wavePhysicsService = new EasyWavePhysicsService(bounceType, width);
		}
	}
}
