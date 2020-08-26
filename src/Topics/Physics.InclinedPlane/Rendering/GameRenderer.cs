using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Physics.InclinedPlane.Rendering
{
    public class GameRenderer : ISkiaVariantRenderer
    {
        private const float InclinedPlaneLengthInPixels = 413;
        private const float HorizontalPlaneLengthInPixels = 910;
        private const float MeterToPixelRatio = 22;

        private bool _isDisposed = false;

        private InclinedPlaneSkiaController _canvasController;

        private SKBitmap _stadiumBackground;
        private SKBitmap _ramp;
        private SKBitmap _tvBox;
        private SKBitmap _floor;
        private SKBitmap _stoneRamp;
        private SKBitmap _stoneFlat;
        private SKBitmap _target;

        private SKPaint _linePaint = new SKPaint()
        {
            Color = SKColors.Black,
            StrokeWidth = 2,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };        

        private float _renderingScale;
        private float _meterToPixelRatio;

        private float _gameLeft;
        private float _gameTop;

        public GameRenderer(InclinedPlaneSkiaController canvasController)
        {
            _canvasController = canvasController;
        }

        public void Draw(SkiaCanvas sender, SKSurface args)
        {
            if (_isDisposed)
            {
                return;
            }
            args.Canvas.Clear(SKColors.Black);
            DrawBackground(sender, args);
            DrawFloor(sender, args);
            DrawTvBox(sender, args);
            DrawRamp(sender, args);
            DrawTarget(sender, args);
            if (_canvasController.Motion != null)
            {
                DrawSituation(sender, args);
            }
        }

        private void DrawBackground(SkiaCanvas sender, SKSurface args)
        {
            var backgroundSize = new SKSize(_stadiumBackground.Width * _renderingScale, _stadiumBackground.Height * _renderingScale);
            args.Canvas.DrawBitmap(
                _stadiumBackground,
                new SKRect(
                    sender.ScaledSize.Width / 2 - backgroundSize.Width / 2,
                    sender.ScaledSize.Height / 2 - backgroundSize.Height / 2,
                    sender.ScaledSize.Width / 2 + backgroundSize.Width / 2,
                    sender.ScaledSize.Height / 2 + backgroundSize.Height / 2));
        }

        private void DrawFloor(SkiaCanvas sender, SKSurface args)
        {
            var backgroundSize = new SKSize(_floor.Width * _renderingScale, _floor.Height * _renderingScale);

            args.Canvas.DrawBitmap(
                _floor,
                new SKRect(
                    sender.ScaledSize.Width / 2 - backgroundSize.Width / 2,
                    sender.ScaledSize.Height / 2 - backgroundSize.Height / 2,
                    sender.ScaledSize.Width / 2 + backgroundSize.Width / 2,
                    sender.ScaledSize.Height / 2 + backgroundSize.Height / 2));
        }

        private void DrawTvBox(SkiaCanvas sender, SKSurface args)
        {
            var tvBoxSize = new SKSize(_tvBox.Width * _renderingScale, _tvBox.Height * _renderingScale);
            var backgroundSize = new SKSize(_stadiumBackground.Width * _renderingScale, _stadiumBackground.Height * _renderingScale);

            args.Canvas.DrawBitmap(
                _tvBox,
                new SKRect(
                    sender.ScaledSize.Width / 2 - tvBoxSize.Width / 2,
                    sender.ScaledSize.Height / 2 - backgroundSize.Height / 2,
                    sender.ScaledSize.Width / 2 + tvBoxSize.Width / 2,
                    sender.ScaledSize.Height / 2 - backgroundSize.Height / 2 + tvBoxSize.Height));
        }

        private void DrawRamp(SkiaCanvas sender, SKSurface args)
        {
            var rampSize = new SKSize(_ramp.Width * _renderingScale, _ramp.Height * _renderingScale);
            var backgroundSize = new SKSize(_stadiumBackground.Width * _renderingScale, _stadiumBackground.Height * _renderingScale);

            args.Canvas.DrawBitmap(
                _ramp,
                new SKRect(
                    130 * _renderingScale,
                    _gameTop + 540 * _renderingScale,
                    130 * _renderingScale + rampSize.Width,
                    _gameTop + 540 * _renderingScale + rampSize.Height));
        }

        private void DrawTarget(SkiaCanvas sender, SKSurface args)
        {
            var targetSize = new SKSize(_target.Width * _renderingScale, _target.Height * _renderingScale);

            args.Canvas.DrawBitmap(
                _target,
                new SKRect(
                    1450 * _renderingScale - targetSize.Width / 2,
                    GetHorizontalPlaneY() + 5 * _renderingScale - targetSize.Height / 2,
                    1450 * _renderingScale + targetSize.Width / 2,
                    GetHorizontalPlaneY() + 5 * _renderingScale + targetSize.Height / 2));
        }

        private float GetHorizontalPlaneY() => _gameTop + 830 * _renderingScale;

        private void DrawSituation(SkiaCanvas sender, SKSurface args)
        {
            DrawHorizontalPlane(sender, args);
            DrawInclinedPlane(sender, args);
            DrawObject(sender, args);
        }

        private void DrawHorizontalPlane(SkiaCanvas sender, SKSurface surface)
        {
            surface.Canvas.DrawLine(
                new SKPoint(
                    GetHorizontalPlaneStartRenderX(),
                    GetHorizontalPlaneY()),
                new SKPoint(
                    GetHorizontalPlaneEndRenderX(),
                    GetHorizontalPlaneY()),
                _linePaint);
        }

        private void DrawInclinedPlane(SkiaCanvas sender, SKSurface surface)
        {
            var angleInRad = Shared.Helpers.MathHelpers.DegreesToRadians(30);
            var y = (float)(InclinedPlaneLengthInPixels * _renderingScale * (float)Math.Sin(angleInRad));
            var x = (float)(InclinedPlaneLengthInPixels * _renderingScale * Math.Cos(angleInRad));
            surface.Canvas.DrawLine(
                new SKPoint(185 * _renderingScale, GetHorizontalPlaneY() - y),
                new SKPoint(185 * _renderingScale + x, GetHorizontalPlaneY()),
                _linePaint);
        }

        private void DrawObject(SkiaCanvas sender, SKSurface surface)
        {
            var t = (float)_canvasController.SimulationTime.TotalTime.TotalSeconds;
            t = Math.Min(t, _canvasController.PhysicsService.CalculateMaxT());

            if (t <= _canvasController.PhysicsService.CalculateHorizontalStartTime())
            {
                var stoneRampSize = new SKSize(_stoneRamp.Width * _renderingScale, _stoneRamp.Height * _renderingScale);
                
                var x = _canvasController.PhysicsService.CalculateX(t) * _meterToPixelRatio * _renderingScale;
                var y = _canvasController.PhysicsService.CalculateY(t) * _meterToPixelRatio * _renderingScale;

                surface.Canvas.DrawBitmap(
                    _stoneRamp,
                    new SKRect(
                        x - stoneRampSize.Width / 2,
                        GetHorizontalPlaneY() - y - stoneRampSize.Height / 2,
                        x + stoneRampSize.Width / 2,
                        GetHorizontalPlaneY() - y + stoneRampSize.Height / 2));
            }
            else
            {
                var horizontalT = t - _canvasController.PhysicsService.CalculateHorizontalStartTime();

                var x = _canvasController.PhysicsService.CalculateHorizontalX(horizontalT) * _meterToPixelRatio * _renderingScale;

                var stoneFlatSize = new SKSize(_stoneFlat.Width * _renderingScale, _stoneFlat.Height * _renderingScale);

                surface.Canvas.DrawBitmap(
                    _stoneFlat,
                    new SKRect(
                        GetHorizontalPlaneStartRenderX() + x - stoneFlatSize.Width / 2,
                        GetHorizontalPlaneY() - 18f * _renderingScale - stoneFlatSize.Height / 2,
                        GetHorizontalPlaneStartRenderX() + x + stoneFlatSize.Width / 2,
                        GetHorizontalPlaneY() - 18f * _renderingScale + stoneFlatSize.Height / 2));
            }
        }

        private float CalculateRenderingScale(SkiaCanvas sender)
        {
            return sender.ScaledSize.Width / 1920;
        }

        private float GetHorizontalPlaneStartRenderX()
        {
            return GetHorizontalPlaneStartPixelX() * _renderingScale;
        }

        private float GetHorizontalPlaneEndRenderX()
        {
            return GetHorizontalPlaneEndPixelX() * _renderingScale;
        }

        private float GetHorizontalPlaneStartPixelX()
        {
            return 540;
        }

        private float GetHorizontalPlaneEndPixelX()
        {
            return 1700;
        }

        public void Update(SkiaCanvas sender)
        {
            EnsureBitmaps();
            _renderingScale = CalculateRenderingScale(sender);
            _meterToPixelRatio = 32.09f;
            _gameLeft = 0;
            _gameTop = sender.ScaledSize.Height / 2 - (1080 / 2) * _renderingScale;
        }

        private void EnsureBitmaps()
        {
            if (_stadiumBackground == null)
            {
                var gameAssetsPath = Path.Combine(Package.Current.InstalledPath, "Assets", "Game");
                _stadiumBackground = SKBitmap.Decode(Path.Combine(gameAssetsPath, "pozadi_stadion.png"));
                _ramp = SKBitmap.Decode(Path.Combine(gameAssetsPath, "rampa.png"));
                _tvBox = SKBitmap.Decode(Path.Combine(gameAssetsPath, "tv_kostka.png"));
                _floor = SKBitmap.Decode(Path.Combine(gameAssetsPath, "plocha.png"));
                _stoneRamp = SKBitmap.Decode(Path.Combine(gameAssetsPath, "kamen_2.png"));
                _stoneFlat = SKBitmap.Decode(Path.Combine(gameAssetsPath, "kamen_1.png"));
                _target = SKBitmap.Decode(Path.Combine(gameAssetsPath, "kolo.png"));
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            _floor?.Dispose();
            _ramp?.Dispose();
            _stadiumBackground?.Dispose();
            _stoneFlat?.Dispose();
            _stoneRamp?.Dispose();
            _tvBox?.Dispose();
            _target?.Dispose();
        }
    }
}
