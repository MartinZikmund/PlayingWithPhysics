using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class AnimationInfo
	{
		public int ParticleCount { get; set; }
		public float Halflife { get; set; }
		public float Delta { get; set; }

		public AnimationInfo(int particleCount, float halflife)
		{
			ParticleCount = particleCount;
			Halflife = halflife;
			Delta = Halflife/300;
		}
	}
}
