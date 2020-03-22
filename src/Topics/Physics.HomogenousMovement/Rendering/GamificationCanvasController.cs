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
using Windows.UI;

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
        private CanvasBitmap _wood1Image;
        private CanvasBitmap _wood2Image;
        private CanvasBitmap[] _treeImages;

        private GameSetup _game = null;

        private TimeSpan? _trajectoryStopTime;

        private TimeSpan? _castleCollisionTime;
        private TimeSpan? _wallCollisionTime;

        public float CannonAngle { get; internal set; }

        protected override Color XMeasureColor => Colors.White;

        protected override Color YMeasureColor => Colors.Transparent;

        public GamificationCanvasController(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public async Task StartNewGameAsync(GameSetup game)
        {
            await RunOnGameLoopAsync(() =>
            {
                _game = game;
                CalculateMaxima();
            });
        }

        protected override void CalculateMaxima()
        {
            if (_game != null)
            {
                _simulationBoundsInMeters = new RectangleF(0, 0, _game.CastleDistance * 1.2f, 30);
            }
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
            _wood1Image = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/drevo_1.png"));
            _wood2Image = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/drevo_2.png"));

            _treeImages = new[]
            {
                await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/strom_1.png")),
                await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/strom_2.png")),
                await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/strom_3.png"))
            };
        }

        protected override void OnSimulationStarting()
        {
            //calculate collision times
            _castleCollisionTime = CalculateCastleCollisionTime();
            _wallCollisionTime = CalculateWallCollisionTime();

            _trajectoryStopTime = _castleCollisionTime ?? _wallCollisionTime;
        }

        private TimeSpan? CalculateWallCollisionTime()
        {
            return null;
        }

        private TimeSpan? CalculateCastleCollisionTime()
        {
            return null;
        }

        protected override void UpdatePadding(ICanvasAnimatedControl sender)
        {

        }

        protected override TimeSpan? TrajectoryStopTime => _trajectoryStopTime;

        protected override void DrawBall(CanvasAnimatedDrawEventArgs args, Vector2 centerPoint, Color movementColor)
        {
            var scale = (float)(_meterSizeInPixels * 5f / _ballImage.Size.Width);
            var scaledImage = new ScaleEffect()
            {
                Source = _ballImage,
                Scale = new Vector2(scale)
            };
            var actualBallSize = scale * (float)_ballImage.Size.Width;
            args.DrawingSession.DrawImage(scaledImage, centerPoint.X - actualBallSize / 2, centerPoint.Y - actualBallSize / 2);
        }

        protected override void DrawBackground(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(_skyImage, new Rect(0, 0, sender.Size.Width, sender.Size.Height));
            args.DrawingSession.DrawImage(_groundImage, new Rect(0, sender.Size.Height - SimulationPadding, sender.Size.Width, SimulationPadding));
            args.DrawingSession.DrawImage(_grassImage, new Rect(0, sender.Size.Height - SimulationPadding - 10, sender.Size.Width, 10));
            if (_game != null)
            {
                DrawTrees(sender, args);
                DrawCastle(sender, args);
                DrawWall(sender, args);
                DrawBackStand(sender, args);
            }
        }


        private void DrawBackStand(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
        }

        private void DrawFrontStand()
        {
        }

        protected override void DrawOverlay(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            if (_game != null)
            {
                DrawCannon(sender, args, -45);
                DrawFrontStand();
            }
        }

        private void DrawTrees(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            for (int treeId = 0; treeId < _game.TreeDistances.Length; treeId++)
            {
                var distance = _game.TreeDistances[treeId];
                var originalImage = _treeImages[treeId % _treeImages.Length];
                var scaling = (float)(_meterSizeInPixels * 15f / originalImage.Size.Width);
                var scaledImage = new ScaleEffect()
                {
                    Source = originalImage,
                    Scale = new Vector2(scaling)
                };
                args.DrawingSession.DrawImage(
                    scaledImage,
                    SimulationLeftSidePadding + distance * _meterSizeInPixels,
                    (float)sender.Size.Height - SimulationPadding - scaling * (float)originalImage.Size.Height,
                    new Rect(0, 0, (float)(scaledImage.Scale.X * originalImage.Size.Width), (float)(scaledImage.Scale.Y * originalImage.Size.Height)),
                    1,
                    CanvasImageInterpolation.Cubic);
            }
        }

        private void DrawCastle(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scale = (float)(_meterSizeInPixels * 50f / _castleImage.Size.Width);
            var scaledImage = new ScaleEffect()
            {
                Source = _castleImage,
                Scale = new Vector2(scale)
            };
            args.DrawingSession.DrawImage(
                scaledImage,
                SimulationLeftSidePadding + _game.CastleDistance * _meterSizeInPixels,
                (float)sender.Size.Height - SimulationPadding - scale * (float)_castleImage.Size.Height,
                new Rect(0, 0, (float)(scaledImage.Scale.X * _castleImage.Size.Width), (float)(scaledImage.Scale.Y * _castleImage.Size.Height)),
                1,
                CanvasImageInterpolation.Cubic);
        }

        private void DrawWall(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scale = (float)(_meterSizeInPixels * 40f / _wallImage.Size.Width);
            var scaledImage = new ScaleEffect()
            {
                Source = _wallImage,
                Scale = new Vector2(scale)
            };
            args.DrawingSession.DrawImage(
                scaledImage,
                SimulationLeftSidePadding + _game.WallDistance * _meterSizeInPixels,
                (float)sender.Size.Height - SimulationPadding - scale * (float)_wallImage.Size.Height,
                new Rect(0, 0, (float)(scaledImage.Scale.X * _wallImage.Size.Width), (float)(scaledImage.Scale.Y * _wallImage.Size.Height)),
                1,
                CanvasImageInterpolation.Cubic);
        }

        private void DrawCannon(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, float angle)
        {            
            var scale = (float)(_meterSizeInPixels * 30f / _cannonImage.Size.Width);
            var image = _cannonImage;
            var cannonImageScaledSize = new Vector2((float)_cannonImage.Size.Width * scale, (float)_cannonImage.Size.Height * scale);
            var imageTransformPivot = new Vector2((float)_cannonImage.Size.Width / 7, (float)_cannonImage.Size.Height / 2) * scale;
            var axisOriginPoint = new Vector2(SimulationLeftSidePadding, (float)sender.Size.Height - SimulationPadding);
            var rotatedImage = new Transform2DEffect()
            {
                Source = image,
                TransformMatrix =
                    Matrix3x2.CreateScale(scale) *
                    Matrix3x2.CreateTranslation(-imageTransformPivot) *
                    Matrix3x2.CreateRotation(MathHelpers.DegreesToRadians(-CannonAngle)) *
                    Matrix3x2.CreateTranslation(imageTransformPivot)
            };
            
            args.DrawingSession.DrawImage(
                rotatedImage,
                new Vector2(axisOriginPoint.X - imageTransformPivot.X, axisOriginPoint.Y - imageTransformPivot.Y));

            args.DrawingSession.DrawEllipse(axisOriginPoint, 2, 2, Colors.Red);
        }
    }
}

