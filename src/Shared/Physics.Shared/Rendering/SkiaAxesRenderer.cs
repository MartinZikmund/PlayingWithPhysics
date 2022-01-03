using System;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.Shared.UI.Rendering
{
	public class SkiaAxesRenderer : ISkiaRenderer
	{
		private const int TickSize = 4;
		private const int MinTickDistanceInPixels = 72;

		private static int[] _allowedScaleJumps = new int[] { 1, 2, 5 };

		//private readonly CanvasTextFormat _axisTextFormat = new CanvasTextFormat()
		//{
		//	HorizontalAlignment = CanvasHorizontalAlignment.Center,
		//	VerticalAlignment = CanvasVerticalAlignment.Center,
		//	FontSize = 12
		//};

		public SkiaAxesRenderer()
		{
		}

		public SKPoint OriginUnitCoordinates { get; set; } = SKPoint.Empty;

		public bool ShouldDrawXAxis { get; set; } = true;

		public bool ShouldDrawYAxis { get; set; } = true;

		public bool ShouldDrawXMeasure { get; set; } = true;

		public bool ShouldDrawYMeasure { get; set; } = true;

		public int YAxisLabelPadding { get; set; } = 4;

		public int XAxisLabelPadding { get; set; } = 14;

		public SimulationBounds TargetBounds { get; set; } = SimulationBounds.Empty;

		/// <summary>
		/// Sets the relative position of the axes origin.
		/// 0,0 is left bottom corner of the rendering view.
		/// </summary>
		public SKPoint OriginRelativePosition { get; set; } = SKPoint.Empty;

		public SKPaint XMeasurePaint { get; set; } = new SKPaint()
		{
			Color = SKColors.Black
		};

		public SKPaint YMeasurePaint { get; set; } = new SKPaint()
		{
			Color = SKColors.Black
		};

		public float YUnitSizeInPixels { get; set; }

		public float XUnitSizeInPixels { get; set; }

		public string XUnitFormatString { get; set; } = "0.#";

		public float? XJumpSize { get; set; }

		public void Update(ISkiaCanvas sender)
		{
		}

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (ShouldDrawXAxis)
			{
				DrawXAxis(sender, args);
			}

			if (ShouldDrawYAxis)
			{
				DrawYAxis(sender, args);
			}

			if (ShouldDrawXMeasure)
			{
				if (OriginRelativePosition.X != 0)
				{
					DrawXMeasure(sender, args, -1);
				}
				if (OriginRelativePosition.X != 1)
				{
					DrawXMeasure(sender, args, 1);
				}
			}

			if (ShouldDrawYMeasure)
			{
				if (OriginRelativePosition.Y != 0)
				{
					DrawYMeasure(sender, args, -1);
				}
				if (OriginRelativePosition.Y != 1)
				{
					DrawYMeasure(sender, args, 1);
				}
			}
		}

		private void DrawXAxis(ISkiaCanvas sender, SKSurface args)
		{
			var drawing = args.Canvas;

			var renderOrigin = GetRenderOrigin();
			drawing.DrawLine(TargetBounds.Left, renderOrigin.Y, TargetBounds.Right, renderOrigin.Y, XMeasurePaint);
		}

		private void DrawYAxis(ISkiaCanvas sender, SKSurface args)
		{
			var drawing = args.Canvas;
			drawing.DrawLine(TargetBounds.Left, TargetBounds.Bottom, TargetBounds.Left, TargetBounds.Top, YMeasurePaint);
		}

		private void DrawXMeasure(ISkiaCanvas sender, SKSurface args, int direction)
		{
			var drawing = args.Canvas;

			//var axisInfo = GetXAxisInfo();

			//var renderOrigin = GetRenderOrigin();

			//for (float currentDistance = 0; axisInfo.Units - currentDistance > -0.01 || (axisInfo.Jumps == 0 && currentDistance == 0); currentDistance += axisInfo.JumpSize)
			//{
			//	drawing.DrawLine(
			//		TargetBounds.Left + XUnitSizeInPixels * currentDistance,
			//		renderOrigin.Y - TickSize,
			//		TargetBounds.Left + XUnitSizeInPixels * currentDistance,
			//		renderOrigin.Y + TickSize,
			//		XMeasurePaint);

			//	drawing.DrawText(
			//		(currentDistance).ToString("0.#"),
			//		TargetBounds.Left + XUnitSizeInPixels * currentDistance,
			//		renderOrigin.Y + TickSize + VerticalTextDistance,
			//		XMeasurePaint);
			//}

			var axisInfo = GetXAxisInfo();

			var renderOrigin = GetRenderOrigin();

			var unitOrigin = OriginUnitCoordinates;

			var firstTickX = ((int)(unitOrigin.X / axisInfo.JumpSize)) * axisInfo.JumpSize;

			for (float currentWidth = firstTickX; ; currentWidth += direction * axisInfo.JumpSize)
			{
				var renderX = renderOrigin.X + (currentWidth - unitOrigin.X) * XUnitSizeInPixels;

				if (renderX < TargetBounds.Left && direction == 1)
				{
					continue;
				}
				if (renderX > TargetBounds.Right && direction == 1)
				{
					break;
				}

				if (renderX < TargetBounds.Left && direction == -1)
				{
					break;
				}
				if (renderX > TargetBounds.Right && direction == -1)
				{
					continue;
				}

				drawing.DrawLine(
					renderX,
					renderOrigin.Y - TickSize,
					renderX,
					renderOrigin.Y + TickSize,
					YMeasurePaint);

				var tickLabel = currentWidth.ToString(XUnitFormatString);
				var textSize = XMeasurePaint.MeasureText(tickLabel);
				drawing.DrawText(
					tickLabel,
					renderX - textSize / 2,
					renderOrigin.Y + TickSize + XAxisLabelPadding,
					XMeasurePaint);
			}
		}

		private void DrawYMeasure(ISkiaCanvas sender, SKSurface args, int direction)
		{
			var drawing = args.Canvas;

			var axisInfo = GetYAxisInfo();

			var renderOrigin = GetRenderOrigin();

			var unitOrigin = OriginUnitCoordinates;

			var firstTickY = ((int)(unitOrigin.Y / axisInfo.JumpSize) + direction) * axisInfo.JumpSize;

			for (float currentHeight = firstTickY; ; currentHeight += direction * axisInfo.JumpSize)
			{
				var renderY = renderOrigin.Y - (currentHeight - unitOrigin.Y) * YUnitSizeInPixels;

				if (renderY < TargetBounds.Top || renderY > TargetBounds.Bottom)
				{
					break;
				}

				drawing.DrawLine(
					TargetBounds.Left - TickSize,
					renderY,
					TargetBounds.Left + TickSize,
					renderY,
					YMeasurePaint);

				var tickLabel = currentHeight.ToString("0.#");
				var textSize = YMeasurePaint.MeasureText(tickLabel);
				drawing.DrawText(
					tickLabel,
					TargetBounds.Left - TickSize - YAxisLabelPadding - textSize,
					renderY,
					YMeasurePaint);
			}
		}

		private AxisInfo GetYAxisInfo()
		{
			var maxTicks = GetMaxNumberOfTicks(TargetBounds.Height);
			var axisInUnits = TargetBounds.Height / YUnitSizeInPixels;

			var jumpSize = CalculateOptimalJumpSize(axisInUnits, maxTicks);
			var jumps = (float)(axisInUnits / jumpSize);

			var units = jumps * jumpSize;

			return new AxisInfo()
			{
				AxisInUnits = axisInUnits,
				JumpSize = jumpSize,
				Jumps = jumps,
				Units = units
			};
		}

		private AxisInfo GetXAxisInfo()
		{
			var maxTicks = GetMaxNumberOfTicks(TargetBounds.Width);

			var axisInUnits = TargetBounds.Width / XUnitSizeInPixels;

			var jumpSize = XJumpSize ?? CalculateOptimalJumpSize(axisInUnits, maxTicks);
			var jumps = (float)axisInUnits / jumpSize;

			var units = jumps * jumpSize;

			return new AxisInfo()
			{
				AxisInUnits = axisInUnits,
				JumpSize = jumpSize,
				Jumps = jumps,
				Units = units
			};
		}

		private SKPoint GetRenderOrigin()
		{
			var x = (float)(TargetBounds.Left + OriginRelativePosition.X * (TargetBounds.Right - TargetBounds.Left));
			var y = (float)(TargetBounds.Bottom + OriginRelativePosition.Y * (TargetBounds.Top - TargetBounds.Bottom));
			return new SKPoint(x, y);
		}

		private float CalculateOptimalJumpSize(float requestedAxis, int maxNumberOfTicks)
		{
			var requestedJump = CalculateTickDistance(requestedAxis);

			//too big difference between axis, draw independently
			IncreaseJumpSizeToMaxTicks(requestedAxis, requestedJump, maxNumberOfTicks);
			return requestedJump.Value;
		}

		private float CalculateOptimalJumpSize(float requestedAxis, float otherAxis, int maxNumberOfTicks)
		{
			var requestedJump = CalculateTickDistance(requestedAxis);
			var otherJump = CalculateTickDistance(otherAxis);

			if (Math.Max(requestedJump.Value, otherJump.Value) / Math.Min(requestedJump.Value, otherJump.Value) > 5)
			{
				//too big difference between axis, draw independently
				IncreaseJumpSizeToMaxTicks(requestedAxis, requestedJump, maxNumberOfTicks);
				return requestedJump.Value;
			}
			var result = requestedJump.Value < otherJump.Value ? requestedJump : otherJump;
			IncreaseJumpSizeToMaxTicks(requestedAxis, result, maxNumberOfTicks);
			return result.Value;
		}

		private void IncreaseJumpSizeToMaxTicks(float totalSizeInMeters, TickDistance tickDistance, int maxNumberOfTicks)
		{
			while (totalSizeInMeters / tickDistance.Value > maxNumberOfTicks)
			{
				tickDistance.Increment();
			}
		}

		public static TickDistance CalculateTickDistance(float range)
		{
			if (range == 0)
			{
				return new TickDistance(1, 1f);
			}
			else if (range <= 0.0001)
			{
				return new TickDistance(1, 0.00001f);
			}
			else if (range <= 0.001)
			{
				return new TickDistance(1, 0.0001f);
			}
			else if (range <= 0.01)
			{
				return new TickDistance(1, 0.001f);
			}
			else if (range <= 0.1)
			{
				return new TickDistance(1, 0.01f);
			}
			else if (range <= 1)
			{
				return new TickDistance(1, 0.1f);
			}
			else if (range <= 2)
			{
				return new TickDistance(2, 0.1f);
			}
			else if (range <= 5)
			{
				return new TickDistance(5, 0.1f);
			}
			else if (range <= 10)
			{
				return new TickDistance(1, 1f);
			}
			else if (range <= 20)
			{
				return new TickDistance(2, 1f);
			}
			else if (range <= 50)
			{
				return new TickDistance(5, 1f);
			}
			else
			{
				var upperBound = (float)Math.Ceiling(range);
				var currentMultiplier = 1;
				while (true)
				{
					foreach (var allowedJumpSize in _allowedScaleJumps)
					{
						var scaleSize = allowedJumpSize * currentMultiplier;
						if (upperBound / scaleSize < 20)
						{
							return new TickDistance(allowedJumpSize, currentMultiplier);
						}
					}
					currentMultiplier *= 10;
				}
			}
		}

		private int GetMaxNumberOfTicks(float pixelDimension)
		{
			return (int)(pixelDimension / MinTickDistanceInPixels);
		}
	}
}
