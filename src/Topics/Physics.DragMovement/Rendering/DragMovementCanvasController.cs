using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.DragMovement.Logic.PhysicsServices;
using Physics.Shared.UI.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Physics.DragMovement.Rendering
{

    public class DragMovementCanvasController : BaseCanvasController, IDisposable
    {
        private const int BallRadius = 5;
        protected MotionInfo[] _motions = Array.Empty<MotionInfo>();
        protected TrajectoryData[] _trajectories = Array.Empty<TrajectoryData>();
        protected PhysicsService[] _physicsServices = Array.Empty<PhysicsService>();

        protected virtual Vector2 XAxisOffset => Vector2.Zero;

        private int[] _allowedScaleJumps = new int[] { 1, 2, 5 };

        private bool _drawTrajectoriesContinously;
        private bool _breakDownMotions;

        private ICanvasBrush _brush;

        protected int SimulationPadding { get; set; } = 52;
        protected int SimulationLeftSidePadding { get; set; } = 80;

        protected virtual Windows.UI.Color XMeasureColor => Windows.UI.Colors.Black;
        protected virtual Windows.UI.Color YMeasureColor => Windows.UI.Colors.Black;

        //private ICanvasBrush _backgroundBrush;
        //private CanvasBitmap _skyImage;
        private readonly CanvasTextFormat _yAxisFormat = new CanvasTextFormat()
        {
            HorizontalAlignment = CanvasHorizontalAlignment.Center,
            VerticalAlignment = CanvasVerticalAlignment.Center,
            FontSize = 12
        };

        public DragMovementCanvasController(CanvasAnimatedControl canvasAnimatedControl)
            : base(canvasAnimatedControl)
        {
        }

        public override Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            _brush = new CanvasSolidColorBrush(sender, Colors.Black);
            return Task.CompletedTask;
        }

        protected RectangleF _simulationBoundsInMeters = new RectangleF();
        private RectangleF _simulationBoundsInPixels = new RectangleF();

        protected float _meterSizeInPixels = 0;

        public virtual TimeSpan? TrajectoryStopTime { get; private set; } = null;

        public void StartNewSimulation(bool drawTrajectoriesContinuously, bool breakDownMotions, params MotionInfo[] throws)
        {
            if (throws is null)
            {
                throw new ArgumentNullException(nameof(throws));
            }

            _motions = throws;
            _drawTrajectoriesContinously = drawTrajectoriesContinuously;
            _breakDownMotions = breakDownMotions;

            PrepareTrajectories();

            Reset();

            OnSimulationStarting();

            CalculateMaxima();
        }

        protected virtual void OnSimulationStarting()
        {
        }

        private void PrepareTrajectories()
        {
            var trajectories = new List<TrajectoryData>();
            var physicsServices = new List<PhysicsService>();
            foreach (var movement in _motions)
            {
                var physicsService = new PhysicsService(movement);
                physicsServices.Add(physicsService);
                trajectories.Add(physicsService.CreateTrajectoryData());
            }
            _trajectories = trajectories.ToArray();
            _physicsServices = physicsServices.ToArray();
            if (_physicsServices.Count() > 0)
            {
                TrajectoryStopTime = TimeSpan.FromSeconds(_physicsServices.Max(s => s.MaxT));
            }
        }

        protected virtual void CalculateMaxima()
        {
            var minX = 0f;
            var maxX = 0f;
            foreach (var trajectory in _trajectories)
            {
                maxX = Math.Max(trajectory.MaxX, maxX);
            }
            foreach (var motion in _motions)
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
            var totalSeconds = (float)SimulationTime.TotalTime.TotalSeconds;

            UpdatePadding(sender);

            _simulationBoundsInPixels = new RectangleF(
                SimulationLeftSidePadding,
                SimulationPadding,
                (float)sender.Size.Width - SimulationLeftSidePadding - SimulationPadding,
                (float)sender.Size.Height - SimulationPadding * 2);
            var verticalMeterInPixels = CalculateRequiredMeterSize(_simulationBoundsInMeters.Height, _simulationBoundsInPixels.Height);
            if (Math.Abs(_simulationBoundsInMeters.Width) > 0.01)
            {
                var horizontalMeterInPixels = CalculateRequiredMeterSize(_simulationBoundsInMeters.Width, _simulationBoundsInPixels.Width);
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
            if (realMeters == 0)
            {
                return 0;
            }
            return (float)pixels / realMeters;
        }

        public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = CanvasAntialiasing.Antialiased;

            DrawBackground(sender, args);

            DrawYMeasure(sender, args);
            DrawXMeasure(sender, args);


            if (_trajectories.Length > 0)
            {
                DrawTrajectories(sender, args);
            }

            DrawOverlay(sender, args);
        }

        protected virtual void DrawBackground(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));
		}

        protected virtual void DrawOverlay(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {

        }

        private void DrawTrajectories(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            for (int movementId = 0; movementId < _trajectories.Length; movementId++)
            {
                var trajectory = _trajectories[movementId];
                var movement = _motions[movementId];
                var movementColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(movement.Color);
                DrawTrajectory(args, movementId, trajectory, movementColor);
            }
        }

        private void DrawTrajectory(CanvasAnimatedDrawEventArgs args, int movementId, TrajectoryData trajectory, Windows.UI.Color movementColor)
        {
            var startPoint = trajectory.Points.First();
            var lastPoint = trajectory.Points.First();

            var minX = startPoint.X;
            var maxX = startPoint.X;
            var maxY = startPoint.Y;
            var minY = startPoint.Y;

            foreach (var point in trajectory.Points.Where(p => TrajectoryStopTime == null || p.Time <= TrajectoryStopTime.Value))
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

                minX = Math.Min(minX, currentPoint.X);
                maxX = Math.Max(maxX, currentPoint.X);
                minY = Math.Min(minY, currentPoint.Y);
                maxY = Math.Max(maxY, currentPoint.Y);

                //draw
                args.DrawingSession.DrawLine(
                    new Vector2(MetersToPixelsX(lastPoint.X), MetersToPixelsY(lastPoint.Y)),
                    new Vector2(MetersToPixelsX(currentPoint.X), MetersToPixelsY(currentPoint.Y)),
                    movementColor, 2);

                lastPoint = currentPoint;
                if (shouldEnd)
                {
                    break;
                }
            }

            var ballX = MetersToPixelsX(lastPoint.X);
            var ballY = MetersToPixelsY(lastPoint.Y); //ball reference point is its horizontal center and vertical bottom                

            if (_breakDownMotions)
            {
                var semiTransparentColor = Windows.UI.Color.FromArgb(150, movementColor.R, movementColor.G, movementColor.B);
                args.DrawingSession.DrawLine(
                    new Vector2(MetersToPixelsX(minX), MetersToPixelsY(startPoint.Y)), new Vector2(MetersToPixelsX(maxX), MetersToPixelsY(startPoint.Y)), semiTransparentColor);
                args.DrawingSession.DrawLine(
                    new Vector2(MetersToPixelsX(startPoint.X), MetersToPixelsY(minY)), new Vector2(MetersToPixelsX(startPoint.X), MetersToPixelsY(maxY)), semiTransparentColor);

                args.DrawingSession.DrawCircle(new Vector2(MetersToPixelsX(startPoint.X), ballY), BallRadius, semiTransparentColor);
                args.DrawingSession.DrawCircle(new Vector2(ballX, MetersToPixelsY(startPoint.Y)), BallRadius, semiTransparentColor);
            }

            //draw ball            
            DrawBall(args, new Vector2(ballX, ballY), movementColor);
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
            drawing.DrawLine(_simulationBoundsInPixels.Left, _simulationBoundsInPixels.Bottom, _simulationBoundsInPixels.Left, _simulationBoundsInPixels.Top, YMeasureColor);

            var jumpSize = CalculateOptimalJumpSize(_simulationBoundsInMeters.Height, _simulationBoundsInMeters.Width);
            var jumps = (float)Math.Ceiling(_simulationBoundsInMeters.Height / jumpSize);
            var meters = jumps * jumpSize;
            for (float currentHeight = jumpSize; meters - currentHeight > -0.01; currentHeight += jumpSize)
            {
                drawing.DrawLine(
                    SimulationLeftSidePadding - 3,
                    _simulationBoundsInPixels.Bottom - _meterSizeInPixels * currentHeight,
                    SimulationLeftSidePadding + 3,
                    _simulationBoundsInPixels.Bottom - _meterSizeInPixels * currentHeight,
                    YMeasureColor);

                drawing.DrawText(
                    currentHeight.ToString("0.#"),
                    new Rect(
                        0,
                        _simulationBoundsInPixels.Bottom - _meterSizeInPixels * currentHeight - 50,
                        SimulationLeftSidePadding,
                        100),
                    YMeasureColor,
                    _yAxisFormat);
            }
        }

        private void DrawXMeasure(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var drawing = args.DrawingSession;
            drawing.DrawLine(_simulationBoundsInPixels.Left, _simulationBoundsInPixels.Bottom + XAxisOffset.Y, _simulationBoundsInPixels.Right, _simulationBoundsInPixels.Bottom + XAxisOffset.Y, XMeasureColor);

            var jumpSize = CalculateOptimalJumpSize(_simulationBoundsInMeters.Width, _simulationBoundsInMeters.Height);
            var jumps = (float)Math.Ceiling(_simulationBoundsInMeters.Width / jumpSize);
            var meters = jumps * jumpSize;
            for (float currentDistance = 0; meters - currentDistance > -0.01 || (jumps == 0 && currentDistance == 0); currentDistance += jumpSize * 2)
            {
                drawing.DrawLine(
                    _simulationBoundsInPixels.Left + _meterSizeInPixels * currentDistance,
                    _simulationBoundsInPixels.Bottom - 3 + XAxisOffset.Y,
                    _simulationBoundsInPixels.Left + _meterSizeInPixels * currentDistance,
                    _simulationBoundsInPixels.Bottom + 3 + XAxisOffset.Y,
                    XMeasureColor);

                drawing.DrawText(
                    (currentDistance + _simulationBoundsInMeters.Left).ToString("0.#"),
                    _simulationBoundsInPixels.Left + _meterSizeInPixels * currentDistance,
                    _simulationBoundsInPixels.Bottom + 12 + XAxisOffset.Y,
                    XMeasureColor,
                    _yAxisFormat);
            }
        }

        private float CalculateOptimalJumpSize(float requestedAxis, float otherAxis)
        {
            var requestedJump = CalculateJumpSizeForAxis(requestedAxis);
            var otherJump = CalculateJumpSizeForAxis(otherAxis);

            if (Math.Max(requestedAxis, otherJump) / Math.Min(requestedAxis, otherJump) > 5)
            {
                //too big difference between axis, draw independently
                return requestedJump;
            }
            return Math.Min(requestedJump, otherJump);
        }

        private float CalculateJumpSizeForAxis(float range)
        {
            if (range == 0)
            {
                return 1f;
            }
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
                return 5f;
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
