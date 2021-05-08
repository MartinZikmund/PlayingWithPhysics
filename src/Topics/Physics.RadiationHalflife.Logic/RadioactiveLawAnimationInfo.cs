using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class RadioactiveLawAnimationInfo : AnimationInfo
	{
		public string CustomHalflifeUnit { get; set; }
		public int ParticleCount { get; set; }

		public RadioactiveLawAnimationInfo(string chemicalElement, float halflife, int particleCount, string customHalflifeUnit = "") : base(chemicalElement, halflife)
		{
			CustomHalflifeUnit = customHalflifeUnit;
			ParticleCount = particleCount;
		}
	}
}
