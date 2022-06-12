using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MvvmCross.WeakSubscription;
using Physics.GravitationalFieldMovement.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Physics.GravitationalFieldMovement.Rendering.Extensions;
using Windows.ApplicationModel;
using Typography.TextLayout;
using Physics.Shared.UI.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;
using SkiaSharp.Views.UWP;
using Physics.Shared.UI.Infrastructure.Topics;

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

		private double _lastPlanetRadius = 0;

		public SKPaint PlanetPaint { get; set; } = new SKPaint()
		{
			Color = SKColors.LightGray,
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

		private int _currentFrame = 0;
		
		public GravitationalFieldMovementCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
			var gameAssetsPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Objects");
			EarthBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "earth.png"));
		}

		public void SetInputConfiguration(InputConfiguration input, double dt)
		{
			if (Planet != null)
			{
				var color = ColorHelper.ToColor(Planet.ColorHex);
				PlanetPaint.Color = color.ToSKColor();
			} else
			{
				PlanetPaint.Color = SKColors.LightGray;
			}
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
			if (_trajectory == null || InputStoppingCondition())
			{
				return;
			}

			DrawPlanet(sender, args);
			DrawTrajectory(sender, args);
		}

		private bool InputStoppingCondition()
		{
			if (_trajectory == null)
			{
				return false;
			}

			return _input.ConicSec switch
			{
				MovementType.Ellipse =>
					_trajectory switch
					{
						_ => false
					},
				_ => false
			};
		}

		public TrajectoryPoint CurrentPoint => _trajectory?.Length > 0 ? _trajectory[_currentFrame] : null;

		public override void Update(ISkiaCanvas sender)
		{
			if (_trajectory == null)
			{
				return;
			}

			var scaleX = sender.ScaledSize.Width * 0.8 / (2 * Math.Max(Math.Abs(_maxX), Math.Abs(_minX)));
			var scaleY = sender.ScaledSize.Height * 0.8 / (2 * Math.Max(Math.Abs(_maxY), Math.Abs(_minY)));
			_scale = Math.Min(scaleX, scaleY);

			_currentFrame = (int)(SimulationTime.TotalTime.TotalSeconds / (1 / 60.0));
			_currentFrame = Math.Min(_currentFrame, _trajectory.Length - 1);
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
			foreach (var point in _trajectory.Take(_currentFrame))
			{
				x = GetRenderX(point.X);
				y = GetRenderY(point.Y);
				path.LineTo(x, y);
			}

			args.Canvas.DrawPath(path, _trajectoryPaint);

			var endPoint = _trajectory[_currentFrame];
			x = GetRenderX(endPoint.X);
			y = GetRenderY(endPoint.Y);
			args.Canvas.DrawCircle(x, y, 4, _trajectoryFillPaint);
		}

		private void DrawPlanet(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.DrawCircle(GetRenderX(0), GetRenderY(0), (float)PlanetRadius, PlanetPaint);
		}

		public SKBitmap EarthBitmap { get; private set; }
		public PlanetPreset Planet { get; set; }

		public double PlanetRadius => Math.Max(4, _scale * _input.Rz);

		private float GetRenderX(double x) => (float)(_canvas.ScaledSize.Width / 2 + x * _scale);

		private float GetRenderY(double y) => (float)(_canvas.ScaledSize.Height / 2 - y * _scale);
	}
}
