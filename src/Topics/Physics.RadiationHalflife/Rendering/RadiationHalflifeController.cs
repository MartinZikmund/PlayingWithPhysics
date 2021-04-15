using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RadiationHalflife.Rendering
{
	public class RadiationHalflifeController : SkiaCanvasController
	{
		private float _displayScale = 1f;
		private float _smallerDimension = 0f;
		private float _padding = 1f;
		private List<(float X, float Y)> _values = new List<(float x, float y)>();
		private SKPoint _lastPoint = new SKPoint();
		public PhysicsService PhysicsService { get; set; }

		private int _centerY = -1;

		private SKPaint _ballFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
			Color = SKColors.Gray
		};

		private SKPaint _graphStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 3,
			FilterQuality = SKFilterQuality.High,
			Color = SKColors.Orange
		};

		private SKPaint _axisStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			Color = SKColors.Gray
		};

		private SKPaint _axisLabelPaint = new SKPaint()
		{
			TextSize = 16f,
			IsAntialias = true,
			Color = SKColors.Gray,
			IsStroke = false,
			TextAlign = SKTextAlign.Center
		};



		public void SetAnimation(PhysicsService physicsService)
		{
			PhysicsService = physicsService;
			_values = PhysicsService.FillTablePrecise();
		}

		public void StartSimulation()
		{
			SimulationTime.Restart();
			Play();
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			DrawAces(sender, args);
			PlotGraph(sender, args);
			DrawLabels(sender, args);
			//var elapsedTime = SimulationTime.TotalTime.TotalSeconds;
			//double endTime = elapsedTime / 30.0 * (6 * PhysicsService.Animation.Halflife);
			//var n = _values.FirstOrDefault(i => i.Y > (float)endTime);
			args.Canvas.DrawCircle(_lastPoint, 5, _ballFillPaint);

		}
		public override void Update(ISkiaCanvas sender)
		{
			_centerY = (int)(sender.ScaledSize.Height / 2);
			//smallerDimension so that resizing works
			_smallerDimension = Math.Min(sender.ScaledSize.Width, sender.ScaledSize.Height);
			_padding = _smallerDimension * 0.1f;
			_displayScale = (float)_smallerDimension - 2 * _padding;
		}

		private void DrawLabels(ISkiaCanvas sender, SKSurface args)
		{
			var paddingAroundCenter = _centerY * 0.1f;
			//Y-axis
			
			args.Canvas.DrawText("1", new SKPoint(_padding - (_padding / 8), _padding + (_padding / 6)), _axisLabelPaint);
			args.Canvas.DrawText("0.5", new SKPoint(_padding - (_padding / 8), _centerY / 2 + _axisLabelPaint.MeasureText("0.5")), _axisLabelPaint);
			args.Canvas.DrawText("0", new SKPoint(_padding - (_padding / 8), _centerY - paddingAroundCenter), _axisLabelPaint);
			//X-axis (0 already drawn above)
			for (int i = 1; i <=6; i++)
			{
				args.Canvas.DrawText($"{i}T", new SKPoint(_padding+(i*((sender.ScaledSize.Width - _padding - _padding)/6)), _centerY - paddingAroundCenter + _axisLabelPaint.MeasureText($"{i}T")), _axisLabelPaint);
			}
			//args.Canvas.DrawText(_padding - (_padding/2), _padding, _padding, _centerY - paddingAroundCenter, _fillPaint);
		}

		private void DrawAces(ISkiaCanvas sender, SKSurface args)
		{
			//args.Canvas.DrawCircle();
			var paddingAroundCenter = _centerY * 0.1f;
			//Vertical
			args.Canvas.DrawLine(_padding, _padding, _padding, _centerY - paddingAroundCenter, _axisStrokePaint);
			//Horizontal
			args.Canvas.DrawLine(_padding, _centerY - paddingAroundCenter, sender.ScaledSize.Width - _padding, _centerY - paddingAroundCenter, _axisStrokePaint);
		}

		private void PlotGraph(ISkiaCanvas sender, SKSurface args)
		{
			var deltaX = (PhysicsService.Animation.Delta / (6 * PhysicsService.Animation.Halflife))*(sender.ScaledSize.Width - _padding - _padding);
			var path = new SKPath();
			var firstPoint = _values[0];
			var elapsedTime = SimulationTime.TotalTime.TotalSeconds;
			double endTime = elapsedTime / 30.0 * (6 * PhysicsService.Animation.Halflife);
			path.MoveTo(new SKPoint(_padding, GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, firstPoint.Y))));
			for (int i = 0; (i + 1) < _values.Count && _values[i].X < endTime; i++)
			{
				var relativeX = deltaX * i+ _padding;
				var nextPoint = _values[i + 1];
				path.LineTo(relativeX, GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, nextPoint.Y)));
				_lastPoint.X = relativeX;
				_lastPoint.Y = GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, nextPoint.Y));
			}
			args.Canvas.DrawPath(path, _graphStrokePaint);
		}

		private float GetGraphRelativeY(ISkiaCanvas sender, float n)
		{
			var paddingAroundCenter = _centerY * 0.1f;
			return (_centerY - paddingAroundCenter - _padding) * (n / (PhysicsService.Animation.ParticleCount * 1.0f));
		}

		private float GetGraphAbsoluteY(ISkiaCanvas sender, float relativeY)
		{
			var centerY = sender.ScaledSize.Height / 2;
			var paddingAroundCenter = centerY * 0.1f;
			return (float)(_padding + (centerY - paddingAroundCenter - _padding) - relativeY);
		}

		private int GetGraphRelativeX(ISkiaCanvas sender, float relativeTime)
		{
			return (int)((sender.ScaledSize.Width - _padding - _padding) * relativeTime);
		}

		private int GetGraphAbsoluteX(ISkiaCanvas sender, int relativeY)
		{
			var centerY = sender.ScaledSize.Height / 2;
			var paddingAroundCenter = centerY * 0.1f;
			return (int)(_padding + (centerY - paddingAroundCenter - _padding) - relativeY);
		}

		public RadiationHalflifeController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
			_centerY = (int)(canvasAnimatedControl.ScaledSize.Height / 2);
		}
	}
}
