using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace Physics.Shared.UI.Rendering
{
    public class AxesRenderer : IRenderer
    {
        private int[] _allowedScaleJumps = new int[] { 1, 2, 5 };

        private readonly CanvasTextFormat _yAxisFormat = new CanvasTextFormat()
        {
            HorizontalAlignment = CanvasHorizontalAlignment.Center,
            VerticalAlignment = CanvasVerticalAlignment.Center,
            FontSize = 12
        };

        public bool DrawXAxis { get; set; } = true;

        public bool DrawYAxis { get; set; } = true;

        public bool DrawXJumps { get; set; } = true;

        public bool DrawYJumps { get; set; } = true;

        public Color XAxisColor { get; set; }

        public Color YAxisColor { get; set; }

        public Vector2 OriginPosition { get; set; } = new Vector2(0.5f, 0.5f);

        public bool IsOriginAbsolute { get; set; } = false;

        public SimulationPadding PaddingInPixels { get; set; }

        public Size UnitDimensions { get; set; }

        private float _unitToPixels;

        public void Update(ICanvasAnimatedControl sender)
        {

        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            _unitToPixels = (float)sender.Size.Width / (float)UnitDimensions.Width;
            if (DrawXAxis)
            {
                DrawXMeasure(sender, args);
            }
            if (DrawYAxis)
            {
                DrawYMeasure(sender, args);
            }
        }

        //private float MetersToPixelsX(float meters) => BoundsInPixels.Left + (meters - BoundsInUnits.Left) * _unitToPixels;

        //private float MetersToPixelsY(float meters) => BoundsInPixels.Bottom - meters * _unitToPixels;

        private void DrawYMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var drawing = args.DrawingSession;

            var x = (float)(IsOriginAbsolute ? OriginPosition.X : sender.Size.Width * OriginPosition.X);

            drawing.DrawLine(
                x,
                (float)sender.Size.Height - PaddingInPixels.Bottom,
                x,
                PaddingInPixels.Top,
                YAxisColor);

            var jumpSize = (float)CalculateJumpSizeForAxis((float)UnitDimensions.Height);

            var originY = (float)(IsOriginAbsolute ? OriginPosition.Y : sender.Size.Height * OriginPosition.Y);
            var minY = (int)(-originY / _unitToPixels) * _unitToPixels;
            var maxY = UnitDimensions.Height;
            for (var currentHeight = minY; currentHeight <= maxY; currentHeight += jumpSize)
            {
                drawing.DrawLine(
                    x - 3,
                    OriginPosition.Y + _unitToPixels * currentHeight,
                    x + 3,
                    OriginPosition.Y + _unitToPixels * currentHeight,
                    Colors.Black);

                drawing.DrawText(
                    currentHeight.ToString("0.#"),
                    new Rect(
                        0,
                        OriginPosition.Y - _unitToPixels * currentHeight - 50,
                        x,
                        100),
                    Colors.Black,
                    _yAxisFormat);
            }

            //    var jumps = (float)Math.Ceiling(UnitDimensions.Height / jumpSize);
            //var meters = jumps * jumpSize;
            //for (float currentHeight = jumpSize; meters - currentHeight > -0.01; currentHeight += jumpSize)
            //{

            //}
        }

        private void DrawXMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var drawing = args.DrawingSession;

            var y = (float)(IsOriginAbsolute ? OriginPosition.Y : sender.Size.Height * OriginPosition.Y);

            drawing.DrawLine(
                PaddingInPixels.Left,
                y,
                (float)sender.Size.Width - PaddingInPixels.Right,
                y,
                XAxisColor);

            //var jumpSize = CalculateJumpSizeForAxis(_simulationBoundsInMeters.Width);
            //var jumps = (float)Math.Ceiling(_simulationBoundsInMeters.Width / jumpSize);
            //var meters = jumps * jumpSize;
            //for (float currentDistance = 0; meters - currentDistance > -0.01 || (jumps == 0 && currentDistance == 0); currentDistance += jumpSize)
            //{
            //    drawing.DrawLine(
            //        _controller.SimulationBoundsInPixels.Left + _meterToPixels * currentDistance,
            //        _controller.SimulationBoundsInPixels.Bottom - 3 + XAxisOffset.Y,
            //        _controller.SimulationBoundsInPixels.Left + _meterToPixels * currentDistance,
            //        _controller.SimulationBoundsInPixels.Bottom + 3 + XAxisOffset.Y,
            //        Colors.Gray);

            //    drawing.DrawText(
            //        (currentDistance + _simulationBoundsInMeters.Left).ToString("0.#"),
            //        _controller.SimulationBoundsInPixels.Left + _meterToPixels * currentDistance,
            //        _controller.SimulationBoundsInPixels.Bottom + 12 + XAxisOffset.Y,
            //        XMeasureColor,
            //        _yAxisFormat);
            //}
        }

        private double CalculateJumpSizeForAxis(float range)
        {
            if (range <= 1)
            {
                var upperBound = (float)Math.Ceiling(range);
                var currentMultiplier = 1.0;
                while (true)
                {
                    foreach (var allowedJumpSize in _allowedScaleJumps)
                    {
                        var scaleSize = allowedJumpSize * currentMultiplier;
                        if (upperBound / scaleSize > 20)
                        {
                            return scaleSize;
                        }
                    }
                    currentMultiplier /= 10;
                }
            }
            else if (range <= 2)
            {
                return 0.2f;
            }
            else if (range <= 5)
            {
                return 0.5f;
            }
            else if (range <= 10)
            {
                return 1f;
            }
            else if (range <= 20)
            {
                return 2f;
            }
            else if (range <= 50)
            {
                return 10;
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
                            return scaleSize;
                        }
                    }
                    currentMultiplier *= 10;
                }
            }
        }
    }
}
