using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.Graphics.Canvas.Effects;
using Physics.HomogenousMovement.Gamification;
using Physics.Shared.Helpers;
using Color = Windows.UI.Color;

namespace Physics.HomogenousMovement.Rendering
{
    public class GamificationCanvasController : HomogenousMovementCanvasController
    {
        private CanvasBitmap _skyImage;
        private CanvasBitmap _groundImage;
        private CanvasBitmap _grassImage;
        private CanvasBitmap _ballImage;
        private CanvasBitmap _cannonImage;
        private CanvasBitmap _castleImage;
        private CanvasBitmap _castleKoImage;
        private CanvasBitmap _wallImage;
        private CanvasBitmap _wallKoImage;

        private CanvasBitmap[] _treeImages;

        private GameSetup _game = null;

        public GamificationCanvasController(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public async Task StartNewGameAsync(GameSetup game)
        {
            await RunOnGameLoopAsync(() =>
            {
                _game = game;
                _simulationBoundsInMeters = new RectangleF(0, 0, _game.CastleDistance * 1.2f, 30);
            });
        }

        public override async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            await base.CreateResourcesAsync(sender);
            _skyImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/nebe.png"));
            _groundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/puda.png"));
            _grassImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/travnik.png"));
            _ballImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/koule.png"));
            _cannonImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/delo.png"));
            _castleImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/hrad.png"));
            _castleKoImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/hrad-ko.png"));
            _wallImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/hradby.png"));
            _wallKoImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/hradby-ko.png"));

            _treeImages = new[]
            {
                await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/strom_1.png")),
                await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/strom_2.png")),
                await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/strom_3.png"))
            };
        }

        protected override void UpdatePadding(ICanvasAnimatedControl sender)
        {

        }

        protected override void DrawBall(CanvasAnimatedDrawEventArgs args, Vector2 centerPoint, Color movementColor)
        {
            args.DrawingSession.DrawImage(_ballImage, centerPoint.X - (float)_ballImage.Size.Width / 2, centerPoint.Y - (float)_ballImage.Size.Height / 2);
        }

        protected override void DrawBackground(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(_skyImage, new Rect(0, 0, sender.Size.Width, sender.Size.Height));
            args.DrawingSession.DrawImage(_groundImage, new Rect(0, sender.Size.Height - SimulationPadding, sender.Size.Width, SimulationPadding));
            args.DrawingSession.DrawImage(_grassImage, new Rect(0, sender.Size.Height - SimulationPadding - 10, sender.Size.Width, 10));
            if (_game != null)
            {
                DrawTrees(sender, args);
                DrawCannon(sender, args, -45);
                DrawCastle(sender, args);
                DrawWall(sender, args);
            }
        }

        private void DrawTrees(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            for (int treeId = 0; treeId < _game.TreeDistances.Length; treeId++)
            {
                var distance = _game.TreeDistances[treeId];
                var originalImage = _treeImages[treeId % _treeImages.Length];
                var scaling = (float) (_meterSizeInPixels * 4f / originalImage.Size.Width);
                var scaledImage = new ScaleEffect()
                {
                    Source = originalImage,
                    Scale = new Vector2(scaling)
                };
                args.DrawingSession.DrawImage(
                    scaledImage,
                    SimulationLeftSidePadding + distance * _meterSizeInPixels,
                    (float)sender.Size.Height - SimulationPadding - scaling * (float)originalImage.Size.Height,
                    new Rect(0, 0, originalImage.Size.Width, originalImage.Size.Height),
                    1,
                    CanvasImageInterpolation.Cubic);
            }
        }

        private void DrawCastle(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scaledImage = new ScaleEffect()
            {
                Source = _castleImage,
                Scale = new Vector2((float)(_meterSizeInPixels * 10f / _castleImage.Size.Width))
            };
            args.DrawingSession.DrawImage(
                scaledImage,
                SimulationLeftSidePadding + _game.CastleDistance * _meterSizeInPixels,
                (float)sender.Size.Height - SimulationPadding - (float)(_meterSizeInPixels * 10f / _castleImage.Size.Width) * (float)_castleImage.Size.Height,
                new Rect(0, 0, _castleImage.Size.Width, _castleImage.Size.Height),
                1,
                CanvasImageInterpolation.Cubic);
        }

        private void DrawWall(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scaledImage = new ScaleEffect()
            {
                Source = _wallImage,
                Scale = new Vector2((float)(_meterSizeInPixels * 5f / _wallImage.Size.Width))
            };
            args.DrawingSession.DrawImage(
                scaledImage,
                SimulationLeftSidePadding + _game.WallDistance * _meterSizeInPixels,
                (float)sender.Size.Height - SimulationPadding - (float)(_meterSizeInPixels * 5f / _wallImage.Size.Width) * (float)_wallImage.Size.Height,
                new Rect(0, 0, _wallImage.Size.Width, _wallImage.Size.Height),
                1,
                CanvasImageInterpolation.Cubic);
        }

        private void DrawCannon(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, float angle)
        {
            var scaledImage = new ScaleEffect()
            {
                Source = _cannonImage,
                Scale = new Vector2((float)(_meterSizeInPixels * 4f / _cannonImage.Size.Width))
            };
            args.DrawingSession.DrawImage(
                scaledImage,
                SimulationLeftSidePadding,
                (float)sender.Size.Height - SimulationLeftSidePadding,
                new Rect(0, 0, _cannonImage.Size.Width, _cannonImage.Size.Height),
                1,
                CanvasImageInterpolation.Cubic);
        }
    }
}

