using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.RadiationHalflife.Logic
{
	public class AnimationInfo
	{
		public string ChemicalElement { get; set; }
		public float Halflife { get; set; }
		public float Delta { get; set; }

		public AnimationInfo(string chemicalElement, float halflife)
		{
			ChemicalElement = chemicalElement;
			Halflife = halflife;
			Delta = Halflife/300;
		}
	}
}
