using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.ViewModels;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.ViewModels;
using Physics.StationaryWaves.Logic;
using Windows.UI.Xaml.Controls;

namespace Physics.StationaryWaves.ViewModels
{
	public class AddOrUpdateWaveViewModel : ViewModelBase
	{
		public DifficultyOption Difficulty { get; }
		public AddOrUpdateWaveViewModel(DifficultyOption difficulty)
		{
			Difficulty = difficulty;
			SelectedAVariantIndex = 0;
		}

		public float Amplitude { get; set; } = 1;
		public List<AVariant> AVariants { get; } = new List<AVariant>()
		{
			AVariant.Zero,
			AVariant.Pi
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

		public ICommand SaveCommand => GetOrCreateCommand<ContentDialogButtonClickEventArgs>(Save);

		public void Save(ContentDialogButtonClickEventArgs args)
		{
			if (Difficulty == DifficultyOption.Easy)
			{
				ResultWaveInfo = new WaveInfo(Localizer.Instance["Wave"], SelectedBouncingPoint);
			}
			else
			{
				ResultWaveInfo = new WaveInfo(Localizer.Instance["Wave"], SelectedBouncingPoint, Amplitude, SelectedAVariant, SelectedRange, "#FF0000");
			}
		}

		public WaveInfo ResultWaveInfo { get; private set; }

		public int SelectedBouncingPointIndex { get; set; }
		public BouncingPoints SelectedBouncingPoint
		{
			get
			{
				switch (SelectedBouncingPointIndex)
				{
					case 1:
						Debug.WriteLine("Rigid");
						return BouncingPoints.RigidEnd;
					case 0:
					default:
						Debug.WriteLine("Free");
						return BouncingPoints.FreeEnd;
				}
			}
		}
		public bool IsEasyOption => Difficulty == DifficultyOption.Easy;

		public bool IsAdvancedOption => !IsEasyOption;
	}
}
