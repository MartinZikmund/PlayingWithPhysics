using System;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.WaveInterference.Rendering
{
	public class GameController : SkiaCanvasController
	{
		public GameController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
		}

		public override void Update(ISkiaCanvas sender)
		{
		}
	}
}
