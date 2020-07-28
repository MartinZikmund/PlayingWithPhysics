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

            for (var currentHeight = jumpSize; true; currentHeight += jumpSize)
            {
                bool drawn = false;
                DrawVerticalTick(originY + _unitToPixels * currentHeight, -currentHeight);
                DrawVerticalTick(originY - _unitToPixels * currentHeight, currentHeight);
                void DrawVerticalTick(float position, float value)
                {
                    if (position > sender.Size.Height || position < 0)
                    {
                        return;
                    }                    
                    drawing.DrawLine(
                        x - 3,
                        position,
                        x + 3,
                        position,
                        Colors.Black);
                    drawn = true;

                    drawing.DrawText(
                        value.ToString("0.#####"),
                        new Rect(
                            x - 50,
                            position - 50,
                            50,
                            100),
                        Colors.Black,
                        _yAxisFormat);
                }
                if (!drawn)
                {
                    break;
                }
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

            var jumpSize = (float)CalculateJumpSizeForAxis((float)UnitDimensions.Height);

            var originX = (float)(IsOriginAbsolute ? OriginPosition.X : sender.Size.Width * OriginPosition.X);
            var minX = (int)(-originX / _unitToPixels) * _unitToPixels;
            var maxX = UnitDimensions.Width;

            for (var currentWidth = jumpSize; true; currentWidth += jumpSize)
            {
                bool drawn = false;
                DrawHorizontalTick(originX - _unitToPixels * currentWidth, -currentWidth);
                DrawHorizontalTick(originX + _unitToPixels * currentWidth, currentWidth);
                void DrawHorizontalTick(float position, float value)
                {
                    if (position > sender.Size.Width || position < 0)
                    {
                        return;
                    }
                    drawing.DrawLine(
                        position,
                        y - 3,
                        position,
                        y + 3,
                        Colors.Black);
                    drawn = true;

                    drawing.DrawText(
                        value.ToString("0.#####"),
                        new Rect(                           
                            position - 50,
                            y + 8,
                            100,
                            20),
                        Colors.Black,
                        _yAxisFormat);
                }
                if (!drawn)
                {
                    break;
                }
            }
        }

        private double CalculateJumpSizeForAxis(float range)
        {
            if (range <= 1)
            {
                var upperBound = range;
                var currentMultiplier = 1.0;
                while (true)
                {
                    foreach (var allowedJumpSize in _allowedScaleJumps.Reverse())
                    {
                        var scaleSize = allowedJumpSize * currentMultiplier;
                        if (upperBound / scaleSize > 10)
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
