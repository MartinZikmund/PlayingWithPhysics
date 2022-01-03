using System;

namespace Physics.StationaryWaves.Logic
{
	public class WaveTrajectory : IWaveTrajectory
	{
		private readonly WaveInfo _waveInfo;
		private readonly IWavePhysicsService _wavePhysicsService;
		private bool _first = true;

		public WaveTrajectory(IWavePhysicsService physicsService, WaveInfo wave, bool first)
		{
			if (wave is null)
			{
				throw new ArgumentNullException(nameof(wave));
			}

			_waveInfo = wave;
			_wavePhysicsService = physicsService;
			_first = first;
		}

		public float? GetY(float x, float time) => _first ? _wavePhysicsService.CalculateFirstWaveY(x, time) : _wavePhysicsService.CalculateSecondWaveY(x, time);
	}
}
