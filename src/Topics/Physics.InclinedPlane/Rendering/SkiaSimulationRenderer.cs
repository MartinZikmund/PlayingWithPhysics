using MvvmCross.Base;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Physics.InclinedPlane.Rendering
{
    public class SkiaSimulationRenderer : ISkiaVariantRenderer
    {
        private bool _isDisposed = false;
        private readonly InclinedPlaneSkiaController _canvasController;
        private SimulationBounds _simulationBounds;
        private float _scalingRatio;
        private float _padding = 12;

        private SKPaint _linePaint = new SKPaint()
        {
            Color = SKColors.Black,
            StrokeWidth = 2,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        private SKPaint _boxPaint = new SKPaint()
        {
            Color = SKColors.Black,
            StrokeWidth = 2,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public SkiaSimulationRenderer(InclinedPlaneSkiaController canvasController)
        {
            _canvasController = canvasController;
        }

        public void Draw(SkiaCanvas sender, SKSurface args)
        {
            if (_scalingRatio < 0 || _isDisposed)
            {
                return;
            }

            var imageInfo = new SKImageInfo((int)(sender.ScaledSize.Width - 2 * _padding), (int)(sender.ScaledSize.Height - _padding), SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            using (var surface = SKSurface.Create(imageInfo))
            {
                var canvas = surface.Canvas;
                canvas.Clear(new SKColor(255, 244, 244, 244));
                DrawFloor(imageInfo, surface);
                DrawInclinedPlane(imageInfo, surface);
                DrawObject(imageInfo, surface);

                args.Canvas.DrawSurface(surface, new SKPoint(_padding, _padding), _linePaint);
            }
        }

        private void DrawObject(SKImageInfo imageInfo, SKSurface surface)
        {
            var t = (float)_canvasController.SimulationTime.TotalTime.TotalSeconds;
            var x = _canvasController.PhysicsService.CalculateX(t) * _scalingRatio;
            var y = _canvasController.PhysicsService.CalculateY(t) * _scalingRatio;

            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_canvasController.Motion.Color);
            _boxPaint.Color = new SKColor(color.R, color.G, color.B);
            if (t <= _canvasController.PhysicsService.CalculateHorizontalStartTime() || !_canvasController.Motion.HasHorizontal)
            {
                //var leftTop = new SKPoint(-12, 0);
                //var rightBottom = new SKPoint(12, 24);
                //var matrix =
                //    SKMatrix.Concat(
                //    SKMatrix.CreateRotation(MathHelpers.DegreesToRadians(180 + _canvasController.Motion.InclinedAngle)),
                //    SKMatrix.CreateTranslation(PadX(x), FlipY(imageInfo, y)));
                float rad = MathHelpers.DegreesToRadians(_canvasController.Motion.InclinedAngle);
                using (SKPath pathRotated = new SKPath())
                {
                    var centerPoint = new SKPoint(PadX(x) + (float)Math.Cos(rad - Math.PI / 2) * 12, FlipY(imageInfo, y) + (float)Math.Sin(rad - Math.PI / 2) * 12);
                    int cx = (int)centerPoint.X;
                    int cy = (int)centerPoint.Y;
                    pathRotated.MoveTo(SkiaHelpers.RotatePoint(centerPoint - new SKPoint(12,12), cx, cy, rad));
                    pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint - new SKPoint(12, -12), cx, cy, rad));
                    pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint + new SKPoint(12, 12), cx, cy, rad));
                    pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint + new SKPoint(12, -12), cx, cy, rad));
                    pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint - new SKPoint(12, 12), cx, cy, rad));
                    surface.Canvas.DrawPath(pathRotated, _boxPaint);
                }
            }
            else
            {
                var left = PadX(x - 12);
                var top = FlipY(imageInfo, y + 24);
                surface.Canvas.DrawRect(new SKRect(left, top, left + 24, top + 24), _boxPaint);
            }
        }

        private float PadX(float x) => x + _padding;

        private float FlipY(SKImageInfo target, float y)
        {
            return (float)target.Size.Height - y - _padding;
        }

        public void Update(SkiaCanvas surface)
        {
            var reducedSize = surface.ScaledSize.ReduceBy(_padding * 2);
            if (reducedSize.Height < 50 || reducedSize.Width < 50)
            {               
                _scalingRatio = -1;
                return;
            }

            var physicsService = _canvasController.PhysicsService;
            var minX = 0;
            var maxX = physicsService.CalculateTotalWidth();
            var minY = 0;
            var maxY = physicsService.CalculateY(0);

            if (_canvasController.Motion.HorizontalLength == 0)
            {
                _padding = 60;
            }
            else
            {
                _padding = 20;
            }

            _simulationBounds = new SimulationBounds(minX, maxY, maxX, minY);
            _scalingRatio = _simulationBounds.Size.ScaleToFit(surface.ScaledSize.ReduceBy(_padding * 2));
        }

        private void DrawInclinedPlane(SKImageInfo info, SKSurface surface)
        {
            var motion = _canvasController.Motion;
            var angleInRad = MathHelpers.DegreesToRadians(motion.InclinedAngle);
            var y = (float)motion.InclinedLength * (float)Math.Sin(angleInRad);
            var x = motion.InclinedLength * Math.Cos(angleInRad);
            surface.Canvas.DrawLine(
                new SKPoint(PadX(0), FlipY(info, (float)y * _scalingRatio)),
                new SKPoint(PadX((float)x * _scalingRatio), FlipY(info, 0)),
                _linePaint);
        }

        private void DrawFloor(SKImageInfo info, SKSurface surface)
        {
            if (_canvasController.Motion.HasHorizontal)
            {
                surface.Canvas.DrawLine(
                    new SKPoint(
                        PadX(_canvasController.PhysicsService.CalculateHorizontalStartX() * _scalingRatio),
                        FlipY(info, 0)),
                    new SKPoint(
                        PadX((float)info.Size.Width - _padding * 2),
                        FlipY(info, 0)),
                    _linePaint);
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            _boxPaint?.Dispose();
        }
    }
}
