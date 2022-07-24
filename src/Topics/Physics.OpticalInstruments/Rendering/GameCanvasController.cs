﻿using System.IO;
using Physics.OpticalInstruments.Game;
using Physics.Shared.Helpers;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;

namespace Physics.OpticalInstruments.Rendering
{
	public class GameCanvasController : SkiaCanvasController
	{
		private readonly SKPoint _mirrorCenterPoint = new SKPoint(714, 642);
		private readonly SKPoint _mirrorHitPoint = new SKPoint(714, 630);
		private readonly SKPoint _gunPoint = new SKPoint(714, 10);

		private SKBitmap _backgroundBitmap;
		private SKBitmap _backgroundCacheBitmap;
		private SKBitmap _mirrorBitmap;
		private SKBitmap _planetBitmap;
		private SKBitmap _backLegBitmap;

		private readonly SKPaint _laserFillPaint = new SKPaint()
		{
			Color = SKColors.Red,
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			StrokeWidth = 18,
			StrokeJoin = SKStrokeJoin.Bevel
		};

		private readonly SKPaint _laserStrokePaint = new SKPaint()
		{
			Color = SKColors.Black,
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			StrokeWidth = 20,
			StrokeJoin = SKStrokeJoin.Bevel
		};

		public GameCanvasController(ISkiaCanvas control) : base(control)
		{
		}

		public GameInfo GameInfo { get; internal set; }

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (GameInfo == null)
			{
				return;
			}
			DrawBackground(sender, args);
			DrawMirror(sender, args);
			DrawLaser(sender, args);
			DrawObject(sender, args);
		}

		public override void Update(ISkiaCanvas sender)
		{
			EnsureBitmaps();
			if (GameInfo == null)
			{
				return;
			}
		}

		private void DrawBackground(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.DrawBitmap(
				_backgroundBitmap,
				new SKRect(0, 0, _backgroundBitmap.Width, _backgroundBitmap.Height),
				new SKRect(0, 0, sender.ScaledSize.Width, sender.ScaledSize.Height));

			//args.Canvas.DrawBitmap(
			//	_backgroundCacheBitmap,
			//	new SKRect(0, 0, _backgroundCacheBitmap.Width, _backgroundCacheBitmap.Height),
			//	new SKRect(0, 0, sender.ScaledSize.Width, sender.ScaledSize.Height));

			var legLeft = 640;
			var legTop = 560;
			args.Canvas.DrawBitmap(
				_backLegBitmap,
				new SKRect(0, 0, _backLegBitmap.Width, _backLegBitmap.Height),
				new SKRect(legLeft, legTop, legLeft + _backLegBitmap.Width, legTop + _backLegBitmap.Height));
		}

		private void DrawMirror(ISkiaCanvas sender, SKSurface args)
		{
			using var image = SkiaHelpers.RotateBitmap(_mirrorBitmap, 90 - GameInfo.CurrentAngle);
			int left = 714;
			int top = 642;
			args.Canvas.DrawBitmap(
				image,
				new SKRect(0, 0, image.Width, image.Height),
				new SKRect(left - image.Width / 2, top - image.Height / 2, left + image.Width / 2, top + image.Height / 2));
		}

		private void DrawObject(ISkiaCanvas sender, SKSurface args)
		{
			//GameInfo.MockAngle();
			var targetAngle = GameInfo.TargetAngle;

			if (_planetPointAngle != targetAngle)
			{
				var targetX = GameInfo.ObjectX;

				var targetPointDirection = SkiaHelpers.RotatePoint(_gunPoint, _mirrorHitPoint, MathHelpers.DegreesToRadians((90 - (float)targetAngle) * 2));

				var lineToTarget = new Line2d(new Point2d(_mirrorHitPoint.X, _mirrorHitPoint.Y), new Point2d(targetPointDirection.X, targetPointDirection.Y));
				var lineX = new Line2d(new Point2d(targetX, 0), new Point2d(targetX, 1000));
				_planetPoint = lineToTarget.IntersectWith(lineX).Value;
				_planetPointAngle = (int)targetAngle;
			}

			args.Canvas.DrawBitmap(
				_planetBitmap,
				new SKRect(0, 0, _planetBitmap.Width, _planetBitmap.Height),
				new SKRect(
					(float)_planetPoint.X - _planetBitmap.Width / 2,
					(float)_planetPoint.Y - _planetBitmap.Height / 2,
					(float)_planetPoint.X + _planetBitmap.Width / 2,
					(float)_planetPoint.Y + _planetBitmap.Height / 2));
		}

		private int _planetPointAngle = -1;
		private Point2d _planetPoint = default;

		private void DrawLaser(ISkiaCanvas sender, SKSurface args)
		{
			if (GameInfo.State == GameState.SetAngle)
			{
				return;
			}

			// Down laser
			using var laserPath = new SKPath();
			laserPath.MoveTo(_gunPoint);
			laserPath.LineTo(_mirrorHitPoint);
			var targetPoint = SkiaHelpers.RotatePoint(_gunPoint, _mirrorHitPoint, MathHelpers.DegreesToRadians((90 - GameInfo.CurrentAngle) * 2));
			laserPath.LineTo(targetPoint);
			args.Canvas.DrawPath(laserPath, _laserStrokePaint);
			args.Canvas.DrawPath(laserPath, _laserFillPaint);

		}

		private void EnsureBitmaps()
		{
			if (_backgroundBitmap == null)
			{
				var gameAssetsPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Game");
				_backgroundBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Background.png"));
				_backgroundCacheBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "BackgroundCache.png"));
				_backLegBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "BackLeg.png"));
				_mirrorBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Mirror.png"));
				_planetBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Planet.png"));
			}
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
