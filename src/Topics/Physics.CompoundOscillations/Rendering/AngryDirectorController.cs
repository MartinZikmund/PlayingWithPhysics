using System.Reactive.Disposables;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.CompoundOscillations.Rendering
{
	public class AngryDirectorController : SkiaCanvasController
	{
		// Bitmap assets
		private CompositeDisposable _bitmapsDisposable = new CompositeDisposable();
		private SKBitmap _background = null;
		private SKBitmap _camera = null;
		private SKBitmap _stick = null;
		private SKBitmap _wheel = null;
		private SKBitmap _robotBody = null;
		private SKBitmap _directorHappy = null;
		private SKBitmap _directorAnnoyed = null;
		private SKBitmap _directorAngry = null;

		/// <summary>
		/// Paint for labels
		/// </summary>
		private SKPaint _labelPaint = new SKPaint()
		{
			IsAntialias = true,
			TextSize = 18,
			Color = new SKColor(255, 255, 255),
			TextAlign = SKTextAlign.Right,
			IsStroke = false
		};

		private string DirectorLabelKey = "Director";
		private string PlotLabelKey = "Plot";

		private float _renderingScale;
		private bool _isDisposed;

		public AngryDirectorController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public override void Initialized(ISkiaCanvas sender, SKSurface args)
		{
			// Load assets
			var gameAssetsPath = "Assets/Game/";
			_background = LoadImageFromPackage($"{gameAssetsPath}background.png").DisposeWith(_bitmapsDisposable);
			_camera = LoadImageFromPackage($"{gameAssetsPath}camera.png").DisposeWith(_bitmapsDisposable); ;
			_stick = LoadImageFromPackage($"{gameAssetsPath}stick.png").DisposeWith(_bitmapsDisposable); ;
			_wheel = LoadImageFromPackage($"{gameAssetsPath}wheel.png").DisposeWith(_bitmapsDisposable); ;
			_robotBody = LoadImageFromPackage($"{gameAssetsPath}robotBody.png").DisposeWith(_bitmapsDisposable); ;
			_directorHappy = LoadImageFromPackage($"{gameAssetsPath}directorHappy.png").DisposeWith(_bitmapsDisposable); ;
			_directorAnnoyed = LoadImageFromPackage($"{gameAssetsPath}directorAnnoyed.png").DisposeWith(_bitmapsDisposable); ;
			_directorAngry = LoadImageFromPackage($"{gameAssetsPath}directorAngry.png").DisposeWith(_bitmapsDisposable);
		}

		public override void Update(ISkiaCanvas sender)
		{
			_renderingScale = CalculateRenderingScale(sender);
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_isDisposed)
			{
				return;
			}
			args.Canvas.Clear(SKColors.Black);

			DrawBackground(sender, args);
			DrawLabels(sender, args);
		}

		public override void Dispose()
		{
			_isDisposed = true;
			base.Dispose();

			// Dispose assets
			_bitmapsDisposable.Dispose();
		}

		private void DrawBackground(ISkiaCanvas sender, SKSurface args)
		{
			var backgroundSize = new SKSize(_background.Width * _renderingScale, _background.Height * _renderingScale);
			args.Canvas.DrawBitmap(
				_background,
				new SKRect(
					sender.ScaledSize.Width / 2 - backgroundSize.Width / 2,
					sender.ScaledSize.Height / 2 - backgroundSize.Height / 2,
					sender.ScaledSize.Width / 2 + backgroundSize.Width / 2,
					sender.ScaledSize.Height / 2 + backgroundSize.Height / 2));
		}

		private void DrawLabels(ISkiaCanvas sender, SKSurface args)
		{

		}

		private float CalculateRenderingScale(ISkiaCanvas sender)
		{
			return sender.ScaledSize.Width / 1920;
		}
	}
}
