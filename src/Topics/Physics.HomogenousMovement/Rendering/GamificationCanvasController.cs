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
using Physics.HomogenousMovement.Logic.PhysicsServices;

namespace Physics.HomogenousMovement.Rendering
{
    public class GamificationCanvasController : HomogenousMovementCanvasController
    {
        private const float WallWidthInMeters = 40f;
        private const float CastleWidthInMeters = 50f;

        private readonly (Vector2, Vector2)[] _castleRectangles = new (Vector2, Vector2)[]
        {
            (new Vector2(0,0), new Vector2(0.267f, 0.54f)), //Left wall
            (new Vector2(0.267f, 0f), new Vector2(0.72f, 0.432f)), //Center base
            (new Vector2(0.72f, 0f), new Vector2(1f, 0.54f)), //Right wall
            (new Vector2(0.38f, 0.432f), new Vector2(0.591f,0.601f)), //Middle tower
            (new Vector2(0.3f, 0.601f), new Vector2(0.683f,0.765f)) //Upper tower
        };

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

        private bool _hasWallCollided = false;
        private bool _hasCastleCollided = false;
        private TimeSpan? _wallCollisionTime;
        private TimeSpan? _castleCollisionTime;

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
            if (_trajectories.Length > 0)
            {
                var trajectory = _trajectories[0];
                _wallCollisionTime = FindWallCollisionTime(trajectory);
                _castleCollisionTime = FindCastleCollisionTime(trajectory);
            }
        }

        private TimeSpan? FindCastleCollisionTime(TrajectoryData trajectory)
        {
            foreach (var point in trajectory.Points)
            {
                if (IsCastleCollision(point))
                {
                    return point.Time;
                }
            }
            return null;
        }

        private bool IsCastleCollision(TrajectoryPoint ballPositionInMeter)
        {
            var castleWidth = CastleWidthInMeters;
            var castleHeight = (float)((CastleWidthInMeters / _castleImage.Size.Width) * _castleImage.Size.Height);

            foreach (var (bottomLeftRelative, topRightRelative) in _castleRectangles)
            {
                var bottomLeft = new Vector2(_game.CastleDistance + castleWidth * bottomLeftRelative.X, 0 + castleHeight * bottomLeftRelative.Y);
                var topRight = new Vector2(_game.CastleDistance + castleWidth * topRightRelative.X, 0 + castleHeight * topRightRelative.Y);
                if (IsPointInRect(ballPositionInMeter, bottomLeft, topRight))
                {
                    return true;
                }
            }
            return false;
        }

        private TimeSpan? FindWallCollisionTime(TrajectoryData trajectory)
        {
            foreach (var point in trajectory.Points)
            {
                if (IsWallCollision(point))
                {
                    return point.Time;
                }
            }
            return null;
        }

        private bool IsWallCollision(TrajectoryPoint ballPositionInMeters)
        {
            var wallHeight = (WallWidthInMeters / _wallImage.Size.Width) * _wallImage.Size.Height;
            var wallBottomLeft = new Vector2(_game.WallDistance, 0);
            var wallTopRight = new Vector2(_game.WallDistance + WallWidthInMeters, (float)wallHeight);
            return IsPointInRect(ballPositionInMeters, wallBottomLeft, wallTopRight);
        }

        private bool IsPointInRect(TrajectoryPoint point, Vector2 bottomLeft, Vector2 topRight)
        {
            return bottomLeft.X <= point.X && point.X <= topRight.X &&
                   bottomLeft .Y <= point.Y && point.Y <= topRight.Y;
        }

        protected override void UpdatePadding(ICanvasAnimatedControl sender)
        {

        }

        public override void Update(ICanvasAnimatedControl sender)
        {
            base.Update(sender);

        }

        protected override TimeSpan? TrajectoryStopTime => _wallCollisionTime ?? _castleCollisionTime;

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
                if (_castleCollisionTime == null || SimulationTime.TotalTime < _castleCollisionTime)
                {
                    DrawCastle(sender, args);
                }
                else
                {
                    DrawKoCastle(sender, args);
                }

                if (_wallCollisionTime == null || SimulationTime.TotalTime < _wallCollisionTime)
                {
                    DrawWall(sender, args);
                }
                else
                {
                    DrawKoWall(sender, args);
                }

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
            var scale = (float)(_meterSizeInPixels * CastleWidthInMeters / _castleImage.Size.Width);
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

        private void DrawKoCastle(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scale = (float)(_meterSizeInPixels * 1.75f * CastleWidthInMeters / _castleKoImage.Size.Width);
            var scaledImage = new ScaleEffect()
            {
                Source = _castleKoImage,
                Scale = new Vector2(scale)
            };
            args.DrawingSession.DrawImage(
                scaledImage,
                SimulationLeftSidePadding + _game.CastleDistance * _meterSizeInPixels - scale * (float)_castleKoImage.Size.Width * 0.115f,
                (float)sender.Size.Height - SimulationPadding - scale * (float)_castleKoImage.Size.Height * 0.8f,
                new Rect(0, 0, (float)(scaledImage.Scale.X * _castleKoImage.Size.Width), (float)(scaledImage.Scale.Y * _castleKoImage.Size.Height)),
                1,
                CanvasImageInterpolation.Cubic);
        }

        private void DrawWall(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scale = (float)(_meterSizeInPixels * WallWidthInMeters / _wallImage.Size.Width);
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

        private void DrawKoWall(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scale = (float)(_meterSizeInPixels * 2.675f * WallWidthInMeters / _wallKoImage.Size.Width);
            var scaledImage = new ScaleEffect()
            {
                Source = _wallKoImage,
                Scale = new Vector2(scale)
            };
            args.DrawingSession.DrawImage(
                scaledImage,
                SimulationLeftSidePadding + _game.WallDistance * _meterSizeInPixels - scale * (float)_wallKoImage.Size.Width * 0.515f,
                (float)sender.Size.Height - SimulationPadding - scale * (float)_wallKoImage.Size.Height * 0.9f,
                new Rect(0, 0, (float)(scaledImage.Scale.X * _wallKoImage.Size.Width), (float)(scaledImage.Scale.Y * _wallKoImage.Size.Height)),
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

