using System;
using System.Linq;

namespace Physics.StationaryWaves.Logic
{
	public class WaveInterferenceTrajectory : IWaveTrajectory
	{
		private readonly IWaveTrajectory[] _singleTrajectories;

		public WaveInterferenceTrajectory(params IWaveTrajectory[] singleTrajectories)
		{
			_singleTrajectories = singleTrajectories;
		}

		public float? GetY(float x, float time)
		{
			var first = _singleTrajectories[0].GetY(x, time);
			var second = _singleTrajectories[1].GetY(x, time);
			if(first == null || second == null)
			{
				return null;
			}
			return first.Value + second.Value;
		}
	}
}
