using Physics.OpticalInstruments.Logic;

namespace Physics.OpticalInstruments.Rendering
{
	public class ConvexLensRenderer : LensRenderer
	{
		public ConvexLensRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.5f;

		protected override InstrumentType InstrumentType => InstrumentType.ConvexLens;
	}
}
