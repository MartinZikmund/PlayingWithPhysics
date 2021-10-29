using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.StationaryWaves.Logic;

namespace Physics.StationaryWaves.ViewModels
{
	public class AddOrUpdateWaveViewModel : MvxNotifyPropertyChanged
	{
		public AddOrUpdateWaveViewModel(DifficultyOption difficulty)
		{
			SelectedAVariantIndex = 0;
		}

		public float Amplitude { get; set; } = 1;
		public List<AVariant> AVariants { get; } = new List<AVariant>()
		{
			AVariant.Zero,
			AVariant.Quarter
		};

		public List<Range> Ranges { get; } = new List<Range>()
		{
			new Range(0, 0.5f),
			new Range(0f, 1f),
			new Range(0f, 1.5f),
			new Range(0f, 2f),
			new Range(0f, 6f)
		};

		public int SelectedRangeIndex { get; set; }

		public Range SelectedRange => Ranges[SelectedRangeIndex];

		public int SelectedAVariantIndex { get; set; }
		public AVariant SelectedAVariant => (AVariant)SelectedAVariantIndex;

		public string DialogTitle { get; set; } = "Edit Wave";
	}
}
