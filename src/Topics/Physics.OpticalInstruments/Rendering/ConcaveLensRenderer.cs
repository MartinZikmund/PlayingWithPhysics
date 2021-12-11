using Physics.OpticalInstruments.Logic;

namespace Physics.OpticalInstruments.Rendering
{
	public class ConcaveLensRenderer : LensRenderer
	{
		public ConcaveLensRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.5f;

		protected override InstrumentType InstrumentType => InstrumentType.ConcaveLens;
	}
}
