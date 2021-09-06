using System;
using System.Linq;

namespace Physics.WaveInterference.Logic
{
	public class WaveInterferenceTrajectory : IWaveTrajectory
	{
		private readonly IWaveTrajectory[] _singleTrajectories;

		public WaveInterferenceTrajectory(IWaveTrajectory[] singleTrajectories)
		{
			_singleTrajectories = singleTrajectories;
		}

		public float? GetY(float x, float time, bool accurate = false)
		{
			var first = _singleTrajectories[0].GetY(x, time);
			var second = _singleTrajectories[1].GetY(x, time);
			if(first == null || second == null)
			{
				return null;
			}
			return first.Value + second.Value;
		}

		public float StartX => Math.Max(_singleTrajectories[0].StartX, _singleTrajectories[1].StartX);

		public float EndX => Math.Min(_singleTrajectories[0].EndX, _singleTrajectories[1].EndX);
	}
}
