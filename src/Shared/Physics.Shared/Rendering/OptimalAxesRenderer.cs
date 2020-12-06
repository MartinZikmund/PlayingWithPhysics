using System;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;

namespace Physics.Shared.UI.Rendering
{
	public class OptimalAxesRenderer : IRenderer
	{
		private const int TickSize = 3;
		private const int VerticalTextDistance = 9;
		private const int MinTickDistanceInPixels = 72;

		private static int[] _allowedScaleJumps = new int[] { 1, 2, 5 };

		private readonly CanvasTextFormat _axisTextFormat = new CanvasTextFormat()
		{
			HorizontalAlignment = CanvasHorizontalAlignment.Center,
			VerticalAlignment = CanvasVerticalAlignment.Center,
			FontSize = 12
		};

		public OptimalAxesRenderer()
		{
		}

		public bool ShouldDrawXAxis { get; set; } = true;

		public bool ShouldDrawYAxis { get; set; } = true;

		public bool ShouldDrawXMeasure { get; set; } = true;

		public bool ShouldDrawYMeasure { get; set; } = true;

		public SimulationBounds TargetBounds { get; set; } = SimulationBounds.Empty;

		public Windows.UI.Color XMeasureColor { get; set; } = Windows.UI.Colors.Black;

		public Windows.UI.Color YMeasureColor { get; set; } = Windows.UI.Colors.Black;

		public float MeterSizeInPixels { get; set; }

		public void Update(ICanvasAnimatedControl sender)
		{
		}

		public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
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
				DrawYMeasure(sender, args);
			}
		}

		private void DrawXAxis(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			var drawing = args.DrawingSession;
			drawing.DrawLine(TargetBounds.Left, TargetBounds.Bottom, TargetBounds.Right, TargetBounds.Bottom, XMeasureColor);
		}

		private void DrawYAxis(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			var drawing = args.DrawingSession;
			drawing.DrawLine(TargetBounds.Left, TargetBounds.Bottom, TargetBounds.Left, TargetBounds.Top, YMeasureColor);
		}

		private void DrawXMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			var drawing = args.DrawingSession;

			var maxTicks = GetMaxNumberOfTicks(TargetBounds.Width);

			var axisInMeters = TargetBounds.Width / MeterSizeInPixels;

			var jumpSize = CalculateOptimalJumpSize(axisInMeters, maxTicks);
			var jumps = (float)axisInMeters / jumpSize;

			var meters = jumps * jumpSize;
			for (float currentDistance = 0; meters - currentDistance > -0.01 || (jumps == 0 && currentDistance == 0); currentDistance += jumpSize)
			{
				drawing.DrawLine(
					TargetBounds.Left + MeterSizeInPixels * currentDistance,
					TargetBounds.Bottom - TickSize,
					TargetBounds.Left + MeterSizeInPixels * currentDistance,
					TargetBounds.Bottom + TickSize,
					XMeasureColor);

				drawing.DrawText(
					(currentDistance).ToString("0.#"),
					TargetBounds.Left + MeterSizeInPixels * currentDistance,
					TargetBounds.Bottom + TickSize + VerticalTextDistance,
					XMeasureColor,
					_axisTextFormat);
			}
		}

		private void DrawYMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			var drawing = args.DrawingSession;

			var maxTicks = GetMaxNumberOfTicks(TargetBounds.Height);

			var axisInMeters = TargetBounds.Height / MeterSizeInPixels;

			var jumpSize = CalculateOptimalJumpSize(axisInMeters, maxTicks);
			var jumps = (float)axisInMeters / jumpSize;

			var meters = jumps * jumpSize;
			for (float currentHeight = jumpSize; meters - currentHeight > -0.01; currentHeight += jumpSize)
			{
				drawing.DrawLine(
					TargetBounds.Left - TickSize,
					TargetBounds.Bottom - MeterSizeInPixels * currentHeight,
					TargetBounds.Left + TickSize,
					TargetBounds.Bottom - MeterSizeInPixels * currentHeight,
					YMeasureColor);

				drawing.DrawText(
					currentHeight.ToString("0.#"),
					new Rect(
						0,
						TargetBounds.Bottom - MeterSizeInPixels * currentHeight - 50,
						TargetBounds.Left,
						100),
					YMeasureColor,
					_axisTextFormat);
			}
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
