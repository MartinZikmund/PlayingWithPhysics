using System.Xml.Schema;
using MvvmCross.ViewModels;

namespace Physics.OpticalInstruments.Logic
{
	public class SceneConfiguration : MvxNotifyPropertyChanged
	{
		public float FocalDistance { get; set; } = 2.5f;

		public float FocalDistanceCm
		{
			get => FocalDistance * 100;
			set => FocalDistance = value / 100;
		}

		public string DioptersString => (1 / FocalDistance).ToString("0.##");

		public float ObjectHeight { get; set; } = 1.5f;

		public float ObjectHeightCm { get => ObjectHeight * 100; set => ObjectHeight = value / 100; }

		public float ObjectDistance { get; set; } = 5f;

		public float ObjectDistanceCm { get => ObjectDistance * 100; set => ObjectDistance = value / 100; }
	}
}
