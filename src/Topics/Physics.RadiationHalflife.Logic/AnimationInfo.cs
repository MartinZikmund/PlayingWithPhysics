using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class AnimationInfo
	{
		public string ChemicalElement { get; set; }
		public int ParticleCount { get; set; }
		public float Halflife { get; set; }
		public float Delta { get; set; }
		public string CustomHalflifeUnit { get; set; }

		public AnimationInfo(string chemicalElement, int particleCount, float halflife, string customHalflifeUnit = "")
		{
			ChemicalElement = chemicalElement;
			ParticleCount = particleCount;
			Halflife = halflife;
			Delta = Halflife/300;
			CustomHalflifeUnit = customHalflifeUnit;
		}
	}
}
