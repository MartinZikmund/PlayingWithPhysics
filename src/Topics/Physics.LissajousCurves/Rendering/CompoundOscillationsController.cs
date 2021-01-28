using System;
using System.Collections.Generic;
using System.Linq;
using Physics.LissajousCurves.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.LissajousCurves.Rendering
{
	public class CompoundOscillationsController : SkiaCanvasController
	{
		private const float VerticalPadding = 12;

		private float _displayScale = 1f;
		private float _padding = 1f;

		private OscillationInfo _horizontalOscillation;
		private OscillationInfo _verticalOscillation;

		private Dictionary<OscillationInfo, OscillationTrajectory> _oscillationTrajectories;
		private Dictionary<OscillationInfo, SKPaint> _oscillationFillPaints;
		private Dictionary<OscillationInfo, SKPaint> _oscillationStrokePaints;
		private CompoundOscillationsPhysicsService _compoundOscillationsPhysicsService;
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
			StrokeWidth = 3,
			Color = SKColors.Black
		};

		private SKPaint _borderStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1,
			Color = SKColors.Black
		};

		private SKPaint _borderDashedStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1,
			PathEffect = SKPathEffect.CreateDash(new[] { 10f, 10f }, 20),
			Color = SKColors.Gray
		};

		private float _maxDimension = 0f;

		public CompoundOscillationsController(ISkiaCanvas canvasAnimatedControl) :
			base(canvasAnimatedControl)
		{
		}

		public void SetActiveOscillations(OscillationInfo horizontalOscillation, OscillationInfo verticalOscillation)
		{
			if (horizontalOscillation is null)
			{
				throw new System.ArgumentNullException(nameof(horizontalOscillation));
			}

			if (verticalOscillation is null)
			{
				throw new System.ArgumentNullException(nameof(verticalOscillation));
			}

			_horizontalOscillation = horizontalOscillation;
			_verticalOscillation = verticalOscillation;
			_oscillationTrajectories = new Dictionary<OscillationInfo, OscillationTrajectory>();
			_oscillationFillPaints = new Dictionary<OscillationInfo, SKPaint>();
			_oscillationStrokePaints = new Dictionary<OscillationInfo, SKPaint>();

			_maxDimension = 0f;

			foreach (var oscillation in new[] { horizontalOscillation, verticalOscillation })
			{
				var physicsService = new OscillationPhysicsService(oscillation);

				_maxDimension = Math.Max(_maxDimension, oscillation.Amplitude);

				_oscillationTrajectories.Add(oscillation, physicsService.CreateTrajectoryData());

				var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(oscillation.Color);
				var skColor = new SKColor(color.R, color.G, color.B);
				var fillPaint = new SKPaint()
				{
					Color = skColor,
					IsStroke = false,
					IsAntialias = true,
				};

				_oscillationFillPaints.Add(oscillation, fillPaint);
				var strokePaint = new SKPaint()
				{
					Color = skColor,
					IsStroke = true,
					StrokeWidth = 2,
					IsAntialias = true,
				};
				_oscillationStrokePaints.Add(oscillation, strokePaint);
			}
		}

		public void StartSimulation()
		{
			SimulationTime.Restart();
			Play();
		}

		public override void Update(ISkiaCanvas sender)
		{
			if (_horizontalOscillation != null)
			{
				var minDimension = Math.Min(sender.ScaledSize.Width, sender.ScaledSize.Height);
				_padding = minDimension * 0.15f;

				var sideSize = _maxDimension * 2;

				_displayScale = (float)(minDimension - 2 * _padding) / (sideSize);
			}
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));

			if (_horizontalOscillation == null)
			{
				return;
			}

			var currentTime = (float)SimulationTime.TotalTime.TotalSeconds;
			var totalValue = 0.0f;

			// Draw border
			DrawBorders(sender, args);

			var x = _oscillationTrajectories[_horizontalOscillation].GetY(currentTime);
			var y = _oscillationTrajectories[_verticalOscillation].GetY(currentTime);

			DrawOscillationCurrentPoint(sender, args, x, 0, _oscillationFillPaints[_horizontalOscillation]);
			DrawOscillationCurrentPoint(sender, args, 0, y, _oscillationFillPaints[_verticalOscillation]);

			DrawOscillationCurrentPoint(sender, args, x, y, _compoundFillPaint, 8);

			//foreach (var oscillation in _activeOscillations)
			//{
			//	var trajectory = _oscillationTrajectories[oscillation];
			//	var y = trajectory.GetY(currentTime);
			//	totalValue += y;

			//	var strokePain = _oscillationStrokePaints[oscillation];
			//	DrawTrajectory(sender, args, trajectory, strokePain);
			//	var fillPaint = _oscillationFillPaints[oscillation];
			//	DrawOscillationCurrentPoint(sender, args, y, fillPaint);
			//}

			//if (_activeOscillations.Length > 1)
			//{
			//	DrawTrajectory(sender, args, _compoundOscillationTrajectory, _compoundStrokePaint);
			//	DrawOscillationCurrentPoint(sender, args, totalValue, _compoundFillPaint);
			//}
		}

		private void DrawBorders(ISkiaCanvas sender, SKSurface surface)
		{
			var centerX = sender.ScaledSize.Width / 2;
			var centerY = sender.ScaledSize.Height / 2;
			surface.Canvas.DrawRect(centerX - _maxDimension * _displayScale, centerY - _maxDimension * _displayScale, _maxDimension * 2 * _displayScale, _maxDimension * 2 * _displayScale, _borderStrokePaint);
			surface.Canvas.DrawLine(centerX, centerY - _maxDimension * _displayScale, centerX, centerY + _maxDimension * _displayScale, _borderDashedStrokePaint);
			surface.Canvas.DrawLine(centerX - _maxDimension * _displayScale, centerY, centerX + _maxDimension * _displayScale, centerY, _borderDashedStrokePaint);
		}

		private void DrawTrajectory(ISkiaCanvas sender, SKSurface args, IOscillationTrajectory trajectory, SKPaint paint)
		{
			////Try rendering using https://github.com/PeterWaher/IoTGateway/blob/master/Script/Waher.Script.Graphs/Functions/Plots/Plot2DCurve.cs

			//using SKPath path = new SKPath();
			//float lastRenderTime = (float)SimulationTime.TotalTime.TotalSeconds;
			//float endRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			//var endTime = (float)SimulationTime.TotalTime.TotalSeconds;
			//float lastRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			//float lastRenderY = GetRenderY(trajectory.GetY((float)SimulationTime.TotalTime.TotalSeconds));
			//path.MoveTo(lastRenderX, lastRenderY);
			//while (lastRenderTime > 0)
			//{
			//	var newRenderTime = lastRenderTime - 0.01f;
			//	if (newRenderTime < 0)
			//	{
			//		break;
			//	}
			//	var renderX = endRenderX - (endTime - newRenderTime) * _horizontalScale;
			//	if (renderX < 0)
			//	{
			//		break;
			//	}
			//	var renderY = (float)(sender.ScaledSize.Height / 2 - trajectory.GetY(newRenderTime) * _verticalScale);
			//	path.LineTo(renderX, renderY);
			//	lastRenderX = renderX;
			//	lastRenderY = renderY;
			//	lastRenderTime = newRenderTime;
			//}
			//args.Canvas.DrawPath(path, paint);
		}

		private void DrawOscillationCurrentPoint(ISkiaCanvas sender, SKSurface args, float x, float y, SKPaint paint, float size = 4)
		{
			var renderX = GetRenderX(sender, x);
			var renderY = GetRenderY(sender, y);

			args.Canvas.DrawCircle(renderX, renderY, size, paint);
		}

		private float GetRenderX(ISkiaCanvas sender, float x)
		{
			var centerX = sender.ScaledSize.Width / 2;
			return centerX + x * _displayScale;
		}

		private float GetRenderY(ISkiaCanvas sender, float y)
		{
			var centerY = sender.ScaledSize.Height / 2;
			return centerY + (-y * _displayScale);
		}
	}
}
