using System;
using System.Linq;
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

		public MotionSetup[] Motions { get; private set; } = Array.Empty<MotionSetup>();

		public PhysicsService[] PhysicsServices { get; private set; } = Array.Empty<PhysicsService>();

		public ISkiaVariantRenderer Renderer { get; set; }

		public void StartSimulation(MotionSetup[] motions)
		{
			if (motions is null)
			{
				throw new ArgumentNullException(nameof(motions));
			}

			Motions = motions;
			PhysicsServices = motions.Select(m => new PhysicsService(m)).ToArray();
			SimulationTime.Restart();
		}

		public void SetVariantRenderer(ISkiaVariantRenderer renderer) => Renderer = renderer;

		public override void Update(ISkiaCanvas sender) => Renderer?.Update(sender);

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			Renderer?.Draw(sender, args);
		}
	}
}
