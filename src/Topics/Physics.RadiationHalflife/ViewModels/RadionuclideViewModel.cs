using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.UI.Localization;
using Windows.UI.Xaml;

namespace Physics.RadiationHalflife.ViewModels
{
	public class RadionuclideViewModel
	{
		public string ChemicalElement { get; set; }
		public string Name => Localizer.Instance[ChemicalElement + "_Name"];
		public float Halflife { get; set; }
		public float ActivityBase { get; set; }
		public int ActivityMantissa { get; set; }
		public Visibility HasMantissa => ActivityMantissa > 0 ? Visibility.Visible : Visibility.Collapsed;
		public RadionuclideViewModel(string chemicalElement, float halflife, float activityBase, int activityMantissa = 0)
		{
			ChemicalElement = chemicalElement;
			Halflife = halflife;
			ActivityBase = activityBase;
			ActivityMantissa = activityMantissa;
		}
	}
}
