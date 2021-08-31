﻿using System;
using System.Collections.Generic;
using System.Linq;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using Physics.WaveInterference.Logic;
using Physics.WaveInterference.ViewModels;
using SkiaSharp;

namespace Physics.WaveInterference.Rendering
{
	public class WaveInterferenceController : SkiaCanvasController
	{
		private const float VerticalPadding = 12;

		private float _verticalScale = 1f;
		private float _horizontalScale = 300f;

		private WaveInfoViewModel[] _activeOscillations;
		private Dictionary<WaveInfo, OscillationTrajectory> _oscillationTrajectories;
		private Dictionary<WaveInfo, SKPaint> _oscillationFillPaints;
		private Dictionary<WaveInfo, SKPaint> _oscillationStrokePaints;		
		private CompoundOscillationTrajectory _compoundOscillationTrajectory;
		private SKPaint _compoundFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
			Color = SKColors.Black
		};
		private SKPaint _compoundStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			StrokeWidth = 3,
			Color = SKColors.Black
		};

		private SKPaint _axesPaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1,
			Color = SKColors.Black
		};

		private float _maxY;
		private float _minY;
		private float _maximumFrequency;
		private double _inherentSlowdown = 1.0;

		public WaveInterferenceController(ISkiaCanvas canvasAnimatedControl) :
			base(canvasAnimatedControl)
		{
		}

		public void SetActiveOscillations(WaveInfoViewModel[] info)
		{
			if (info == null || info.Length == 0)
			{
				_activeOscillations = null;
				return;
			}

			_activeOscillations = info;
			_oscillationTrajectories = new Dictionary<WaveInfo, OscillationTrajectory>();
			_oscillationFillPaints = new Dictionary<WaveInfo, SKPaint>();
			_oscillationStrokePaints = new Dictionary<WaveInfo, SKPaint>();
			_maxY = 0;
			_minY = 0;

			_maximumFrequency = _activeOscillations.Max(o => o.WaveInfo.Frequency);
			SetInherentSlowdown();

			foreach (var oscillation in _activeOscillations)
			{
				var slowedDownOscillationInfo = new WaveInfo(
					oscillation.WaveInfo.Label,
					oscillation.WaveInfo.Amplitude,
					(float)(oscillation.WaveInfo.Frequency * _inherentSlowdown),
					oscillation.WaveInfo.WaveLength,
					WaveDirection.Right,
					0.0f,
					oscillation.WaveInfo.Color);

				var physicsService = new WavePhysicsService(slowedDownOscillationInfo);

				_maxY += physicsService.MaxY;
				_minY += physicsService.MinY;

				//TODO:
				//_oscillationTrajectories.Add(oscillation.WaveInfo, physicsService.CreateTrajectoryData());

				var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(oscillation.WaveInfo.Color);
				var skColor = new SKColor(color.R, color.G, color.B);
				var fillPaint = new SKPaint()
				{
					Color = skColor,
					IsStroke = false,
					IsAntialias = true,
				};
				_oscillationFillPaints.Add(oscillation.WaveInfo, fillPaint);
				var strokePaint = new SKPaint()
				{
					Color = skColor,
					IsStroke = true,
					StrokeWidth = 2,
					IsAntialias = true,
				};
				_oscillationStrokePaints.Add(oscillation.WaveInfo, strokePaint);
			}

			//TODO:
			//_compoundOscillationsPhysicsService = new WavePhysicsService(_activeOscillations.Select(wave => wave.WaveInfo).ToArray());
			//_compoundOscillationTrajectory = new CompoundOscillationTrajectory(_oscillationTrajectories.Select(t => t.Value).ToArray());


		}

		private void SetInherentSlowdown()
		{
			if (_maximumFrequency > 5000)
			{
				_inherentSlowdown = 1 / 10000.0;
			}
			else if (_maximumFrequency > 500)
			{
				_inherentSlowdown = 1 / 1000.0;
			}
			else if (_maximumFrequency > 50)
			{
				_inherentSlowdown = 1 / 100.0;
			}
			else if (_maximumFrequency > 5)
			{
				_inherentSlowdown = 1 / 10.0;
			}
			else
			{
				_inherentSlowdown = 1;
			}
		}

		public void StartSimulation()
		{
			SimulationTime.Restart();
			Play();
		}

		public override void Update(ISkiaCanvas sender)
		{
			if (_activeOscillations != null)
			{
				var verticalPadding = sender.ScaledSize.Height * 0.15;
				_verticalScale = (float)(sender.ScaledSize.Height - 2 * verticalPadding) / (_maxY - _minY);
			}
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));

			if (_activeOscillations == null)
			{
				return;
			}

			var currentTime = GetAdjustedTotalTime();
			var totalValue = 0.0f;

			//DrawAxes(sender, args);

			foreach (var oscillation in _activeOscillations)
			{
				var trajectory = _oscillationTrajectories[oscillation.WaveInfo];
				if (oscillation.IsEnabled)
				{
					var y = trajectory.GetY((float)currentTime);
					totalValue += y;

					if (oscillation.IsVisible)
					{
						var strokePain = _oscillationStrokePaints[oscillation.WaveInfo];
						DrawTrajectory(sender, args, trajectory, strokePain, false);
						var fillPaint = _oscillationFillPaints[oscillation.WaveInfo];
						DrawOscillationCurrentPoint(sender, args, y, fillPaint);
					}
				}
			}

			if (_activeOscillations.Length > 1)
			{
				DrawTrajectory(sender, args, _compoundOscillationTrajectory, _compoundStrokePaint, true);
				DrawOscillationCurrentPoint(sender, args, totalValue, _compoundFillPaint);
			}
		}

		public double GetSimulationDisplayTime() => SimulationTime.TotalTime.TotalSeconds * _inherentSlowdown;

		private double GetAdjustedTotalTime() => Math.Truncate(SimulationTime.TotalTime.TotalMilliseconds) / 1000;

		private void DrawTrajectory(ISkiaCanvas sender, SKSurface args, IOscillationTrajectory trajectory, SKPaint paint, bool accurate)
		{
			//Try rendering using https://github.com/PeterWaher/IoTGateway/blob/master/Script/Waher.Script.Graphs/Functions/Plots/Plot2DCurve.cs

			using SKPath path = new SKPath();
			var lastRenderTime = GetAdjustedTotalTime();
			float endRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			var endTime = GetAdjustedTotalTime();
			float lastRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			float lastRenderY = GetRenderY(trajectory.GetY((float)lastRenderTime, accurate));
			path.MoveTo(lastRenderX, lastRenderY);
			while (lastRenderTime > 0)
			{
				var newRenderTime = lastRenderTime - (0.008);
				if (newRenderTime < 0)
				{
					newRenderTime = 0;
				}
				var renderX = (float)(endRenderX - (endTime - newRenderTime) * (_horizontalScale));
				if (renderX < 0)
				{
					break;
				}
				var renderY = (float)(sender.ScaledSize.Height / 2 - trajectory.GetY((float)newRenderTime, accurate) * _verticalScale);
				path.LineTo(renderX, renderY);
				lastRenderX = renderX;
				lastRenderY = renderY;
				lastRenderTime = newRenderTime;
			}
			args.Canvas.DrawPath(path, paint);
		}

		private SkiaAxesRenderer _axesRenderer = new SkiaAxesRenderer();

		private void DrawAxes(ISkiaCanvas sender, SKSurface args)
		{
			var axesBounds = new SimulationBounds(30, 10, (float)sender.ScaledSize.Width - 20, (float)sender.ScaledSize.Height - 10);
			var endRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			var horizontalPixelDiff = endRenderX - axesBounds.Left;
			var endTime = GetAdjustedTotalTime();
			var originSeconds = ((float)endTime - horizontalPixelDiff / _horizontalScale) * (float)_inherentSlowdown;

			_axesRenderer.YUnitSizeInPixels = _verticalScale;
			_axesRenderer.XUnitSizeInPixels = (float)(_horizontalScale / _inherentSlowdown);

			var formatString = "0.#";
			if (_maximumFrequency > 5000)
			{
				formatString = "0.#####";
			}
			else if (_maximumFrequency > 500)
			{
				formatString = "0.####";
			}
			else if (_maximumFrequency > 50)
			{
				formatString = "0.###";
			}
			else if (_maximumFrequency > 5)
			{
				formatString = "0.##";
			}
			_axesRenderer.XUnitFormatString = formatString;

			_axesRenderer.OriginUnitCoordinates = new SKPoint(originSeconds, 0);
			_axesRenderer.TargetBounds = axesBounds;
			_axesRenderer.OriginRelativePosition = new SKPoint(0, 0.5f);
			_axesRenderer.Draw(sender, args);
		}

		private void DrawVerticalAxis()
		{

		}

		private void DrawHorizontalAxis()
		{

		}

		private void DrawOscillationCurrentPoint(ISkiaCanvas sender, SKSurface args, float y, SKPaint paint)
		{
			var renderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			var renderY = GetRenderY(y);

			args.Canvas.DrawCircle(renderX, renderY, 6, paint);
		}

		private float GetRenderY(float y) => (float)(_canvas.ScaledSize.Height / 2 - y * _verticalScale);

		private float HorizontalPadding => _canvas.ScaledSize.Width * 0.1f;
	}
}
