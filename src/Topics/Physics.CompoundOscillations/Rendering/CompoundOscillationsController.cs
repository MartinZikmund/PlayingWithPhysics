using System;
using System.Collections.Generic;
using System.Linq;
using Physics.CompoundOscillations.Logic;
using Physics.CompoundOscillations.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.CompoundOscillations.Rendering
{
	public class CompoundOscillationsController : SkiaCanvasController
	{
		private const float VerticalPadding = 12;

		private float _verticalScale = 1f;
		private float _horizontalScale = 300f;

		private OscillationInfoViewModel[] _activeOscillations;
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
		private float _maxY;
		private float _minY;
		private double _inherentSlowdown = 1.0;

		public CompoundOscillationsController(ISkiaCanvas canvasAnimatedControl) :
			base(canvasAnimatedControl)
		{
		}

		public void SetActiveOscillations(OscillationInfoViewModel[] info)
		{
			_activeOscillations = info;
			_oscillationTrajectories = new Dictionary<OscillationInfo, OscillationTrajectory>();
			_oscillationFillPaints = new Dictionary<OscillationInfo, SKPaint>();
			_oscillationStrokePaints = new Dictionary<OscillationInfo, SKPaint>();
			_maxY = 0;
			_minY = 0;
			foreach (var oscillation in _activeOscillations)
			{
				var physicsService = new OscillationPhysicsService(oscillation.OscillationInfo);

				_maxY += physicsService.MaxY;
				_minY += physicsService.MinY;

				_oscillationTrajectories.Add(oscillation.OscillationInfo, physicsService.CreateTrajectoryData());

				var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(oscillation.OscillationInfo.Color);
				var skColor = new SKColor(color.R, color.G, color.B);
				var fillPaint = new SKPaint()
				{
					Color = skColor,
					IsStroke = false,
					IsAntialias = true,
				};
				_oscillationFillPaints.Add(oscillation.OscillationInfo, fillPaint);
				var strokePaint = new SKPaint()
				{
					Color = skColor,
					IsStroke = true,
					StrokeWidth = 2,
					IsAntialias = true,
				};
				_oscillationStrokePaints.Add(oscillation.OscillationInfo, strokePaint);

				_inherentSlowdown = 1 / Math.Log(_activeOscillations.Max(a => a.OscillationInfo.Frequency), 10);
				if (_inherentSlowdown > 1)
				{
					_inherentSlowdown = 1;
				}
			}

			_compoundOscillationsPhysicsService = new CompoundOscillationsPhysicsService(_activeOscillations.Select(oscillation => oscillation.OscillationInfo).ToArray());
			_compoundOscillationTrajectory = new CompoundOscillationTrajectory(_oscillationTrajectories.Select(t => t.Value).ToArray());
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
			foreach (var oscillation in _activeOscillations)
			{				
				var trajectory = _oscillationTrajectories[oscillation.OscillationInfo];
				if (oscillation.IsEnabled)
				{
					var y = trajectory.GetY(currentTime);
					totalValue += y;

					if (oscillation.IsVisible)
					{
						var strokePain = _oscillationStrokePaints[oscillation.OscillationInfo];
						DrawTrajectory(sender, args, trajectory, strokePain);
						var fillPaint = _oscillationFillPaints[oscillation.OscillationInfo];
						DrawOscillationCurrentPoint(sender, args, y, fillPaint);
					}
				}
			}

			if (_activeOscillations.Length > 1)
			{
				DrawTrajectory(sender, args, _compoundOscillationTrajectory, _compoundStrokePaint);
				DrawOscillationCurrentPoint(sender, args, totalValue, _compoundFillPaint);
			}
		}

		private float GetAdjustedTotalTime() => (float)SimulationTime.TotalTime.TotalSeconds * (float)_inherentSlowdown;


		private void DrawTrajectory(ISkiaCanvas sender, SKSurface args, IOscillationTrajectory trajectory, SKPaint paint)
		{
			//Try rendering using https://github.com/PeterWaher/IoTGateway/blob/master/Script/Waher.Script.Graphs/Functions/Plots/Plot2DCurve.cs

			using SKPath path = new SKPath();
			float lastRenderTime = GetAdjustedTotalTime();
			float endRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			var endTime = GetAdjustedTotalTime();
			float lastRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
			float lastRenderY = GetRenderY(trajectory.GetY(GetAdjustedTotalTime()));
			path.MoveTo(lastRenderX, lastRenderY);
			while (lastRenderTime > 0)
			{
				var newRenderTime = lastRenderTime - 0.01f;
				if (newRenderTime < 0)
				{
					break;
				}
				var renderX = endRenderX - (endTime - newRenderTime) * _horizontalScale;
				if (renderX < 0)
				{
					break;
				}
				var renderY = (float)(sender.ScaledSize.Height / 2 - trajectory.GetY(newRenderTime) * _verticalScale);
				path.LineTo(renderX, renderY);
				lastRenderX = renderX;
				lastRenderY = renderY;
				lastRenderTime = newRenderTime;
			}
			args.Canvas.DrawPath(path, paint);
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
