using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
    public class PhysicsService
    {
		public AnimationInfo Animation { get; set; }
		public List<(float X, int Y)> ValuesTable { get; set; }
		private float _currentTime = 0;

		public PhysicsService(AnimationInfo animation)
		{
			Animation = animation;
			ValuesTable = new List<(float X, int Y)>();
			//FillTable();
		}

		public List<(float X, float Y)> FillTablePrecise()
		{
			List<(float X, float Y)> _preciseValues = new List<(float X, float Y)>();
			float particles = Animation.ParticleCount;
			while (_currentTime <= Animation.Halflife * 6)
			{
				particles = ComputeNPrecise(_currentTime);
				_preciseValues.Add((_currentTime, particles));
				_currentTime += Animation.Delta;
			}

			return _preciseValues;
		}

		public void FillTable()
		{
			int particles = Animation.ParticleCount;
			while (particles > 0)
			{
				particles = ComputeN(_currentTime);
				ValuesTable.Add((_currentTime, particles));
				_currentTime += Animation.Delta;
			}
		}

		public int ComputeN(float time)
		{
			return (int)Math.Round(Animation.ParticleCount * Math.Exp(-Math.Log(2)*time / Animation.Halflife));
		}

		private float ComputeNPrecise(float time)
		{
			return Animation.ParticleCount * (float)Math.Exp(-Math.Log(2) * time / Animation.Halflife);
		}
    }
}
