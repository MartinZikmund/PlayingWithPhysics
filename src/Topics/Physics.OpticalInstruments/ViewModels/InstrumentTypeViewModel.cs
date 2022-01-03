using System;
using Physics.OpticalInstruments.Logic;
using Physics.Shared.UI.Localization;

namespace Physics.OpticalInstruments.ViewModels
{
	public class InstrumentTypeViewModel
	{
		public InstrumentTypeViewModel(InstrumentType type) => Type = type;

		public InstrumentType Type { get; }

		public string Name => Localizer.Instance.GetString(Type.ToString("g"));

		public Uri Image => new Uri($"ms-appx:///Assets/Icons/{Type:g}.png");

		public float DefaultFocalDistance => Type == InstrumentType.ConvexLens || Type == InstrumentType.ConcaveMirror ? 250f : -250f;

		public float MinFocalDistanceCm => Type == InstrumentType.ConvexLens || Type == InstrumentType.ConcaveMirror ? 5f : -500f;

		public float MaxFocalDistanceCm => Type == InstrumentType.ConvexLens || Type == InstrumentType.ConcaveMirror ? 500f : -5f;
	}
}
