using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Physics.ElectricParticle.Logic;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.ElectricParticle.Rendering
{
	public class ElectricParticleCanvasController : SkiaCanvasController
	{
		private const string PlusSign = "+";
		private const string MinusSign = "−";

		private ElectricParticleSimulationSetup _setup;
		private PhysicsService _service;

		private float _unitDimension;

		private float _unitToPixel;
		private float _pixelToUnit;

		private BigNumber _deltaT;

		private readonly SKPaint _minusPlanePaint = new SKPaint()
		{
			Color = new SKColor(173, 26, 0),
			StrokeWidth = 5,
			StrokeCap = SKStrokeCap.Round,
			IsStroke = true
		};

		private readonly SKPaint _plusPlanePaint = new SKPaint()
		{
			Color = new SKColor(0, 75, 153),
			StrokeWidth = 5,
			StrokeCap = SKStrokeCap.Round,
			IsStroke = true
		};

		private readonly SKPaint _particlePaint = new SKPaint()
		{
			Color = SKColors.Black,
			IsStroke = false,
			IsAntialias = true
		};

		private readonly SKPaint _particleText = new SKPaint()
		{
			Color = SKColors.White,
			TextSize = 24,
			TextAlign = SKTextAlign.Center,
			IsAntialias = true
		};

		private readonly SKTypeface _planeSignTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);

		private readonly SKPaint _minusPlaneTextPaint = new SKPaint()
		{
			Color = new SKColor(173, 26, 0),
			TextSize = 40,
			TextAlign = SKTextAlign.Center,
			IsAntialias = true
		};

		private readonly SKPaint _plusPlaneTextPaint = new SKPaint()
		{
			Color = new SKColor(0, 75, 153),
			TextSize = 40,
			TextAlign = SKTextAlign.Center,
			IsAntialias = true
		};

		private readonly SKPaint _trajectoryPaint = new SKPaint()
		{
			Color = SKColors.Black,
			IsStroke = true,
			StrokeWidth = 2,
			IsAntialias = true
		};

		private TrajectoryPoint[] _trajectory;

		public ElectricParticleCanvasController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
			_minusPlaneTextPaint.Typeface = _planeSignTypeface;
			_plusPlaneTextPaint.Typeface = _planeSignTypeface;
		}

		public void StartSimulation()
		{
			SimulationTime.Restart();
			Play();
		}

		public void SetMotion(ElectricParticleSimulationSetup setup)
		{
			_setup = setup;

			if (_setup == null)
			{
				return;
			}

			var unitXSize = -1f;
			var unitYSize = -1f;

			if (_setup.VerticalPlane != null)
			{
				unitXSize = _setup.VerticalPlane.Distance * 1.25f;
			}

			if (_setup.HorizontalPlane != null)
			{
				unitYSize = _setup.HorizontalPlane.Distance * 1.25f;
			}

			_unitDimension = Math.Max(unitXSize, unitYSize);

			var color = ColorHelper.ToColor(setup.Color);
			_particlePaint.Color = new SKColor(color.R, color.G, color.B);
			_trajectoryPaint.Color = new SKColor(color.R, color.G, color.B);
			_service = new PhysicsService(_setup);
			_trajectory = _service.ComputeTrajectory(600);
		}

		public override void Update(ISkiaCanvas sender)
		{
			var minDimension = Math.Min(sender.ScaledSize.Width, sender.ScaledSize.Height);
			_pixelToUnit = _unitDimension / minDimension;
			_unitToPixel = minDimension / _unitDimension;
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_setup == null)
			{
				return;
			}

			DrawPlanes(sender, args);

			DrawParticleTrajectory(sender, args);
			DrawParticle(sender, args);
		}

		private void DrawPlanes(ISkiaCanvas sender, SKSurface args)
		{
			if (_setup.HorizontalPlane != null)
			{
				DrawHorizontalPlanes(sender, args);
			}

			if (_setup.VerticalPlane != null)
			{
				DrawVerticalPlanes(sender, args);
			}
		}

		private void DrawParticle(ISkiaCanvas sender, SKSurface args)
		{
			var point = _trajectory[Math.Min(_trajectory.Length - 1, SimulationTime.UpdateCount)];

			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			var particleCenter = new SKPoint(zeroX + point.X * _unitToPixel, zeroY - point.Y * _unitToPixel);
			var particleSign = _setup.Particle.Polarity == Polarity.Positive ? PlusSign : MinusSign;
			var textMeasure = _particleText.MeasureText(particleSign);
			args.Canvas.DrawCircle(particleCenter, 12, _particlePaint);
			particleCenter.Y = particleCenter.Y + textMeasure / 2;
			args.Canvas.DrawText(particleSign, particleCenter, _particleText);
		}

		private void DrawParticleTrajectory(ISkiaCanvas sender, SKSurface args)
		{
			if (_trajectory.Length == 0)
			{
				return;
			}

			using var path = new SKPath();

			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			var startPoint = new SKPoint(zeroX + _trajectory[0].X * _unitToPixel, zeroY - _trajectory[0].Y * _unitToPixel);
			path.MoveTo(startPoint);

			for (int i = 0; i < Math.Min(SimulationTime.UpdateCount + 1, _trajectory.Length); i++)
			{
				var point = _trajectory[Math.Min(_trajectory.Length - 1, i)];

				var particleCenter = new SKPoint(zeroX + point.X * _unitToPixel, zeroY - point.Y * _unitToPixel);
				path.LineTo(particleCenter);
			}

			args.Canvas.DrawPath(path, _trajectoryPaint);
		}

		private void DrawVerticalPlanes(ISkiaCanvas sender, SKSurface args)
		{
			var verticalPlaneDistance = _setup.VerticalPlane.Distance;
			var horizontalPlaneDistance = _setup.HorizontalPlane?.Distance ?? sender.ScaledSize.Height * 0.8f * _pixelToUnit;
			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			var planeTop = zeroY - horizontalPlaneDistance * _unitToPixel / 2;
			var planeBottom = zeroY + horizontalPlaneDistance * _unitToPixel / 2;

			var leftPlaneX = zeroX - verticalPlaneDistance * _unitToPixel / 2;
			var rightPlaneX = zeroX + verticalPlaneDistance * _unitToPixel / 2;

			var leftPlaneSignMeasure = _plusPlaneTextPaint.MeasureText(_setup.VerticalPlane.Polarity == Polarity.Positive ? PlusSign : MinusSign);
			var leftPlaneSign = _setup.VerticalPlane.Polarity == Polarity.Positive ? PlusSign : MinusSign;
			var leftPlanePaint = _setup.VerticalPlane.Polarity == Polarity.Positive ? _plusPlanePaint : _minusPlanePaint;
			var leftPlaneTextPaint = _setup.VerticalPlane.Polarity == Polarity.Positive ? _plusPlaneTextPaint : _minusPlaneTextPaint;

			args.Canvas.DrawLine(
				new SKPoint(leftPlaneX, planeTop),
				new SKPoint(leftPlaneX, planeBottom),
				leftPlanePaint);

			args.Canvas.DrawText(leftPlaneSign, leftPlaneX - 20, (planeTop + planeBottom) / 2 + leftPlaneSignMeasure / 2, leftPlaneTextPaint);

			var rightPlaneSignMeasure = _plusPlaneTextPaint.MeasureText(_setup.VerticalPlane.Polarity == Polarity.Negative ? PlusSign : MinusSign);
			var rightPlaneSign = _setup.VerticalPlane.Polarity == Polarity.Negative ? PlusSign : MinusSign;
			var rightPlanePaint = _setup.VerticalPlane.Polarity == Polarity.Negative ? _plusPlanePaint : _minusPlanePaint;
			var rightPlaneTextPaint = _setup.VerticalPlane.Polarity == Polarity.Negative ? _plusPlaneTextPaint : _minusPlaneTextPaint;

			args.Canvas.DrawLine(
				new SKPoint(rightPlaneX, planeTop),
				new SKPoint(rightPlaneX, planeBottom),
				rightPlanePaint);

			args.Canvas.DrawText(rightPlaneSign, rightPlaneX + 20, (planeTop + planeBottom) / 2 + rightPlaneSignMeasure / 2, rightPlaneTextPaint);
		}

		private void DrawHorizontalPlanes(ISkiaCanvas sender, SKSurface args)
		{
			var verticalPlaneDistance = _setup.VerticalPlane?.Distance ?? sender.ScaledSize.Width * 0.8f * _pixelToUnit;
			var horizontalPlaneDistance = _setup.HorizontalPlane.Distance;
			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			var planeLeft = zeroX - verticalPlaneDistance * _unitToPixel / 2;
			var planeRight = zeroX + verticalPlaneDistance * _unitToPixel / 2;

			var topPlaneY = zeroY - horizontalPlaneDistance * _unitToPixel / 2;
			var bottomPlaneY = zeroY + horizontalPlaneDistance * _unitToPixel / 2;

			var topPlaneSignMeasure = _plusPlaneTextPaint.MeasureText(_setup.HorizontalPlane.Polarity == Polarity.Negative ? PlusSign : MinusSign);
			var topPlaneSign = _setup.HorizontalPlane.Polarity == Polarity.Negative ? PlusSign : MinusSign;
			var topPlanePaint = _setup.HorizontalPlane.Polarity == Polarity.Negative ? _plusPlanePaint : _minusPlanePaint;
			var topPlaneTextPaint = _setup.HorizontalPlane.Polarity == Polarity.Negative ? _plusPlaneTextPaint : _minusPlaneTextPaint;

			args.Canvas.DrawLine(
				new SKPoint(planeLeft, topPlaneY),
				new SKPoint(planeRight, topPlaneY),
				topPlanePaint);

			args.Canvas.DrawText(topPlaneSign, (planeLeft + planeRight) / 2, topPlaneY - 20 + topPlaneSignMeasure / 2, topPlaneTextPaint);

			var bottomPlaneSignMeasure = _plusPlaneTextPaint.MeasureText(_setup.HorizontalPlane.Polarity == Polarity.Positive ? PlusSign : MinusSign);
			var bottomPlaneSign = _setup.HorizontalPlane.Polarity == Polarity.Positive ? PlusSign : MinusSign;
			var bottomPlanePaint = _setup.HorizontalPlane.Polarity == Polarity.Positive ? _plusPlanePaint : _minusPlanePaint;
			var bottomPlaneTextPaint = _setup.HorizontalPlane.Polarity == Polarity.Positive ? _plusPlaneTextPaint : _minusPlaneTextPaint;

			args.Canvas.DrawLine(
				new SKPoint(planeLeft, bottomPlaneY),
				new SKPoint(planeRight, bottomPlaneY),
				bottomPlanePaint);

			args.Canvas.DrawText(bottomPlaneSign, (planeLeft + planeRight) / 2, bottomPlaneY + 20 + bottomPlaneSignMeasure / 2, bottomPlaneTextPaint);
		}
	}
}
