using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.FluidFlow.Logic;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.FluidFlow.Rendering
{
	public abstract class FluidFlowRenderer : ISkiaVariantRenderer
	{
		protected readonly FluidFlowCanvasController _controller;
		protected readonly ISkiaCanvas _canvas;

		private SKBitmap _particlePathsBitmap;
		private TimeSpan _pathBitmapRenderTime;

		private float _horizontalPadding;

		private float _pixelsPerUnitX;
		private float _pixelsPerUnitY;

		private SKPaint[] _particlePaints;
		private SKPaint[] _particlePathPaints;

		private Dictionary<int, List<ParticleTrajectoryPoint>> _pathHistory = new Dictionary<int, List<ParticleTrajectoryPoint>>();

		public FluidFlowRenderer(FluidFlowCanvasController controller)
		{
			_controller = controller;
			_canvas = controller.Canvas;

			CreatePaints();
		}

		private void CreatePaints()
		{
			var colors = new[]
			{
				"#0063B1",
				"#E81123",
				"#2D7D9A",
				"#881798",
				"#498205",
				"#515C6B",
				"#333333",
			};

			var paints = new List<SKPaint>();
			var pathPaints = new List<SKPaint>();
			foreach (var colorHex in colors)
			{
				var color = colorHex.ToSKColor();
				paints.Add(new SKPaint()
				{
					IsStroke = false,
					IsAntialias = true,
					FilterQuality = SKFilterQuality.High,
					Color = color
				});
				pathPaints.Add(new SKPaint()
				{
					IsStroke = true,
					IsAntialias = true,
					FilterQuality = SKFilterQuality.High,
					Color = color
				});
			}
			_particlePaints = paints.ToArray();
			_particlePathPaints = pathPaints.ToArray();
		}

		public abstract IPhysicsService PhysicsService { get; }

		internal abstract void StartSimulation(SceneConfiguration sceneConfiguration);

		public void Dispose() { }

		public void Update(ISkiaCanvas sender)
		{
			if (PhysicsService == null)
			{
				return;
			}

			if (_particlePathsBitmap?.Width != sender.ScaledSize.Width ||
				_particlePathsBitmap?.Height != sender.ScaledSize.Height)
			{
				_particlePathsBitmap = null;
			}

			_horizontalPadding = _canvas.ScaledSize.Width / 10;

			_pixelsPerUnitX = ((float)_canvas.ScaledSize.Width - 2 * _horizontalPadding) / PhysicsService.XMax;
			_pixelsPerUnitY = ((float)_canvas.ScaledSize.Height / 2) / (Math.Abs(PhysicsService.YMax) + Math.Abs(PhysicsService.YMin));
		}



		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			if (PhysicsService == null)
			{
				return;
			}

			if (_particlePathsBitmap == null)
			{
				_particlePathsBitmap = new SKBitmap((int)sender.ScaledSize.Width, (int)sender.ScaledSize.Height);
				DrawAllParticlePaths();
			}

			var time = (float)_controller.SimulationTime.TotalTime.TotalSeconds;
			var anyAdded = false;
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var position = PhysicsService.GetParticlePosition(time, particleId);
				var x = GetRenderX((float)position.X);
				var y = GetRenderY((float)position.Y);
				args.Canvas.DrawCircle(x, y, 4, _particlePaints[particleId % _particlePaints.Length]);

				anyAdded |= AddPointToPath(particleId, position);
			}

			if (anyAdded)
			{
				DrawLastPathParts();
			}

			args.Canvas.DrawBitmap(_particlePathsBitmap, new SKPoint(0, 0));

			args.
		}

		protected abstract void DrawVectors(SKCanvas args);

		private bool AddPointToPath(int particleId, Point2d position)
		{
			if (!_pathHistory.TryGetValue(particleId, out var path))
			{
				path = new List<Point2d>();
				_pathHistory[particleId] = path;
			}
			if (path.Count == 0 ||
				path[path.Count - 1].X != position.X ||
				path[path.Count - 1].Y != position.Y)
			{
				path.Add(position);
				return true;
			}
			return false;
		}

		private void DrawLastPathParts()
		{
			using var canvas = new SKCanvas(_particlePathsBitmap);
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				if (_pathHistory.TryGetValue(particleId, out var path))
				{
					if (path.Count > 1)
					{
						var previousPoint = path[path.Count - 2];
						var point = path[path.Count - 1];
						canvas.DrawLine(
							GetRenderX((float)previousPoint.X),
							GetRenderY((float)previousPoint.Y),
							GetRenderX((float)point.X),
							GetRenderY((float)point.Y),
							_particlePathPaints[particleId % _particlePaints.Length]);
					}
				}
			}
		}

		private void DrawAllParticlePaths()
		{
			using var canvas = new SKCanvas(_particlePathsBitmap);
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				if (_pathHistory.TryGetValue(particleId, out var path))
				{
					for (int i = 1; i < path.Count; i++)
					{
						var previousPoint = path[i - 1];
						var point = path[i];
						canvas.DrawLine(
							GetRenderX((float)previousPoint.X),
							GetRenderY((float)previousPoint.Y),
							GetRenderX((float)point.X),
							GetRenderY((float)point.Y),
							_particlePathPaints[particleId % _particlePaints.Length]);
					}
				}
			}
		}

		private float GetRenderX(float x) => _horizontalPadding + x * _pixelsPerUnitX;

		private float GetRenderY(float y) => _canvas.ScaledSize.Height / 2 - (y - PhysicsService.YMin) * _pixelsPerUnitY + Math.Abs(PhysicsService.YMax - PhysicsService.YMin) / 2 * _pixelsPerUnitY;
	}
}
