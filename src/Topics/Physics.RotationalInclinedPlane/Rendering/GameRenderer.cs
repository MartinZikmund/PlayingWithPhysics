#nullable enable

using System;
using System.IO;
using System.Linq;
using Physics.RotationalInclinedPlane.Game;
using Physics.RotationalInclinedPlane.Logic;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;

namespace Physics.RotationalInclinedPlane.Rendering
{
	public class GameRenderer : ISkiaVariantRenderer
	{
		private SKBitmap _backgroundBitmap = null;
		private SKBitmap _objectBitmap = null;

		private readonly SKPaint _rampFillPaint = new SKPaint()
		{
			IsStroke = false,
			Color = SKColors.Gray,
		};

		private readonly SKPaint _rampStrokePaint = new SKPaint()
		{
			IsStroke = true,
			Color = SKColors.Black,
			IsAntialias = true,
			StrokeWidth = 10f
		};

		private readonly RotationalInclinedPlaneCanvasController _controller;

		public GameRenderer(RotationalInclinedPlaneCanvasController controller)
		{
			_controller = controller;
		}

		public GameInfo GameInfo { get; private set; }

		public void Dispose() { }

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (GameInfo is null)
			{
				return;
			}

			DrawBackground(sender, args);
			DrawInclinedPlane(sender, args);
			DrawObject(sender, args);
		}

		internal PhysicsService PhysicsService => _controller.PhysicsServices.FirstOrDefault();

		internal PhysicsService PreviewPhysicsService { get; private set; }

		internal void StartGame(GameInfo gameInfo) => GameInfo = gameInfo;

		public void Update(ISkiaCanvas sender)
		{
			EnsureBitmaps();
			if (GameInfo is null)
			{
				return;
			}

			if (PreviewPhysicsService is null ||
				PreviewPhysicsService.Setup.InclinedAngle != GameInfo.CurrentAngle)
			{
				PreviewPhysicsService = new PhysicsService(GameInfo.CreateGameMotionSetupWithAngle(GameInfo.CurrentAngle));
			}
		}

		private void DrawBackground(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.DrawBitmap(
				_backgroundBitmap,
				new SKRect(0, 0, _backgroundBitmap.Width, _backgroundBitmap.Height),
				new SKRect(0, 0, sender.ScaledSize.Width, sender.ScaledSize.Height));
		}
		
		private void DrawObject(ISkiaCanvas sender, SKSurface args)
		{
			var rampEndX = CalculateRampEndRenderX(sender);
			var metersToPixels = 100;
			var physicsService = PhysicsService ?? PreviewPhysicsService;
			var thisHorizontalWidth = physicsService.CalculateInclinedWidth();
			var startRenderX = rampEndX - thisHorizontalWidth * metersToPixels;
			var bottomRenderY = CalculateRampBottomY(sender);

			var t = GameInfo.State != GameState.SetAngle ? (float)_controller.SimulationTime.TotalTime.TotalSeconds : 0;
			var xScale = 10;
			var x = physicsService.CalculateX(t);
			var y = physicsService.CalculateY(t);
			var r = physicsService.Setup.Radius;

			float rad = MathHelpers.DegreesToRadians(physicsService.Setup.InclinedAngle);

			var dx = r * Math.Sin(rad);
			var dy = r * Math.Cos(rad);
			float cx = x + (float)dx;
			float cy = y + (float)dy;

			var renderX = startRenderX + cx * metersToPixels;
			var renderY = bottomRenderY - cy * metersToPixels;


			args.Canvas.DrawCircle(renderX, renderY, physicsService.Setup.Radius * metersToPixels, new SKPaint() { Color = SKColors.Red});

			//var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(physicsService.Setup.Color);
			////_boxPaint.Color = new SKColor(color.R, color.G, color.B, 180);
			////_boxStrokePaint.Color = new SKColor((byte)Math.Max(0, color.R - 60), (byte)Math.Max(0, color.G - 60), (byte)Math.Max(0, color.B - 60));

			float objectScale = 0.6f;

			args.Canvas.DrawBitmap(
				_objectBitmap,
				new SKRect(0, 0, _objectBitmap.Width, _objectBitmap.Height),
				new SKRect(renderX - _objectBitmap.Width * objectScale / 2, renderY - _objectBitmap.Height * objectScale / 2, renderX + _objectBitmap.Width * objectScale / 2, renderY + _objectBitmap.Height * objectScale / 2));
			//var radiusPx = _scalingRatio * motion.Radius;

			//float rad = MathHelpers.DegreesToRadians(PhysicsService.Setup.InclinedAngle);
			//var centerPoint = new SKPoint(PadX(imageInfo, x) + (float)Math.Cos(rad - Math.PI / 2) * radiusPx, FlipY(imageInfo, y) + (float)Math.Sin(rad - Math.PI / 2) * radiusPx);
			//int cx = (int)centerPoint.X;
			//int cy = (int)centerPoint.Y;
			//if (motion.BodyType == Logic.BodyType.FullCylinder || motion.BodyType == Logic.BodyType.Sphere)
			//{
			//	surface.Canvas.DrawCircle(cx, cy, radiusPx, _boxPaint);
			//}
			//if (motion.BodyType == Logic.BodyType.FullCylinder || motion.BodyType == Logic.BodyType.HollowCylinder)
			//{
			//	surface.Canvas.DrawCircle(cx, cy, radiusPx - 1, _boxStrokePaint);
			//}

			//var totalAngle = PhysicsService.CalculateTotalAngleInRad(t);
			//var rotationPoint = new SKPoint(PadX(imageInfo, centerPoint.X) + (float)Math.Cos(totalAngle - Math.PI / 2) * radiusPx, FlipY(imageInfo, centerPoint.Y) + (float)Math.Sin(totalAngle - Math.PI / 2) * radiusPx);
			//var point = RotatePointAroundCenter(centerPoint.X, centerPoint.Y, totalAngle, new SKPoint(PadX(imageInfo, x), FlipY(imageInfo, y)));
			//surface.Canvas.DrawLine(centerPoint, point, _boxStrokePaint);
		}

		private void DrawInclinedPlane(ISkiaCanvas sender, SKSurface args)
		{
			var currentAngle = GameInfo.CurrentAngle;

			var baseWidth = sender.ScaledSize.Width * 0.72f;
			var tan = Math.Tan(MathHelpers.DegreesToRadians(currentAngle));
			var height = (float)(baseWidth * tan);

			SKPath path = new SKPath();
			var topLeft = new SKPoint(-sender.ScaledSize.Width * 0.02f, CalculateRampBottomY(sender) - height);
			var bottomLeft = new SKPoint(-sender.ScaledSize.Width * 0.02f, CalculateRampBottomY(sender));
			var bottomRight = new SKPoint(CalculateRampEndRenderX(sender), bottomLeft.Y);

			path.MoveTo(topLeft);
			path.LineTo(bottomRight);
			path.LineTo(bottomLeft);
			path.LineTo(topLeft);
			path.Close();

			args.Canvas.DrawPath(path, _rampFillPaint);
			args.Canvas.DrawPath(path, _rampStrokePaint);
		}

		private void EnsureBitmaps()
		{
			if (_backgroundBitmap == null)
			{
				var gameAssetsPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Game");
				_backgroundBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Background.jpg"));
				_objectBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Object.png"));
			}
		}

		private float CalculateRampEndRenderX(ISkiaCanvas sender) => sender.ScaledSize.Width * 0.7f;

		private float CalculateRampBottomY(ISkiaCanvas sender) => sender.ScaledSize.Height * 0.94f;
	}
}
