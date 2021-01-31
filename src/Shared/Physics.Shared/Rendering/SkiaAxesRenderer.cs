using System;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.Shared.UI.Rendering
{
	public class SkiaAxesRenderer : ISkiaRenderer
	{
		private const int TickSize = 3;
		private const int VerticalTextDistance = 9;
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

		public bool ShouldDrawXAxis { get; set; } = true;

		public bool ShouldDrawYAxis { get; set; } = true;

		public bool ShouldDrawXMeasure { get; set; } = true;

		public bool ShouldDrawYMeasure { get; set; } = true;

		public SimulationBounds TargetBounds { get; set; } = SimulationBounds.Empty;

		public double XAxisPositionViewportPosition { get; set; } = 0;

		public SKPaint XMeasurePaint { get; set; } = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1,
			Color = SKColors.Black
		};

		public SKPaint YMeasurePaint { get; set; } = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1,
			Color = SKColors.Black
		};

		public float YUnitSizeInPixels { get; set; }

		public float XUnitSizeInPixels { get; set; }

		public void Update(ISkiaCanvas sender)
		{
		}

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (ShouldDrawXAxis)
			{
				DrawXAxis(sender, args);
			}

			if (ShouldDrawXMeasure)
			{
				DrawXMeasure(sender, args);
			}

			if (ShouldDrawYAxis)
			{
				DrawYAxis(sender, args);
			}

			if (ShouldDrawYMeasure)
			{
				DrawYMeasure(sender, args, 1);
				DrawYMeasure(sender, args, -1);
			}
		}

		private void DrawXAxis(ISkiaCanvas sender, SKSurface args)
		{
			var drawing = args.Canvas;

			var y = GetZeroHeight();
			drawing.DrawLine(TargetBounds.Left, y, TargetBounds.Right, y, XMeasurePaint);
		}

		private void DrawYAxis(ISkiaCanvas sender, SKSurface args)
		{
			var drawing = args.Canvas;
			drawing.DrawLine(TargetBounds.Left, TargetBounds.Bottom, TargetBounds.Left, TargetBounds.Top, YMeasurePaint);
		}

		private void DrawXMeasure(ISkiaCanvas sender, SKSurface args)
		{
			var drawing = args.Canvas;

			var axisInfo = GetXAxisInfo();
			
			for (float currentDistance = 0; axisInfo.Units - currentDistance > -0.01 || (axisInfo.Jumps == 0 && currentDistance == 0); currentDistance += axisInfo.JumpSize)
			{
				drawing.DrawLine(
					TargetBounds.Left + XUnitSizeInPixels * currentDistance,
					TargetBounds.Bottom - TickSize,
					TargetBounds.Left + XUnitSizeInPixels * currentDistance,
					TargetBounds.Bottom + TickSize,
					XMeasurePaint);

				drawing.DrawText(
					(currentDistance).ToString("0.#"),
					TargetBounds.Left + XUnitSizeInPixels * currentDistance,
					TargetBounds.Bottom + TickSize + VerticalTextDistance,
					XMeasurePaint);
			}
		}

		private void DrawYMeasure(ISkiaCanvas sender, SKSurface args, double direction)
		{
			var drawing = args.Canvas;

			var axisInfo = GetYAxisInfo();

			var zeroHeight = GetZeroHeight();

			for (float currentHeight = axisInfo.JumpSize; axisInfo.Units - currentHeight > -0.01; currentHeight += axisInfo.JumpSize)
			{
				drawing.DrawLine(
					TargetBounds.Left - TickSize,
					TargetBounds.Bottom + zeroHeight - YUnitSizeInPixels * currentHeight,
					TargetBounds.Left + TickSize,
					TargetBounds.Bottom + zeroHeight - YUnitSizeInPixels * currentHeight,
					YMeasurePaint);

				drawing.DrawText(
					currentHeight.ToString("0.#"),
					TargetBounds.Left,
					TargetBounds.Bottom + zeroHeight - YUnitSizeInPixels * currentHeight - 50,
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

			var jumpSize = CalculateOptimalJumpSize(axisInUnits, maxTicks);
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

		private float GetZeroHeight()
		{
			var y = (float)(TargetBounds.Bottom + XAxisPositionViewportPosition * (TargetBounds.Top - TargetBounds.Bottom));
			return y;
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
			if (range <= 1)
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
