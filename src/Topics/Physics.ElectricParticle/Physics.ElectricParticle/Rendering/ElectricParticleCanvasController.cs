using System;
using System.Collections.Generic;
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

		private int _simulationFrame;
		private int _frame;
		private BigNumber _deltaT;

		private readonly SKPaint _planePaint = new SKPaint()
		{
			Color = SKColors.Black,
			StrokeWidth = 5,
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
			TextSize = 30,
			TextAlign = SKTextAlign.Center,
			IsAntialias = true
		};

		private readonly SKPaint _planeText = new SKPaint()
		{
			Color = SKColors.Black,
			TextSize = 30,
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
		}

		public void StartSimulation()
		{
			_simulationFrame = 0;
			_frame = 0;
			SimulationTime.Restart();
			Play();
		}

		public void SetMotion(ElectricParticleSimulationSetup setup)
		{
			_setup = setup;

			var unitXSize = -1f;
			var unitYSize = -1f;

			if (_setup.VerticalPlane != null)
			{
				unitXSize = _setup.VerticalPlane.Distance * 1.1f;
			}

			if (_setup.HorizontalPlane != null)
			{
				unitYSize = _setup.HorizontalPlane.Distance * 1.1f;
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

			_frame++;
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
			var point = _trajectory[Math.Min(_trajectory.Length - 1, _frame)];

			var zeroX = _canvas.ScaledSize.Width / 2;
			var zeroY = _canvas.ScaledSize.Height / 2;

			var particleCenter = new SKPoint(zeroX + point.X * _unitToPixel, zeroY - point.Y * _unitToPixel);
			var textMeasure = _particleText.MeasureText(_setup.Particle.Polarity == Polarity.Positive ? PlusSign : MinusSign);
			args.Canvas.DrawCircle(particleCenter, 14, _particlePaint);
			particleCenter.Y = particleCenter.Y + textMeasure / 2;
			args.Canvas.DrawText(_setup.Particle.Polarity == Polarity.Positive ? PlusSign : MinusSign, particleCenter, _particleText);
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

			for (int i = 0; i < Math.Min(_frame + 1, _trajectory.Length); i++)
			{
				var point = _trajectory[Math.Min(_trajectory.Length - 1, _frame)];
								
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

			args.Canvas.DrawLine(
				new SKPoint(leftPlaneX, planeTop),
				new SKPoint(leftPlaneX, planeBottom),
				_planePaint);
			var leftPlaneSignMeasure = _particleText.MeasureText(_setup.VerticalPlane.Polarity == Polarity.Positive ? PlusSign : MinusSign);

			args.Canvas.DrawText(_setup.VerticalPlane.Polarity == Polarity.Positive ? PlusSign : MinusSign, leftPlaneX + 20, planeTop, _planeText);

			args.Canvas.DrawLine(
				new SKPoint(rightPlaneX, planeTop),
				new SKPoint(rightPlaneX, planeBottom),
				_planePaint);

			args.Canvas.DrawText(PlusSign, rightPlaneX - 20, planeTop, _planeText);
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

			args.Canvas.DrawLine(
				new SKPoint(planeLeft, topPlaneY),
				new SKPoint(planeRight, topPlaneY),
				_planePaint);

			args.Canvas.DrawText(_setup.HorizontalPlane.Polarity == Polarity.Positive ? PlusSign : MinusSign, planeLeft, topPlaneY - 20, _planeText);

			args.Canvas.DrawLine(
				new SKPoint(planeLeft, bottomPlaneY),
				new SKPoint(planeRight, bottomPlaneY),
				_planePaint);

			args.Canvas.DrawText(PlusSign, planeLeft, bottomPlaneY + 20, _planeText);
		}
	}
}
