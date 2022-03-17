using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Helpers;
using Physics.FluidFlow.Logic;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using SkiaSharp.Views.UWP;

namespace Physics.FluidFlow.Rendering
{
	public abstract class FluidFlowRenderer : ISkiaVariantRenderer
	{
		protected readonly FluidFlowCanvasController _controller;
		protected readonly ISkiaCanvas _canvas;

		private float _horizontalPadding;

		private float _pixelsPerUnit;

		private SKPaint[] _particlePaints;
		private SKPaint[] _particlePathPaints;
		private SKPaint[] _particleVectorPaints;

		protected readonly SKPaint _plumbingBorderPaint = new SKPaint()
		{
			Color = SKColors.Black,
			StrokeWidth = 2,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			IsStroke = true
		};

		protected readonly SKPaint _plumbingFillPaint = new SKPaint()
		{
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			IsStroke = false
		};

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

		public double GetAdjustedTime() => _controller.SimulationTime.TotalTime.TotalSeconds * (PhysicsService?.SimulationTimeAdjustment ?? 1f);

		internal virtual void StartSimulation(SceneConfiguration sceneConfiguration)
		{
			var fluidColor = sceneConfiguration.Fluid.Color;
			_plumbingFillPaint.Color = ColorHelper.ToColor(fluidColor).ToSKColor();
		}

		public void Dispose() { }

		public void Update(ISkiaCanvas sender)
		{
			if (PhysicsService == null)
			{
				return;
			}

			_horizontalPadding = _canvas.ScaledSize.Width / 20;
			MinRenderX = _horizontalPadding;
			MaxRenderX = _canvas.ScaledSize.Width - _horizontalPadding;

			var pixelsPerUnitX = ((float)_canvas.ScaledSize.Width - 2 * _horizontalPadding) / PhysicsService.XMax;
			var pixelsPerUnitY = ((float)_canvas.ScaledSize.Height / 2) / (Math.Abs(PhysicsService.YMax) + Math.Abs(PhysicsService.YMin));
			_pixelsPerUnit = Math.Min(pixelsPerUnitX, pixelsPerUnitY);
		}

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			if (PhysicsService == null)
			{
				return;
			}

			DrawPlumbing(args.Canvas);
			DrawTrajectory(args.Canvas);
			DrawVectors(args.Canvas);

			var time = (float)GetAdjustedTime();
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var position = PhysicsService.GetParticlePosition(time, particleId);
				var x = GetRenderX((float)Math.Min(position.X, PhysicsService.XMax));
				var y = GetRenderY((float)position.Y);
				args.Canvas.DrawCircle(x, y, 4, _particlePaints[particleId % _particlePaints.Length]);
			}
		}

		protected virtual SKPath GetPlumbingPath() => new SKPath();

		protected virtual void DrawPlumbing(SKCanvas args)
		{
			using var path = GetPlumbingPath();

			args.DrawPath(path, _plumbingFillPaint);
			args.DrawPath(path, _plumbingBorderPaint);
		}

		protected virtual void DrawTrajectory(SKCanvas canvas)
		{
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var paint = _particlePathPaints[particleId % _particlePathPaints.Length];
				using var path = new SKPath();
				var startPoint = PhysicsService.GetParticlePosition(0, particleId);
				path.MoveTo(GetRenderX((float)startPoint.X), GetRenderY((float)startPoint.Y));
				var time = 0f;
				var timeDiff = 0.01f;
				bool pathFinished = false;
				do
				{
					time += timeDiff;
					if (time > GetAdjustedTime() ||
						time > PhysicsService.MaxT)
					{
						time = Math.Min(
							(float)GetAdjustedTime(),
							PhysicsService.MaxT);
						pathFinished = true;
					}

					var newPosition = PhysicsService.GetParticlePosition(time, particleId);
					if (newPosition.X > PhysicsService.XMax)
					{
						pathFinished = true;
						newPosition = new Point2d(PhysicsService.XMax, newPosition.Y);
					}
					path.LineTo(GetRenderX((float)newPosition.X), GetRenderY((float)newPosition.Y));

				} while (!pathFinished);

				canvas.DrawPath(path, paint);
			}
		}

		protected virtual void DrawVectors(SKCanvas canvas)
		{
			var x16 = MinRenderX + (MaxRenderX - MinRenderX) / 6;
			var x56 = MinRenderX + (MaxRenderX - MinRenderX) * 5 / 6;
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var paint = _particleVectorPaints[particleId % _particleVectorPaints.Length];
				// 1/6
				var position = PhysicsService.GetParticlePosition(PhysicsService.Vector1T, particleId);
				var size = GetVelocityVectorSize(0, particleId);
				var y = GetRenderY((float)position.Y);
				if (size > 0)
				{
					Shared.UI.Rendering.Skia.ArrowRenderer.Draw(canvas, new SKPoint(x16, y), new SKPoint(x16 + size, y), 3, paint);
				}

				// 5/6
				position = PhysicsService.GetParticlePosition(PhysicsService.Vector2T, particleId);
				size = GetVelocityVectorSize(1, particleId);
				y = GetRenderY((float)position.Y);
				if (size > 0)
				{
					Shared.UI.Rendering.Skia.ArrowRenderer.Draw(canvas, new SKPoint(x56, y), new SKPoint(x56 + size, y), 3, paint);
				}
			}
		}

		protected abstract float GetVelocityVectorSize(int vectorId, int particleId);

		protected float GetRenderX(float x) => _canvas.ScaledSize.Width / 2 - PhysicsService.XMax / 2 * _pixelsPerUnit + x * _pixelsPerUnit;

		protected float GetRenderY(float y) => _canvas.ScaledSize.Height / 2 - (y - PhysicsService.YMin) * _pixelsPerUnit + Math.Abs(PhysicsService.YMax - PhysicsService.YMin) / 2 * _pixelsPerUnit;

		protected SKPaint GetParticlePathPaint(int particleId) => _particlePathPaints[particleId % _particlePathPaints.Length];
	}
}
