using System;
using System.Collections.Generic;
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

		private float _horizontalPadding;

		private float _pixelsPerUnitX;
		private float _pixelsPerUnitY;

		private SKPaint[] _particlePaints;
		private SKPaint[] _particlePathPaints;
		private SKPaint[] _particleVectorPaints;

		protected float MinRenderX { get; private set; }

		protected float MaxRenderX { get; private set; }

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
			var vectorPaints = new List<SKPaint>();
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
				vectorPaints.Add(new SKPaint()
				{
					StrokeWidth = 2,
					IsStroke = true,
					IsAntialias = true,
					FilterQuality = SKFilterQuality.High,
					Color = color
				});
			}
			_particlePaints = paints.ToArray();
			_particlePathPaints = pathPaints.ToArray();
			_particleVectorPaints = vectorPaints.ToArray();
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

			_horizontalPadding = _canvas.ScaledSize.Width / 10;
			MinRenderX = _horizontalPadding;
			MaxRenderX = _canvas.ScaledSize.Width - _horizontalPadding;
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

			DrawTrajectory(args.Canvas);
			DrawVectors(args.Canvas);

			var time = (float)_controller.SimulationTime.TotalTime.TotalSeconds;
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var position = PhysicsService.GetParticlePosition(time, particleId);
				var x = GetRenderX((float)position.X);
				var y = GetRenderY((float)position.Y);
				args.Canvas.DrawCircle(x, y, 4, _particlePaints[particleId % _particlePaints.Length]);
			}
		}

		private void DrawTrajectory(SKCanvas canvas)
		{
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var paint = _particlePathPaints[particleId % _particlePathPaints.Length];
				using var path = new SKPath();
				var startPoint = PhysicsService.GetParticlePosition(0, particleId);
				path.MoveTo(GetRenderX((float)startPoint.X), GetRenderY((float)startPoint.Y));
				var time = 0f;
				var timeDiff = 1f;
				bool pathFinished = false;
				do
				{
					time += timeDiff;
					if (time > _controller.SimulationTime.TotalTime.TotalSeconds ||
						time > PhysicsService.MaxT)
					{
						time = Math.Min(
							(float)_controller.SimulationTime.TotalTime.TotalSeconds,
							PhysicsService.MaxT);
						pathFinished = true;
					}

					var newPosition = PhysicsService.GetParticlePosition(time, particleId);
					path.LineTo(GetRenderX((float)newPosition.X), GetRenderY((float)newPosition.Y));

				} while (!pathFinished);

				canvas.DrawPath(path, paint);
			}
		}

		private void DrawVectors(SKCanvas canvas)
		{
			var x16 = MinRenderX + (MaxRenderX - MinRenderX) / 6;
			var x56 = MinRenderX + (MaxRenderX - MinRenderX) * 5 / 6;
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var paint = _particleVectorPaints[particleId % _particleVectorPaints.Length];
				// 1/6
				var position = PhysicsService.GetParticlePosition(PhysicsService.MaxT / 6, particleId);
				var size = GetVelocityVectorSize(0, particleId);
				var y = GetRenderY((float)position.Y);
				if (size > 0)
				{
					Shared.UI.Rendering.Skia.ArrowRenderer.Draw(canvas, new SKPoint(x16, y), new SKPoint(x16 + size, y), 3, paint);
				}

				// 5/6
				position = PhysicsService.GetParticlePosition(PhysicsService.MaxT * 5 / 6, particleId);
				size = GetVelocityVectorSize(1, particleId);
				y = GetRenderY((float)position.Y);
				if (size > 0)
				{
					Shared.UI.Rendering.Skia.ArrowRenderer.Draw(canvas, new SKPoint(x56, y), new SKPoint(x56 + size, y), 3, paint);
				}
			}
		}

		protected abstract float GetVelocityVectorSize(int vectorId, int particleId);

		private float GetRenderX(float x) => _horizontalPadding + x * _pixelsPerUnitX;

		private float GetRenderY(float y) => _canvas.ScaledSize.Height / 2 - (y - PhysicsService.YMin) * _pixelsPerUnitY + Math.Abs(PhysicsService.YMax - PhysicsService.YMin) / 2 * _pixelsPerUnitY;
	}
}
