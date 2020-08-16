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

        public SkiaSimulationRenderer(InclinedPlaneSkiaController canvasController)
        {
            _canvasController = canvasController;
        }

        public void Draw(SkiaCanvas sender, SKSurface args)
        {
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

            if (t <= _canvasController.PhysicsService.CalculateHorizontalStartTime() || !_canvasController.Motion.HasHorizontal)
            {
                var leftTop = new SKPoint(-12, 0);
                var rightBottom = new SKPoint(12, 24);
                var matrix =
                    SKMatrix.Concat(
                    SKMatrix.CreateRotation(MathHelpers.DegreesToRadians(180 + _canvasController.Motion.InclinedAngle)),
                    SKMatrix.CreateTranslation(PadX(x), FlipY(imageInfo, y)));
                surface.Canvas.DrawPoints(SKPointMode.Polygon, new SKPoint[] { matrix.MapPoint(leftTop), matrix.MapPoint(rightBottom) }, _linePaint);
                //    .CreateRectangle(target, new Rect(-12, 0, 24, 24))
                //        .Transform(Matrix3x2.CreateRotation(MathHelpers.DegreesToRadians(180 + _canvasController.Motion.InclinedAngle)))
                //        .Transform(Matrix3x2.CreateTranslation(PadX(x), FlipY(target, y)));
                ////apply angle                
                //surface.Canvas.Draw.FillGeometry(rectangle, color);
            }
            else
            {
                var left = PadX(x - 12);
                var top = FlipY(imageInfo, y + 24);
                surface.Canvas.DrawRect(new SKRect(left, top, left + 24, top + 24), _linePaint);
            }
        }

        private float PadX(float x) => x + _padding;

        private float FlipY(SKImageInfo target, float y)
        {
            return (float)target.Size.Height - y - _padding;
        }

        public void Update(SkiaCanvas surface)
        {
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
    }
}
