using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RadiationHalflife.Rendering
{
	public class BeamActivityRenderer : IVariantRenderer
	{
		private float _displayScale = 1f;
		private float _smallerDimension = 0f;
		private float _padding = 1f;
		private float _centerY = -1f;

		private SKPaint _ballFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
			Color = SKColors.Gray
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
			TextAlign = SKTextAlign.Right
		};

		private SKPaint _graphStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 3,
			FilterQuality = SKFilterQuality.High,
			Color = SKColors.Orange
		};


		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			DrawAces(sender, args);
			DrawLabels(sender, args);
			PlotGraph(sender, args);
			args.Canvas.DrawCircle(_lastPoint, 5, _ballFillPaint);
		}
		public void Update(ISkiaCanvas sender)
		{
			_centerY = (int)(sender.ScaledSize.Height / 2);
			//smallerDimension so that resizing works
			_smallerDimension = Math.Min(sender.ScaledSize.Width, sender.ScaledSize.Height);
			_padding = _smallerDimension * 0.12f;
			_displayScale = (float)_smallerDimension - 2 * _padding;
		}
		public void Dispose() { }

		private void DrawAces(ISkiaCanvas sender, SKSurface args)
		{
			//args.Canvas.DrawCircle();
			var paddingAroundCenter = _centerY * 0.1f;
			var graphBottomY = _padding + (sender.ScaledSize.Height - 2 * _padding) * 0.9f;
			var graphTopY = _padding + (sender.ScaledSize.Height - 2 * _padding) * 0.1f;
			var graphHeight = graphBottomY - graphTopY;
			//Vertical
			//args.Canvas.DrawLine(_padding, _centerY + graphHeight / 2, _padding, _centerY + graphHeight/2 - graphHeight, _axisStrokePaint);
			args.Canvas.DrawLine(_padding, graphBottomY, _padding, graphTopY, _axisStrokePaint);
			//Horizontal
			args.Canvas.DrawLine(_padding, graphBottomY, sender.ScaledSize.Width - _padding, graphBottomY, _axisStrokePaint);
		}

		private void DrawLabels(ISkiaCanvas sender, SKSurface args)
		{
			var paddingAroundCenter = _centerY * 0.1f;

			//Helpers
			var graphBottomY = _padding + (sender.ScaledSize.Height - 2 * _padding) * 0.9f;
			var graphTopY = _padding + (sender.ScaledSize.Height - 2 * _padding) * 0.1f;
			var graphHeight = graphBottomY - graphTopY;

			//Y-axis
			var a0 = PhysicsService.Animation.Activity;
			var part = a0 / 12;
			for (int divider = 0; divider < 13; divider++)
			{
				var formattedA0Divided = "";
				if (PhysicsService.Animation.ChemicalElement == "Uranium238")
				{
					formattedA0Divided = ((12 - divider) * part).ToString("0.##");
				}
				else
				{
					formattedA0Divided = ((12 - divider) * part).ToString("0.00");
				}
				args.Canvas.DrawText($"{formattedA0Divided}", new SKPoint(_padding - 5f, graphTopY + (graphHeight / 12) * divider), _axisLabelPaint);
			}

			//X-axis (0 already drawn above)
			for (int i = 1; i <= 6; i++)
			{
				string formattedHalflifeNumber = (i * PhysicsService.Animation.Halflife).ToString("0.##");
				string formattedHalflifeUnit = Localizer.Instance[PhysicsService.Animation.ChemicalElement + "_Halflife"];
				formattedHalflifeUnit = formattedHalflifeUnit.ToLowerInvariant();
				var formattedHalflifeForAxis = $"{formattedHalflifeNumber} {formattedHalflifeUnit}";
				args.Canvas.DrawText($"{formattedHalflifeForAxis}", new SKPoint(_padding + (i * ((sender.ScaledSize.Width - _padding - _padding) / 6)), graphBottomY + _axisLabelPaint.MeasureText($"{i}T")), _axisLabelPaint);
			}

			var axisNamePaint = _axisLabelPaint.Clone();
			axisNamePaint.FakeBoldText = true;
			//Y axis name
			var path = new SKPath();
			path.MoveTo(new SKPoint(_padding - 55f, graphTopY + (graphHeight / 3) * 2));
			path.LineTo(new SKPoint(_padding - 55f, graphTopY + (graphHeight/3)));
			string name = Localizer.Instance["BeamActivity"];
			if (PhysicsService.Animation.Mantissa != 0)
			{
				name = name + " [Bq]" + " (10^" + PhysicsService.Animation.Mantissa + ")";
			}
			args.Canvas.DrawTextOnPath(name, path, new SKPoint(0, 0), axisNamePaint);
		}

		private void PlotGraph(ISkiaCanvas sender, SKSurface args)
		{
			var deltaX = (PhysicsService.Animation.Delta / (6 * PhysicsService.Animation.Halflife)) * (sender.ScaledSize.Width - _padding - _padding);
			var path = new SKPath();
			var firstPoint = _values[0];
			var elapsedTime = _controller.SimulationTime.TotalTime.TotalSeconds;
			double endTime = elapsedTime / 30.0 * (6 * PhysicsService.Animation.Halflife);
			path.MoveTo(new SKPoint(_padding, GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, firstPoint.A))));
			for (int i = 0; (i + 1) < _values.Count && _values[i].Time < endTime; i++)
			{
				var relativeX = deltaX * i + _padding;
				var nextPoint = _values[i + 1];
				path.LineTo(relativeX, GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, nextPoint.A)));
				_lastPoint.X = relativeX;
				_lastPoint.Y = GetGraphAbsoluteY(sender, GetGraphRelativeY(sender, nextPoint.A));
			}

			args.Canvas.DrawPath(path, _graphStrokePaint);
		}

		private float GetGraphRelativeY(ISkiaCanvas sender, float n)
		{
			var graphBottomY = _padding + (sender.ScaledSize.Height - 2 * _padding) * 0.9f;
			var graphTopY = _padding + (sender.ScaledSize.Height - 2 * _padding) * 0.1f;
			//var paddingAroundCenter = _centerY * 0.1f;
			//return (_centerY - paddingAroundCenter - _padding) * (n / (PhysicsService.Animation.Activity * 1.0f));
			return (graphBottomY - graphTopY) * (n / PhysicsService.Animation.Activity);
		}

		private float GetGraphAbsoluteY(ISkiaCanvas sender, float relativeY)
		{
			//var centerY = sender.ScaledSize.Height / 2;
			//var paddingAroundCenter = centerY * 0.1f;
			var graphBottomY = _padding + (sender.ScaledSize.Height - 2 * _padding) * 0.9f;
			//return (float)(_padding + (centerY - paddingAroundCenter - _padding) - relativeY);
			return graphBottomY - relativeY;
		}

		public void StartSimulation()
		{
			_controller.SimulationTime.Restart();
			_controller.Play();
		}

		public void SetAnimation(PhysicsService physicsService)
		{
			PhysicsService = (BeamActivityPhysicsService)physicsService;
			_values = PhysicsService.FillTable();
		}

		private SKPoint _lastPoint = new SKPoint();

		private List<(float Time, float A)> _values = new List<(float Time, float A)>();

		public RadiationHalflifeController _controller;

		public BeamActivityPhysicsService PhysicsService { get; set; }

		public BeamActivityRenderer(RadiationHalflifeController controller)
		{
			_controller = controller;
		}
	}
}
