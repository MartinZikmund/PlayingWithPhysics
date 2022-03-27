using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Physics.GravitationalFieldMovement.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.GravitationalFieldMovement.Rendering
{
	public class GravitationalFieldMovementCanvasController : SkiaCanvasController
	{
		private TrajectoryPoint[] _trajectory;
		private InputConfiguration _input;

		private double _minX = 0;
		private double _maxX = 0;
		private double _minY = 0;
		private double _maxY = 0;

		private double _scale = 1;

		private readonly SKPaint _planetPaint = new SKPaint()
		{
			Color = new SKColor(150, 150, 150, 150),
			IsStroke = false,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High
		};

		private readonly SKPaint _trajectoryPaint = new SKPaint()
		{
			Color = SKColors.Red,
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High
		};

		private readonly SKPaint _trajectoryFillPaint = new SKPaint()
		{
			Color = SKColors.Red,
			IsStroke = false,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High
		};

		public GravitationalFieldMovementCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public void SetInputConfiguration(InputConfiguration input, double dt)
		{
			_input = input;
			var physicsService = new PhysicsService(input, dt);

			_trajectory = physicsService.CalculateTrajectory();

			_minX = _trajectory.Select(p => p.X).Append(-_input.R0).Min();
			_maxX = _trajectory.Select(p => p.X).Append(_input.R0).Max();
			_minY = _trajectory.Select(p => p.Y).Append(-_input.R0).Min();
			_maxY = _trajectory.Select(p => p.Y).Append(_input.R0).Max();
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			if (_trajectory == null)
			{
				return;
			}

			DrawPlanet(sender, args);
			DrawTrajectory(sender, args);
		}


		public override void Update(ISkiaCanvas sender)
		{
			if (_trajectory == null)
			{
				return;
			}

			var scaleX = sender.ScaledSize.Width * 0.8 / (2* Math.Max(Math.Abs(_maxX), Math.Abs(_minX)));
			var scaleY = sender.ScaledSize.Height * 0.8 / (2 * Math.Max(Math.Abs(_maxY),Math.Abs(_minY)));
			_scale = Math.Min(scaleX, scaleY);
		}

		private void DrawTrajectory(ISkiaCanvas sender, SKSurface args)
		{
			if (_trajectory.Length == 0)
			{
				return;
			}

			using var path = new SKPath();
			var startingPoint = _trajectory.First();
			var x = GetRenderX(startingPoint.X);
			var y = GetRenderY(startingPoint.Y);
			path.MoveTo(x, y);
			foreach (var point in _trajectory)
			{
				x = GetRenderX(point.X);
				y = GetRenderY(point.Y);
				path.LineTo(x, y);
			}

			args.Canvas.DrawPath(path, _trajectoryPaint);

			foreach (var point in _trajectory)
			{
				x = GetRenderX(point.X);
				y = GetRenderY(point.Y);
				args.Canvas.DrawCircle(x, y, 2, _trajectoryFillPaint);
			}
		}

		private void DrawPlanet(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.DrawCircle(GetRenderX(0), GetRenderY(0), (float)(_scale * _input.Rz), _planetPaint);
		}

		private float GetRenderX(double x) => (float)(_canvas.ScaledSize.Width / 2 + x * _scale);

		private float GetRenderY(double y) => (float)(_canvas.ScaledSize.Height / 2 - y * _scale);
	}
}
