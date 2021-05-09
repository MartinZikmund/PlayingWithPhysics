using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class BeamActivityPhysicsService : PhysicsService
	{
		public BeamActivityAnimationInfo Animation { get; set; }
		private float _currentTime = 0;
		public BeamActivityPhysicsService(AnimationInfo animation)
		{
			Animation = (BeamActivityAnimationInfo)animation;
			FillTable();
		}

		public List<(float X, float Y)> FillTable()
		{
			List<(float X, float Y)> _values = new List<(float X, float Y)>();
			float currentTime = 0f;
			while (currentTime < Animation.Halflife * 6)
			{
				float a = ComputeA(currentTime);
				_values.Add((currentTime, a));
				currentTime += Animation.Delta;
			}
			return _values;
		}

		public float ComputeA(float time)
		{
			return Animation.Activity * (float)Math.Pow(0.5d, time / Animation.Halflife); 
		}
	}
}
