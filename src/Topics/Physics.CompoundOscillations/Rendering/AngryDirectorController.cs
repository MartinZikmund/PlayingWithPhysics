using System;
using System.Globalization;
using System.Reactive.Disposables;
using Physics.CompoundOscillations.Logic;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.CompoundOscillations.Rendering
{
	public class AngryDirectorController : SkiaCanvasController
	{
		private const string DirectorLabelKey = "Director";
		private const string PlotLabelKey = "Plot";

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
			IsStroke = false,
		};

		private string _plotText;
		private string _directorText;

		private float _renderingScale;
		private float _topY;

		private bool _isDisposed;

		private OscillationPhysicsService _carPhysicsService = null;

		public AngryDirectorController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public override void Initialized(ISkiaCanvas sender, SKSurface args)
		{
			// Load assets
			var gameAssetsPath = "Assets/Game/";
			if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("cs", StringComparison.InvariantCultureIgnoreCase))
			{
				_background = LoadImageFromPackage($"{gameAssetsPath}background.cs.png").DisposeWith(_bitmapsDisposable);
			}
			else
			{
				_background = LoadImageFromPackage($"{gameAssetsPath}background.en.png").DisposeWith(_bitmapsDisposable);
			}
			_camera = LoadImageFromPackage($"{gameAssetsPath}camera.png").DisposeWith(_bitmapsDisposable); ;
			_stick = LoadImageFromPackage($"{gameAssetsPath}stick.png").DisposeWith(_bitmapsDisposable); ;
			_wheel = LoadImageFromPackage($"{gameAssetsPath}wheel.png").DisposeWith(_bitmapsDisposable); ;
			_robotBody = LoadImageFromPackage($"{gameAssetsPath}robotBody.png").DisposeWith(_bitmapsDisposable); ;
			_directorHappy = LoadImageFromPackage($"{gameAssetsPath}directorHappy.png").DisposeWith(_bitmapsDisposable); ;
			_directorAnnoyed = LoadImageFromPackage($"{gameAssetsPath}directorAnnoyed.png").DisposeWith(_bitmapsDisposable);
			_directorAngry = LoadImageFromPackage($"{gameAssetsPath}directorAngry.png").DisposeWith(_bitmapsDisposable);

			_plotText = Localizer.Instance[PlotLabelKey];
			_directorText = Localizer.Instance[DirectorLabelKey];

			_carPhysicsService = new OscillationPhysicsService(new OscillationInfo(
				"Car",
				1,
				0.25f,
				0,
				""));
		}

		public override void Update(ISkiaCanvas sender)
		{
			_renderingScale = CalculateRenderingScale(sender);
			_topY = GetTopY(sender);
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_isDisposed)
			{
				return;
			}
			args.Canvas.Clear(SKColors.Black);

			DrawBackground(sender, args);
			DrawRobot(sender, args);

			DrawPlot(sender, args);
			DrawDirector(sender, args);
		}

		private void DrawDirector(ISkiaCanvas sender, SKSurface args)
		{
		}

		private void DrawPlot(ISkiaCanvas sender, SKSurface args)
		{
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

		private void DrawRobot(ISkiaCanvas sender, SKSurface args)
		{
		}

		private float CalculateRenderingScale(ISkiaCanvas sender)
		{
			return sender.ScaledSize.Width / 1920;
		}

		private float GetTopY(ISkiaCanvas sender) => sender.ScaledSize.Height / 2 - 1080 * _renderingScale / 2;
	}
}
