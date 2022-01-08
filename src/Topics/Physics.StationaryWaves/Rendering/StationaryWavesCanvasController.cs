using System;
using System.Collections.Generic;
using Physics.Shared.UI.Rendering.Skia;
using Physics.StationaryWaves.Logic;
using Physics.StationaryWaves.ViewModels;
using SkiaSharp;

namespace Physics.StationaryWaves.Rendering
{
	public class StationaryWavesCanvasController : SkiaCanvasController
	{
		public StationaryWavesCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public ISkiaCanvas Canvas => _canvas;

		public StationaryWavesRenderer Renderer { get; private set; }

		public DisplaySettingsViewModel DisplaySettings { get; private set; }

		public void SetVariantRenderer(StationaryWavesRenderer renderer) => Renderer = renderer;

		public override void Draw(ISkiaCanvas sender, SKSurface args) => Renderer?.Draw(sender, args);

		public override void Update(ISkiaCanvas sender) => Renderer?.Update(sender);
		
		public void StartSimulation(BounceType bounceType, float width)
		{
			Renderer?.StartSimulation(bounceType, width);
			SimulationTime.Restart();
			Play();
		}

		internal void SetDisplaySettings(DisplaySettingsViewModel displaySettings) => DisplaySettings = displaySettings;
	}
}
