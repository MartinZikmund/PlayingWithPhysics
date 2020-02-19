using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Physics.HomogenousMovement.Rendering
{
    public class GamificationCanvasController : HomogenousMovementCanvasController
    {
        private CanvasBitmap _skyImage;
        private CanvasBitmap _groundImage;
        private CanvasBitmap _ballImage;
        private CanvasBitmap _cannonOnImage;
        private CanvasBitmap _cannonOffImage;

        public GamificationCanvasController(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public override async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            await base.CreateResourcesAsync(sender);
            _skyImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/sky.png"));
            _groundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/ground.png"));
            _ballImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/ball.png"));
            _cannonOnImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/cannonon.png"));
            _cannonOffImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/cannonoff.png"));
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
        }
    }
}

