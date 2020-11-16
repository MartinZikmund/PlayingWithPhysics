using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.Shared.Services.Sounds;
using Windows.Foundation;

namespace Physics.DragMovement.Rendering
{
    public class GamificationCanvasController : DragMovementCanvasController
    {
        private readonly ISoundPlayer _soundPlayer;
		private CanvasBitmap _backgroundImage;

		public GamificationCanvasController(CanvasAnimatedControl canvasAnimatedControl, ISoundPlayer soundPlayer) : base(canvasAnimatedControl)
        {
            _soundPlayer = soundPlayer;
        }

		protected override void DrawBackground(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 68, 197, 214));

			args.DrawingSession.DrawImage(_backgroundImage, new Rect(0, 0, sender.Size.Height, sender.Size.Height));
		}

		public override async Task CreateResourcesAsync(CanvasAnimatedControl sender)
		{
			await base.CreateResourcesAsync(sender);

			// LOAD SOUNDS
			//await _soundPlayer.PreloadSoundAsync(new Uri("ms-appx:///Assets/Sounds/GunCannon.wav", UriKind.Absolute), "Cannon");

			_backgroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/jezero.jpg"));
		}
	}
}
