using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class BeamActivityPhysicsService : PhysicsService
	{
		public BeamActivityAnimationInfo Animation { get; set; }
		public List<(float X, int Y)> ValuesTable { get; set; }
		private float _currentTime = 0;
		public BeamActivityPhysicsService(AnimationInfo animation)
		{
			ValuesTable = new List<(float X, int Y)>();
			Animation = (BeamActivityAnimationInfo)animation;
		}
	}
}
