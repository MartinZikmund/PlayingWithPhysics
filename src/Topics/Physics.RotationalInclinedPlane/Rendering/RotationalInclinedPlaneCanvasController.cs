using System;
using Physics.RotationalInclinedPlane.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RotationalInclinedPlane.Rendering
{
	public class RotationalInclinedPlaneCanvasController : SkiaCanvasController
	{
		public RotationalInclinedPlaneCanvasController(ISkiaCanvas canvasAnimatedControl) :
			base(canvasAnimatedControl)
		{
		}

		public override void Dispose()
		{
			base.Dispose();
			Renderer?.Dispose();
		}

		public MotionSetup Motion { get; private set; }

		public PhysicsService PhysicsService { get; private set; }

		public ISkiaVariantRenderer Renderer { get; set; }

		public void StartSimulation(MotionSetup motion)
		{
			if (motion is null)
			{
				throw new ArgumentNullException(nameof(motion));
			}

			Motion = motion;
			PhysicsService = new PhysicsService(motion);
			SimulationTime.Restart();
		}

		public void SetVariantRenderer(ISkiaVariantRenderer renderer)
		{
			Renderer = renderer;
		}

		public override void Update(ISkiaCanvas sender)
		{
			Renderer?.Update(sender);
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			Renderer?.Draw(sender, args);
		}
	}
}
