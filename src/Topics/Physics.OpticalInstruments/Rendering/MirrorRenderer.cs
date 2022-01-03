using System;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public abstract class MirrorRenderer : OpticalInstrumentsRenderer
	{
		protected MirrorRenderer(OpticalInstrumentsCanvasController controller) : base(controller)
		{
		}

		protected readonly SKPaint _mirrorShadowPaint = new SKPaint()
		{
			Color = new SKColor(0,0,0,90),
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			StrokeWidth = 10,
		};

		protected float MirrorRadius => Math.Abs(SceneConfiguration.FocalDistance) * 2;

		protected abstract void DrawMirror(ISkiaCanvas canvas, SKSurface surface);
	}
}
