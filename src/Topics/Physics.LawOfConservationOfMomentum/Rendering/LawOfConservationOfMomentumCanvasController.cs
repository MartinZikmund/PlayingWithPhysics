using System;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.LawOfConservationOfMomentum.Rendering
{
	public class LawOfConservationOfMomentumCanvasController : SkiaCanvasController
	{
		public const float HorizontalPadding = 20f;
		public const float ObjectSize = 12f;

		private readonly SKPaint _firstObjectPaint = new SKPaint()
		{
			IsStroke = false,
			Color = SKColors.Blue,
			IsAntialias = true,
		};

		private readonly SKPaint _secondObjectPaint = new SKPaint()
		{
			IsStroke = false,
			Color = SKColors.Red,
			IsAntialias = true,
		};


		private readonly SKPaint _wallPaint = new SKPaint()
		{
			IsStroke = true,
			StrokeWidth = ObjectSize,
			Color = SKColors.Red,
			IsAntialias = true,
		};

		private float _metersToPixels = 0;


		public MotionSetup Motion { get; private set; }

		public PhysicsService PhysicsService { get; private set; }

		public LawOfConservationOfMomentumCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public void StartSimulation(MotionSetup motion)
		{
			if (motion is null)
			{
				throw new ArgumentNullException(nameof(motion));
			}

			Motion = motion;
			PhysicsService = new PhysicsService(Motion);

			SimulationTime.Restart();
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (Motion == null)
			{
				return;
			}

			var firstX = PhysicsService.GetX1((float)SimulationTime.TotalTime.TotalSeconds);
			DrawObject(sender, args, firstX, _firstObjectPaint, 0);
			var secondX = PhysicsService.GetX2((float)SimulationTime.TotalTime.TotalSeconds);
			if (Motion.Subtype == CollisionSubtype.V2ZeroM2BiggerThanM1)
			{
				DrawWall(sender, args, secondX);
			}
			else
			{
				DrawObject(sender, args, secondX, _secondObjectPaint, 1);
			}
		}

		public override void Update(ISkiaCanvas sender)
		{
			if (Motion == null)
			{
				return;
			}

			var displayWidth = PhysicsService.GetDisplayWidth();
			_metersToPixels = (sender.ScaledSize.Width - HorizontalPadding * 2) / displayWidth;
		}

		private void DrawObject(ISkiaCanvas sender, SKSurface args, float x, SKPaint paint, int objectIndex)
		{
			var renderX = PadX(x * _metersToPixels);

			args.Canvas.DrawCircle(objectIndex == 0 ? renderX - ObjectSize : renderX + ObjectSize, sender.ScaledSize.Height / 2, ObjectSize, paint);
		}

		private void DrawWall(ISkiaCanvas sender, SKSurface args, float x)
		{
			var renderX = PadX(x * _metersToPixels);

			args.Canvas.DrawLine(renderX + ObjectSize / 2, 0, renderX + ObjectSize / 2, sender.ScaledSize.Height, _wallPaint);
		}

		public float PadX(float x) => HorizontalPadding + x;
	}
}
