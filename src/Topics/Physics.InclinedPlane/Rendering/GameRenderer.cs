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
        private bool _isDisposed = false;

        private InclinedPlaneSkiaController _canvasController;

        private SKBitmap _stadiumBackground;
        private SKBitmap _ramp;
        private SKBitmap _tvBox;
        private SKBitmap _floor;
        private SKBitmap _stoneRamp;
        private SKBitmap _stoneFlat;

        private float _renderingScale;

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
                    sender.ScaledSize.Width / 2 - backgroundSize.Width / 2,
                    sender.ScaledSize.Height / 2 - rampSize.Height / 2,
                    sender.ScaledSize.Width / 2 - backgroundSize.Width / 2 + rampSize.Width,
                    sender.ScaledSize.Height / 2 + rampSize.Height / 2));
        }

        private float CalculateRenderingScale(SkiaCanvas sender)
        {
            return sender.ScaledSize.Width / 1920;
        }

        public void Update(SkiaCanvas sender)
        {
            EnsureBitmaps();
            _renderingScale = CalculateRenderingScale(sender);
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
                _stoneRamp = SKBitmap.Decode(Path.Combine(gameAssetsPath, "kamen_1.png"));
                _stoneFlat = SKBitmap.Decode(Path.Combine(gameAssetsPath, "kamen_2.png"));
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
        }
    }
}
