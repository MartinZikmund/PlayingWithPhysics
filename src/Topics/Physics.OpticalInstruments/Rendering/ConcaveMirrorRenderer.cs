using Physics.OpticalInstruments.Logic;
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

		protected override float RelativeOpticalInstrumentX => 0.75f;

		protected override InstrumentType InstrumentType => InstrumentType.ConcaveMirror;

		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawAxisPoint(args, -SceneConfiguration.FocalDistance, "F");
			DrawAxisPoint(args, -SceneConfiguration.FocalDistance * 2, "C");
			DrawMirror(sender, args);

			DrawLightBeams(sender, args);
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

		private void DrawLightBeams(ISkiaCanvas canvas, SKSurface surface)
		{
			var fromX = GetRenderX(SceneConfiguration.ObjectDistance);
			var fromY = GetRenderY(SceneConfiguration.ObjectHeight);
			var toX = GetRenderX(ImageInfo.ImageDistance);
			var toY = GetRenderY(ImageInfo.ImageHeight);

			surface.Canvas.DrawLine(fromX, fromY, toX, toY, _lightBeamPaint);
		}
	}
}
