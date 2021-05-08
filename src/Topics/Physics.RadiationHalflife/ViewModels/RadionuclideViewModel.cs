using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.UI.Localization;

namespace Physics.RadiationHalflife.ViewModels
{
	public class RadionuclideViewModel
	{
		public string ChemicalElement { get; set; }
		public string Name => Localizer.Instance[ChemicalElement + "_Name"];
		public float Halflife { get; set;  }
		public float Activity { get; set; }
		public RadionuclideViewModel(string chemicalElement, float halflife, float activity)
		{
			ChemicalElement = chemicalElement;
			Halflife = halflife;
			Activity = activity;
		}
	}
}
