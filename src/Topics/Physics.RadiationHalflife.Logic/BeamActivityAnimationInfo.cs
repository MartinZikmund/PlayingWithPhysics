using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class BeamActivityAnimationInfo : AnimationInfo
	{
		public float Activity { get; set; }
		public float Mantissa { get; set; }
		public BeamActivityAnimationInfo(string chemicalElement, float halflife, float activity, float mantissa = 0.0f) : base(chemicalElement, halflife)
		{
			Activity = activity;
			Mantissa = mantissa;
		}
	}
}
