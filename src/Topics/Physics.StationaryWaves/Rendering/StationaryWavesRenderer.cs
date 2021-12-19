using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Rendering.Skia;
using Physics.StationaryWaves.Logic;
using SkiaSharp;

namespace Physics.StationaryWaves.Rendering
{
	public abstract class StationaryWavesRenderer : ISkiaVariantRenderer
	{
		protected readonly StationaryWavesCanvasController _controller;
		protected readonly ISkiaCanvas _canvas;

		private const float HorizontalPadding = 12;
		private AdvancedWavePhysicsService _physicsService;

		private float _pixelsPerUnitX;
		private float _pixelsPerUnitY;

		private float _minX;
		private float _maxX;

		private SKPaint _interferenceFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
			Color = SKColors.Black
		};

		private SKPaint _interferenceStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			StrokeWidth = 5,
			Color = SKColors.Red
		};

		private SKPaint _axis1Paint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1,
			Color = SKColors.Green
		};

		private SKPaint _axis2Paint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1.5f,
			Color = SKColors.Blue
		};

		private SKPaint _axisPaintHorizontal = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1.5f,
			Color = SKColors.Black
		};

		public StationaryWavesRenderer(StationaryWavesCanvasController controller)
		{
			_controller = controller;
			_canvas = controller.Canvas;
		}

		protected abstract IWavePhysicsService WavePhysicsService { get; }

		internal abstract void StartSimulation(BounceType bounceType, float width);

		public void Dispose() { }

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			if (WavePhysicsService == null)
			{
				return;
			}

			//DrawAxes(sender, args);
			args.Canvas.DrawLine(new SKPoint(0, GetDisplayHeightInPixels(sender) / 2), new SKPoint(GetDisplayWidthInPixels(sender), GetDisplayHeightInPixels(sender) / 2), _axisPaintHorizontal);

			//var pointSize = 6f;
			//foreach (var wave in _activeWaves)
			//{
			//	var trajectory = _waveTrajectories[wave.WaveInfo];

			//	var strokePain = _waveStrokePaints[wave.WaveInfo];
			//	DrawTrajectory(sender, args, trajectory, strokePain, false);
			//	var fillPaint = _waveFillPaints[wave.WaveInfo];
			//	DrawWaveOriginPoint(sender, args, trajectory, fillPaint, pointSize);
			//	pointSize -= 2;
			//}

			DrawTrajectory(sender, args, (x, time) => WavePhysicsService.CalculateFirstWaveY(x, time), _axis1Paint);
			DrawTrajectory(sender, args, (x, time) => WavePhysicsService.CalculateSecondWaveY(x, time), _axis2Paint);
			DrawTrajectory(sender, args, (x,time) => WavePhysicsService.CalculateCompoundY(x, time), _interferenceStrokePaint);
		}

		public void Update(ISkiaCanvas sender)
		{
			if (WavePhysicsService != null)
			{
				_pixelsPerUnitX = (float)_canvas.ScaledSize.Width / WavePhysicsService.MaxX;
				_pixelsPerUnitY = (float)_canvas.ScaledSize.Height / (WavePhysicsService.Amplitude * 4.5f);


				_minX = 0;
				_maxX = WavePhysicsService.MaxX;
			}
		}

		private void DrawTrajectory(ISkiaCanvas sender, SKSurface args, Func<float, float, float?> trajectoryCalculator, SKPaint paint)
		{
			float stepSizeInUnits = 0.01f;
			var lastRenderTime = GetAdjustedTotalTime();
			using SKPath path = new SKPath();
			var currentX = _minX;

			float lastRenderX = GetRenderX(currentX);
			var y = trajectoryCalculator(currentX, (float)lastRenderTime);
			var penUp = y == null;
			if (y != null)
			{
				float lastRenderY = GetRenderY(y.Value);
				path.MoveTo(lastRenderX, lastRenderY);
			}
			while (currentX <= _maxX)
			{
				var renderX = GetRenderX(currentX);
				y = trajectoryCalculator(currentX, (float)lastRenderTime);
				if (y != null)
				{
					float renderY = GetRenderY(y.Value);

					if (penUp)
					{
						path.MoveTo(renderX, renderY);
						penUp = false;
					}
					else
					{
						path.LineTo(renderX, renderY);
					}
				}
				else
				{
					penUp = true;
				}
				currentX += stepSizeInUnits;
			}

			args.Canvas.DrawPath(path, paint);
		}

		private double GetAdjustedTotalTime() => Math.Truncate(_controller.SimulationTime.TotalTime.TotalMilliseconds) / 1000;

		private float GetDisplayHeightInPixels(ISkiaCanvas sender) => (float)sender.ScaledSize.Height;

		private float GetDisplayWidthInPixels(ISkiaCanvas sender) => (float)(sender.ScaledSize.Width);

		private float GetRenderY(float y) => (float)(_canvas.ScaledSize.Height / 2 - y * _pixelsPerUnitY);

		private float GetRenderX(float x) => (float)(_canvas.ScaledSize.Width / 2) + ((-WavePhysicsService.MaxX / 2 + x) * _pixelsPerUnitX);
	}
}
