using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.UI.Localization;
using Windows.UI.Xaml;

namespace Physics.RadiationHalflife.ViewModels
{
	public class NucleoidItemViewModel
	{
		public string ChemicalElement { get; set; }
		public string Name => Localizer.Instance[ChemicalElement + "_Name"];
		public Uri ChemicalNotionImage => new Uri($"ms-appx:///Assets/Elements/{ChemicalElement}.png");
		public float Halflife { get; }
		public string LocalizedHalflife => $"{Halflife} {Localizer.Instance[ChemicalElement + "_Halflife"]}";
		public Visibility CustomElementListVisibility { get; set; } = Visibility.Visible;
		public NucleoidItemViewModel(string chemicalElement, float halflife)
		{
			if (chemicalElement == "Custom")
			{
				CustomElementListVisibility = Visibility.Collapsed;
			}
			else
			{
				CustomElementListVisibility = Visibility.Visible;
				Halflife = halflife;
			}
			ChemicalElement = chemicalElement;
		}
	}
}
