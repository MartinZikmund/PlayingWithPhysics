namespace Physics.OpticalInstruments.Rendering
{
	public class ConcaveMirrorRenderer : MirrorRenderer
	{
		public ConcaveMirrorRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.5f;
	}
}
