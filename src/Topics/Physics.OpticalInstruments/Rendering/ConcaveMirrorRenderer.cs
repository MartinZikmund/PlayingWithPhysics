using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public class ConcaveMirrorRenderer : MirrorRenderer
	{
		public ConcaveMirrorRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.95f;

		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawAxisPoint(args, -SceneConfiguration.FocalDistance, "F");
			DrawAxisPoint(args, -SceneConfiguration.FocalDistance * 2, "C");
			DrawMirror(sender, args);
		}

		protected override void DrawMirror(ISkiaCanvas canvas, SKSurface surface)
		{
			var centerX = GetRenderX(-SceneConfiguration.FocalDistance * 2);
			var centerY = GetRenderY(0);
			var radius = MirrorRadius * PixelsPerMeter;
			var bounds = new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius);

			using var path = new SKPath();
			path.AddArc(bounds, -30, 60);
			surface.Canvas.DrawPath(path, _axisStrokePaint);
		}
	}
}
