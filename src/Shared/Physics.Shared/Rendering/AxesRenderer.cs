//using Microsoft.Graphics.Canvas.Text;
//using Microsoft.Graphics.Canvas.UI.Xaml;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Foundation;
//using Windows.UI;

//namespace Physics.Shared.UI.Rendering
//{
//    public class AxesRenderer : IRenderer
//    {
//        private int[] _allowedScaleJumps = new int[] { 1, 2, 5 };

//        private readonly CanvasTextFormat _yAxisFormat = new CanvasTextFormat()
//        {
//            HorizontalAlignment = CanvasHorizontalAlignment.Center,
//            VerticalAlignment = CanvasVerticalAlignment.Center,
//            FontSize = 12
//        };

//        public bool DrawXAxis { get; set; } = true;

//        public bool DrawYAxis { get; set; } = true;

//        public bool DrawXJumps { get; set; } = true;

//        public bool DrawYJumps { get; set; } = true;

//        public Color XAxisColor { get; set; }

//        public Color YAxisColor { get; set; }

//        public Point OriginPosition { get; set; }

//        public bool IsOriginAbsolute { get; set; }

//        public SimulationBounds BoundsInPixels { get; set; }

//        public SimulationBounds BoundsInUnits { get; set; }

//        public void Update(ICanvasAnimatedControl sender)
//        {

//        }

//        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
//        {
//            if (DrawXAxis)
//            {
//                DrawXMeasure(sender, args);
//            }
//            if (DrawYAxis)
//            {
//                DrawYMeasure(sender, args);
//            }
//        }

//        private float MetersToPixelsX(float meters) => BoundsInPixels.Left + (meters - _simulationBoundsInMeters.Left) * _meterToPixels;

//        private float MetersToPixelsY(float meters) => BoundsInPixels.Bottom - meters * _meterToPixels;

//        private void DrawYMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
//        {
//            var drawing = args.DrawingSession;

//            drawing.DrawLine(
//                BoundsInPixels.Left,
//                BoundsInPixels.Bottom,
//                BoundsInPixels.Left,
//                BoundsInPixels.Top,
//                YAxisColor);

//            var jumpSize = CalculateJumpSizeForAxis(_simulationBoundsInMeters.Height);
//            var jumps = (float)Math.Ceiling(_simulationBoundsInMeters.Height / jumpSize);
//            var meters = jumps * jumpSize;
//            for (float currentHeight = jumpSize; meters - currentHeight > -0.01; currentHeight += jumpSize)
//            {
//                drawing.DrawLine(
//                    SimulationLeftSidePadding - 3,
//                    _controller.SimulationBoundsInPixels.Bottom - _meterToPixels * currentHeight,
//                    SimulationLeftSidePadding + 3,
//                    _controller.SimulationBoundsInPixels.Bottom - _meterToPixels * currentHeight,
//                    Colors.Gray);

//                drawing.DrawText(
//                    currentHeight.ToString("0.#"),
//                    new Rect(
//                        0,
//                        _controller.SimulationBoundsInPixels.Bottom - _meterToPixels * currentHeight - 50,
//                        SimulationLeftSidePadding,
//                        100),
//                    Colors.Gray,
//                    _yAxisFormat);
//            }
//        }

//        private void DrawXMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
//        {
//            var drawing = args.DrawingSession;

//            drawing.DrawLine(
//                BoundsInPixels.Left,
//                BoundsInPixels.Bottom,
//                BoundsInPixels.Right,
//                BoundsInPixels.Bottom,
//                XAxisColor);

//            var jumpSize = CalculateJumpSizeForAxis(_simulationBoundsInMeters.Width);
//            var jumps = (float)Math.Ceiling(_simulationBoundsInMeters.Width / jumpSize);
//            var meters = jumps * jumpSize;
//            for (float currentDistance = 0; meters - currentDistance > -0.01 || (jumps == 0 && currentDistance == 0); currentDistance += jumpSize)
//            {
//                drawing.DrawLine(
//                    _controller.SimulationBoundsInPixels.Left + _meterToPixels * currentDistance,
//                    _controller.SimulationBoundsInPixels.Bottom - 3 + XAxisOffset.Y,
//                    _controller.SimulationBoundsInPixels.Left + _meterToPixels * currentDistance,
//                    _controller.SimulationBoundsInPixels.Bottom + 3 + XAxisOffset.Y,
//                    Colors.Gray);

//                drawing.DrawText(
//                    (currentDistance + _simulationBoundsInMeters.Left).ToString("0.#"),
//                    _controller.SimulationBoundsInPixels.Left + _meterToPixels * currentDistance,
//                    _controller.SimulationBoundsInPixels.Bottom + 12 + XAxisOffset.Y,
//                    XMeasureColor,
//                    _yAxisFormat);
//            }
//        }

//        private float CalculateJumpSizeForAxis(float range)
//        {
//            if (range <= 1)
//            {
//                return 0.1f;
//            }
//            else if (range <= 2)
//            {
//                return 0.2f;
//            }
//            else if (range <= 5)
//            {
//                return 0.5f;
//            }
//            else if (range <= 10)
//            {
//                return 1f;
//            }
//            else if (range <= 20)
//            {
//                return 2f;
//            }
//            else if (range <= 50)
//            {
//                return 10;
//            }
//            else
//            {
//                var upperBound = (float)Math.Ceiling(range);
//                var currentMultiplier = 1;
//                while (true)
//                {
//                    foreach (var allowedJumpSize in _allowedScaleJumps)
//                    {
//                        var scaleSize = allowedJumpSize * currentMultiplier;
//                        if (upperBound / scaleSize < 20)
//                        {
//                            return scaleSize;
//                        }
//                    }
//                    currentMultiplier *= 10;
//                }
//            }
//        }
//    }
//}
