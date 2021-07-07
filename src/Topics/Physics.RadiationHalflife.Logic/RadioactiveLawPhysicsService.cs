using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class RadioactiveLawPhysicsService : PhysicsService
	{
		public RadioactiveLawAnimationInfo Animation { get; set; }
		public List<(float X, int Y)> ValuesTable { get; set; }
		private float _currentTime = 0;

		public RadioactiveLawPhysicsService(AnimationInfo animation)
		{
			ValuesTable = new List<(float X, int Y)>();
			Animation = (RadioactiveLawAnimationInfo)animation;
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
			float currentTime = 0;
			int particles = Animation.ParticleCount;
			while (particles > 0)
			{
				particles = ComputeN(currentTime);
				ValuesTable.Add((currentTime, particles));
				currentTime += Animation.Delta;
			}
		}

		public int ComputeN(float time)
		{
			return (int)Math.Round(Animation.ParticleCount * Math.Exp(-Math.Log(2) * time / Animation.Halflife));
		}

		private float ComputeNPrecise(float time)
		{
			return Animation.ParticleCount * (float)Math.Exp(-Math.Log(2) * time / Animation.Halflife);
		}
	}
}
