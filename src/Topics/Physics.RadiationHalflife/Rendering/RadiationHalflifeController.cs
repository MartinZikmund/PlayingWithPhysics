using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RadiationHalflife.Rendering
{
	public class RadiationHalflifeController : SkiaCanvasController
	{
		IVariantRenderer _renderer;

		public RadiationHalflifeController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			_renderer?.Draw(sender, args);
		}


		public void SetRenderer(IVariantRenderer renderer)
		{
			_renderer = renderer;
		}

		public void SetAnimation(PhysicsService physicsService)
		{
			_renderer.SetAnimation(physicsService);
		}

		public void StartSimulation()
		{
			_renderer.StartSimulation();
		}

		public override void Update(ISkiaCanvas sender)
		{
			_renderer?.Update(sender);
		}
	}
}
