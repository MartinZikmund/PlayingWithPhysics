using System.Linq;

namespace Physics.CompoundOscillations.Logic
{
	public class CompoundOscillationTrajectory : IOscillationTrajectory
	{
		private readonly IOscillationTrajectory[] _singleTrajectories;

		public CompoundOscillationTrajectory(IOscillationTrajectory[] singleTrajectories)
		{
			_singleTrajectories = singleTrajectories;
		}

		public float GetY(float timeInSeconds) => _singleTrajectories.Sum(t => t.GetY(timeInSeconds));
	}
}
