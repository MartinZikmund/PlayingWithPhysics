using System.Collections.Generic;
using Physics.Shared.UI.Rendering.Skia;
using Physics.StationaryWaves.Logic;
using SkiaSharp;

namespace Physics.StationaryWaves.Rendering
{
	public class StationaryWavesRenderer : ISkiaVariantRenderer
	{
		protected readonly StationaryWavesCanvasController _controller;

		private const float HorizontalPadding = 12;
		private List<WaveTrajectory> _waveTrajectories;
		private WaveInfo _wave;
		private AdvancedWavePhysicsService _physicsService;
		private WaveInterferenceTrajectory _compoundTrajectory;

		private float _pixelsPerUnit;

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

		public void SetActiveWaves(WaveInfo wave)
		{
			_wave = wave;
			_physicsService = new AdvancedWavePhysicsService(wave);

			_waveTrajectories = new List<WaveTrajectory>();
			_waveTrajectories.Add(new WaveTrajectory(_physicsService, wave, true));
			_waveTrajectories.Add(new WaveTrajectory(_physicsService, wave, false));

			_compoundTrajectory = new WaveInterferenceTrajectory(_waveTrajectories[0], _waveTrajectories[1]);
		}


		public void Dispose() { }

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			if (_wave == null)
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

			var trajectory1 = _waveTrajectories[0];
			DrawTrajectory(sender, args, trajectory1, _axis1Paint);

			var trajectory2 = _waveTrajectories[1];
			DrawTrajectory(sender, args, trajectory2, _axis2Paint);

			DrawTrajectory(sender, args, _compoundTrajectory, _interferenceStrokePaint);
		}

		public void Update(ISkiaCanvas sender)
		{
			if (_wave != null)
			{
				_pixelsPerUnit = (float)Math.Min(_canvas.ScaledSize.Width / _wave.DrawingRange.Max, _canvas.ScaledSize.Height / (_wave.Amplitude * 4.5f));

				_minX = 0;
				_maxX = _wave.DrawingRange.Max;
			}
		}

		private void DrawTrajectory(ISkiaCanvas sender, SKSurface args, IWaveTrajectory trajectory, SKPaint paint)
		{
			float stepSizeInUnits = 0.01f;
			var lastRenderTime = GetAdjustedTotalTime();
			using SKPath path = new SKPath();
			var currentX = _minX;

			float lastRenderX = GetRenderX(currentX);
			float lastRenderY = GetRenderY(trajectory.GetY(currentX, (float)lastRenderTime).Value);
			path.MoveTo(lastRenderX, lastRenderY);

			while (currentX <= _maxX)
			{
				var renderX = GetRenderX(currentX);
				var y = trajectory.GetY(currentX, (float)lastRenderTime);
				float renderY = GetRenderY(y.Value);

				path.LineTo(renderX, renderY);

				currentX += stepSizeInUnits;
			}

			args.Canvas.DrawPath(path, paint);
		}

		private double GetAdjustedTotalTime() => Math.Truncate(SimulationTime.TotalTime.TotalMilliseconds) / 1000;

		private float GetDisplayHeightInPixels(ISkiaCanvas sender) => (float)sender.ScaledSize.Height;

		private float GetDisplayWidthInPixels(ISkiaCanvas sender) => (float)(sender.ScaledSize.Width - 2 * HorizontalPadding);

		private float GetRenderY(float y) => (float)(_canvas.ScaledSize.Height / 2 - y * _pixelsPerUnit);

		private float GetRenderX(float x) => (float)(_canvas.ScaledSize.Width / 2) + ((-_wave.DrawingRange.Max / 2 + x) * _pixelsPerUnit);
	}
}
