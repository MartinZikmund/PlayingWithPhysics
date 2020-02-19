using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.Shared.Rendering;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Text;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.HomogenousMovement.ViewModels;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Physics.HomogenousMovement.Rendering
{
    public class HomogenousMovementCanvasController : BaseCanvasController, IDisposable
    {
        private const int BallRadius = 5;
        private MotionInfo[] _throws = Array.Empty<MotionInfo>();
        private TrajectoryData[] _trajectories = Array.Empty<TrajectoryData>();
        private PhysicsService[] _physicsServices = Array.Empty<PhysicsService>();

        private bool _drawTrajectoriesContinously;

        private ICanvasBrush _brush;

        protected int SimulationPadding { get; set; } = 52;
        protected int SimulationLeftSidePadding { get; set; } = 80;

        //private ICanvasBrush _backgroundBrush;
        //private CanvasBitmap _skyImage;
        private readonly CanvasTextFormat _yAxisFormat = new CanvasTextFormat()
        {
            HorizontalAlignment = CanvasHorizontalAlignment.Center
        };

        public HomogenousMovementCanvasController(CanvasAnimatedControl canvasAnimatedControl)
            : base(canvasAnimatedControl)
        {
        }

        public override Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            _brush = new CanvasSolidColorBrush(sender, Colors.Black);
            return Task.CompletedTask;           
        }

        private RectangleF _simulationBoundsInMeters = new RectangleF();
        private RectangleF _simulationBoundsInPixels = new RectangleF();
        private float _meterSizeInPixels = 0;

        public void StartNewSimulation(bool drawTrajectoriesContinuously, params MotionInfo[] throws)
        {
            if (throws is null)
            {
                throw new ArgumentNullException(nameof(throws));
            }

            _throws = throws;
            _drawTrajectoriesContinously = drawTrajectoriesContinuously;

            PrepareTrajectories();

            Restart();

            CalculateMaxima();
        }

        private void PrepareTrajectories()
        {
            var trajectories = new List<TrajectoryData>();
            var physicsServices = new List<PhysicsService>();
            foreach (var movement in _throws)
            {
                var physicsService = new PhysicsService(movement);
                physicsServices.Add(physicsService);
                trajectories.Add(physicsService.CreateTrajectoryData());
            }
            _trajectories = trajectories.ToArray();
            _physicsServices = physicsServices.ToArray();
        }

        private void CalculateMaxima()
        {
            var minX = float.MaxValue;
            var maxX = 0f;
            foreach (var trajectory in _trajectories)
            {
                maxX = Math.Max(trajectory.MaxX, maxX);
            }
            foreach (var motion in _throws)
            {
                minX = Math.Min(motion.Origin.X, minX);
            }
            minX = Math.Min(minX, maxX);
            var minY = 0f;
            var maxY = 0f;
            foreach (var trajectory in _trajectories)
            {
                maxY = Math.Max(trajectory.MaxY, maxY);
            }
            _simulationBoundsInMeters = new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public override void Update(ICanvasAnimatedControl sender)
        {
            if (_trajectories.Length == 0) return;

            var totalSeconds = (float)SimulationTime.TotalTime.TotalSeconds;

            UpdatePadding(sender);

            _simulationBoundsInPixels = new RectangleF(
                SimulationLeftSidePadding,
                SimulationPadding,
                (float)sender.Size.Width - SimulationLeftSidePadding - SimulationPadding,
                (float)sender.Size.Height - SimulationPadding * 2);
            var verticalMeterInPixels =
                CalculateRequiredMeterSize(_simulationBoundsInMeters.Height, _simulationBoundsInPixels.Height);
            if (Math.Abs(_simulationBoundsInMeters.Width) > 0.01)
            {
                var horizontalMeterInPixels =
                    CalculateRequiredMeterSize(_simulationBoundsInMeters.Width, _simulationBoundsInPixels.Width);
                _meterSizeInPixels = Math.Min(verticalMeterInPixels, horizontalMeterInPixels);
            }
            else
            {
                _meterSizeInPixels = verticalMeterInPixels;
            }
        }

        protected virtual void UpdatePadding(ICanvasAnimatedControl sender)
        {            
        }

        private float CalculateRequiredMeterSize(float meters, float pixels)
        {
            var jumpSize = CalculateJumpSizeForAxis(meters);
            var jumps = (float)Math.Ceiling(meters / jumpSize);
            var realMeters = jumps * jumpSize;
            return (float)pixels / realMeters;
        }

        public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
            
            DrawBackground(sender, args);

            if (_trajectories.Length == 0) return;

            DrawYMeasure(sender, args);
            DrawXMeasure(sender, args);

            DrawTrajectories(sender, args);
        }

        protected virtual void DrawBackground(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));
        }

        private void DrawTrajectories(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            for (int movementId = 0; movementId < _trajectories.Length; movementId++)
            {                
                var trajectory = _trajectories[movementId];
                var movement = _throws[movementId];
                var movementColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(movement.Color);
                var lastPoint = trajectory.Points.First();
                foreach (var point in trajectory.Points)
                {
                    var currentPoint = point;
                    bool shouldEnd = false;

                    //change point if the next generated is after current time
                    if ((_drawTrajectoriesContinously || movementId == 0) && point.Time > SimulationTime.TotalTime)
                    {
                        //calculate last point exactly
                        var x = _physicsServices[movementId].ComputeX((float)SimulationTime.TotalTime.TotalSeconds);
                        var y = _physicsServices[movementId].ComputeY((float)SimulationTime.TotalTime.TotalSeconds);
                        currentPoint = new TrajectoryPoint(SimulationTime.TotalTime, x, y);
                        shouldEnd = true;
                    }

                    //draw
                    args.DrawingSession.DrawLine(
                        new Vector2(MetersToPixelsX(lastPoint.X), MetersToPixelsY(lastPoint.Y) - BallRadius),
                        new Vector2(MetersToPixelsX(currentPoint.X), MetersToPixelsY(currentPoint.Y) - BallRadius),
                        movementColor, 2);

                    lastPoint = currentPoint;
                    if (shouldEnd)
                    {
                        break;
                    }
                }
                //draw ball
                var ballX = MetersToPixelsX(lastPoint.X);
                var ballY = MetersToPixelsY(lastPoint.Y) - BallRadius; //ball reference point is its horizontal center and vertical bottom
                DrawBall(args, new Vector2(ballX, ballY), movementColor);                
            }
        }

        protected virtual void DrawBall(CanvasAnimatedDrawEventArgs args, Vector2 centerPoint, Windows.UI.Color movementColor)
        {
            args.DrawingSession.FillCircle(centerPoint, BallRadius, movementColor);
        }

        private float MetersToPixelsX(float meters) => _simulationBoundsInPixels.X + (meters - _simulationBoundsInMeters.Left) * _meterSizeInPixels;

        private float MetersToPixelsY(float meters) => _simulationBoundsInPixels.Bottom - meters * _meterSizeInPixels;

        private void DrawYMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var drawing = args.DrawingSession;
            var jumpSize = CalculateJumpSizeForAxis(_simulationBoundsInMeters.Height);
            var jumps = (float)Math.Ceiling(_simulationBoundsInMeters.Height / jumpSize);
            var meters = jumps * jumpSize;
            for (float currentHeight = jumpSize; meters - currentHeight > -0.01; currentHeight += jumpSize)
            {
                drawing.DrawText(
                    currentHeight.ToString("0.#"),
                    new Rect(
                        0,
                        _simulationBoundsInPixels.Bottom - _meterSizeInPixels * currentHeight,
                        SimulationLeftSidePadding,
                        100),
                    Colors.Black,
                    _yAxisFormat);
            }
        }

        private void DrawXMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var drawing = args.DrawingSession;
            drawing.DrawLine(_simulationBoundsInPixels.Left, _simulationBoundsInPixels.Bottom, _simulationBoundsInPixels.Right, _simulationBoundsInPixels.Bottom, _brush);
            var jumpSize = CalculateJumpSizeForAxis(_simulationBoundsInMeters.Width);
            var jumps = (float)Math.Ceiling(_simulationBoundsInMeters.Width / jumpSize);
            var meters = jumps * jumpSize;
            for (float currentDistance = 0; meters - currentDistance > -0.01; currentDistance += jumpSize)
            {
                drawing.DrawText(
                    (currentDistance + _simulationBoundsInMeters.Left).ToString("0.#"),
                    _simulationBoundsInPixels.Left + _meterSizeInPixels * currentDistance,
                    _simulationBoundsInPixels.Bottom + 12,
                    Colors.Black);
            }
        }

        private float CalculateJumpSizeForAxis(float range)
        {
            if (range <= 1)
            {
                return 0.1f;
            }
            else if (range <= 2)
            {
                return 0.2f;
            }
            else if (range <= 5)
            {
                return 0.5f;
            }
            else
            {
                var upperBound = (float)Math.Ceiling(range / 10f);
                return upperBound;
            }
        }
    }
}
