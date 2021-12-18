using System;
using System.Collections.Generic;
using Physics.Shared.UI.Rendering.Skia;
using Physics.StationaryWaves.Logic;
using SkiaSharp;

namespace Physics.StationaryWaves.Rendering
{
	public class StationaryWavesCanvasController : SkiaCanvasController
	{
		public StationaryWavesCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public StationaryWavesRenderer Renderer { get; private set; }

		public void SetVariantRenderer(StationaryWavesRenderer renderer) => Renderer = renderer;

		public override void Draw(ISkiaCanvas sender, SKSurface args) => Renderer?.Draw(sender, args);

		public override void Update(ISkiaCanvas sender) => Renderer?.Update(sender);
		
		public abstract void StartSimulation(BounceType bounceType, float width)
		{
			SimulationTime.Restart();
			Play();
		}		
	}
}
