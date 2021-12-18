using System;
using Physics.OpticalInstruments.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public class OpticalInstrumentsCanvasController : SkiaCanvasController
	{
		public OpticalInstrumentsCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public OpticalInstrumentsRenderer Renderer { get; private set; }

		public SceneConfiguration SceneConfiguration { get; set; }

		public void SetVariantRenderer(OpticalInstrumentsRenderer renderer) => Renderer = renderer;

		public override void Draw(ISkiaCanvas sender, SKSurface args) => Renderer?.Draw(sender, args);

		public override void Update(ISkiaCanvas sender) => Renderer?.Update(sender);

		internal bool TryGetObjectPosition(SKPoint pointerPoint, out SKPoint objectPoint)
		{
			if (Renderer == null)
			{
				objectPoint = SKPoint.Empty;
				return false;
			}

			return Renderer.TryGetObjectPosition(pointerPoint, out objectPoint);
		}
	}
}
