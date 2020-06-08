using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Toolkit.Uwp.Helpers;
using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic.PhysicsServices;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Physics.HomogenousParticle.Rendering
{
    public class PerpendicularVariantRenderer : IVariantRenderer
    {
        private const int RelativeRadiusToMinDimension = 3;

        private const int InductionDistance = 40;



        private PerpendicularMotionSetup _motion;
        private PerpendicularPhysicsService _physicsService;
        private Vector2 _currentMotionPosition;
        private readonly HomogenousParticleCanvasController _controller;

        private AxesRenderer _axesRenderer = new AxesRenderer();

        public PerpendicularVariantRenderer(HomogenousParticleCanvasController controller)
        {
            _controller = controller;
        }

        public void StartSimulation(IMotionSetup[] motions)
        {
            if (motions?.Length != 1)
            {
                throw new ArgumentException("First variant supports only one motion", nameof(motions));
            }
            _motion = (PerpendicularMotionSetup)motions[0];
            _lastPosition = null;
            _physicsService = new PerpendicularPhysicsService(_motion);
            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motion.Color);
            _axesRenderer.XAxisColor = Colors.Black;
            _axesRenderer.YAxisColor = Colors.Black;
        }

        private Vector2? _lastPosition = null;

        private Vector2[] _currentMotionPositions = null;

        public void Update(ICanvasAnimatedControl sender)
        {
            if (_motion != null)
            {
                var T = _physicsService.ComputeT();
                var dimension = Math.Min(sender.Size.Width, sender.Size.Height);
                var radius = dimension / RelativeRadiusToMinDimension;
                var unitRadius = (double)_physicsService.ComputeRadius();
                var relativeSize = unitRadius / radius;
                _axesRenderer.UnitDimensions = new Size(sender.Size.Width * relativeSize, sender.Size.Height * relativeSize);
                var period = 1 / _motion.VelocityMultiple;
                var relativeTimeScale = (float)T / period;
                var scaledTime = _controller.SimulationTime.TotalTime * relativeTimeScale;
                _currentMotionPosition = CalculateMotionPosition(_motion, (float)radius);
            }
        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));
            var drawingOffset = new Vector2((float)sender.Size.Width / 2f, (float)sender.Size.Height / 2f);
            if (_motion != null)
            {
                _axesRenderer.Draw(sender, args);
                DrawInductionDirection(sender, args);

                DrawTrajectory(sender, args);
                var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motion.Color);
                _controller.DrawParticle(_currentMotionPosition + drawingOffset, color, args);


                ArrowRenderer.Draw(
                    _currentMotionPosition + drawingOffset,
                    (float)Math.Min(sender.Size.Width, sender.Size.Height) / 30.0f * _motion.VelocityMultiple,
                    GetArrowAngle(_controller.SimulationTime.TotalTime.TotalSeconds),
                    color,
                    sender,
                    args.DrawingSession);
            }
        }

        private void DrawTrajectory(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var dimension = Math.Min(sender.Size.Width, sender.Size.Height);
            var radius = (float)dimension / RelativeRadiusToMinDimension;
            var angle = _motion.VelocityMultiple;

            var clockwise =
                !((_motion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper && _motion.ChargeMultiple > 0) ||
                (_motion.InductionOrientation == PerpendicularInductionOrientation.FromPaper && _motion.ChargeMultiple < 0));
            Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder builder = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(sender);

            var center = new Vector2((float)sender.Size.Width / 2f, (float)sender.Size.Height / 2f);
            var offset = new Vector2(0, (clockwise ? 1 : 1) * (float)ComputeSimulationY(radius, angle, 0));
            builder.BeginFigure(center + offset);
            builder.AddArc(new Vector2((float)sender.Size.Width / 2f, (float)sender.Size.Height / 2f), radius, radius, (clockwise ? -1 : 1) * (float)Math.PI / 2, (clockwise ? 1 : -1) * angle * (float)_controller.SimulationTime.TotalTime.TotalSeconds);
            builder.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Open);
            var arcGeometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(builder);
            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motion.Color);
            args.DrawingSession.DrawGeometry(arcGeometry, color);
        }

        private void DrawInductionDirection(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            if (_motion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper)
            {
                DrawInductionGrid(sender, args, DrawInductionCross);
            }
            else
            {
                DrawInductionGrid(sender, args, DrawInductionDot);
            }
        }

        private void DrawInductionGrid(
            ICanvasAnimatedControl sender,
            CanvasAnimatedDrawEventArgs args,
            Action<ICanvasAnimatedControl, CanvasAnimatedDrawEventArgs, Vector2, Color> drawPoint)
        {
            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motion.Color);
            for (var y = InductionDistance / 2; y < sender.Size.Height; y += InductionDistance)
            {
                for (int x = InductionDistance / 2; x < sender.Size.Width; x += InductionDistance)
                {
                    drawPoint(sender, args, new Vector2(x, y), Colors.Gray);
                }
            }
        }

        private void DrawInductionCross(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, Vector2 point, Color color)
        {
            args.DrawingSession.DrawLine(point - new Vector2(0, 5), point + new Vector2(0, 5), color);
            args.DrawingSession.DrawLine(point - new Vector2(5, 0), point + new Vector2(5, 0), color);
        }

        private void DrawInductionDot(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, Vector2 point, Color color)
        {
            args.DrawingSession.DrawCircle(point, 2, color);
        }

        private Vector2 CalculateMotionPosition(PerpendicularMotionSetup motionSetup, float radius)
        {
            var angularVelocity = _motion.VelocityMultiple;

            var realX = ComputeSimulationX(radius, angularVelocity, _controller.SimulationTime.TotalTime.TotalSeconds);
            var realY = ComputeSimulationY(radius, angularVelocity, _controller.SimulationTime.TotalTime.TotalSeconds);

            return new Vector2((float)realX, (float)realY);
        }

        private double ComputeSimulationX(float radius, float angularVelocity, double seconds)
        {
            if ((_motion.InductionOrientation == PerpendicularInductionOrientation.FromPaper && _motion.ChargeMultiple > 0) ||
                (_motion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper && _motion.ChargeMultiple < 0))
            {
                return
                    radius *
                    Math.Cos(angularVelocity * seconds - (float)Math.PI / 2);
            }
            else
            {
                return
                    radius *
                    Math.Cos((-angularVelocity * seconds - (3 * (float)Math.PI / 2)));
            }
        }

        private float AngularVelocity => _motion.VelocityMultiple;

        private float GetArrowAngle(double seconds)
        {
            var angle = GetCurrentAngle(seconds);
            if ((_motion.InductionOrientation == PerpendicularInductionOrientation.FromPaper && _motion.ChargeMultiple > 0) ||
                (_motion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper && _motion.ChargeMultiple < 0))
            {
                return angle + (float)Math.PI / 2;
            }
            else
            {
                return angle - (float)Math.PI / 2;
            }
        }

        private float GetCurrentAngle(double seconds)
        {
            if ((_motion.InductionOrientation == PerpendicularInductionOrientation.FromPaper && _motion.ChargeMultiple > 0) ||
                (_motion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper && _motion.ChargeMultiple < 0))
            {
                return (float)(AngularVelocity * seconds - (float)Math.PI / 2);
            }
            else
            {
                return (float)(-AngularVelocity * seconds - (3 * (float)Math.PI / 2));
            }
        }

        private double ComputeSimulationY(float radius, float angularVelocity, double seconds)
        {
            if ((_motion.InductionOrientation == PerpendicularInductionOrientation.FromPaper && _motion.ChargeMultiple > 0) ||
                (_motion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper && _motion.ChargeMultiple < 0))
            {

                return
                    radius *
                    Math.Sin(angularVelocity * seconds - Math.PI / 2);
            }
            else
            {
                return
                    radius *
                    Math.Sin(-angularVelocity * seconds - (3 * Math.PI / 2));
            }
        }
    }
}
