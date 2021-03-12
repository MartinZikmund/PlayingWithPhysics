using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.UI.Localization;

namespace Physics.RadiationHalflife.ViewModels
{
	public class NucleoidItemViewModel
	{
		private string ChemicalElement { get; set; }
		public string Name => Localizer.Instance[ChemicalElement + "_Name"];
		public Uri ChemicalNotionImage => new Uri($"ms-appx:///Assets/Elements/{ChemicalElement}.png");
		public float Halflife { get; }
		public string LocalizedHalflife => $"{Halflife} {Localizer.Instance[ChemicalElement + "_Halflife"]}";
		public NucleoidItemViewModel(string chemicalElement, float halflife)
		{
			ChemicalElement = chemicalElement;
			Halflife = halflife;
		}
	}
}
