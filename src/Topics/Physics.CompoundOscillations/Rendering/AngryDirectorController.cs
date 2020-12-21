using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Disposables;
using Physics.CompoundOscillations.Logic;
using Physics.Shared.Helpers;
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
		private SKBitmap[] _robots = null;

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
		private OscillationInfo _oscillationInfo;
		private float _renderingScale;
		private float _topY;

		private bool _isDisposed;

		private OscillationPhysicsService _carPhysicsService = null;
		private float _timeAtEnd = 0;

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

			var robotBitmaps = new List<SKBitmap>();
			for (int i = 1; i <= 18; i++)
			{
				var robotBitmap = LoadImageFromPackage($"{gameAssetsPath}robot_{i}.png").DisposeWith(_bitmapsDisposable);
				robotBitmaps.Add(robotBitmap);
			}
			_robots = robotBitmaps.ToArray();

			_plotText = Localizer.Instance[PlotLabelKey];
			_directorText = Localizer.Instance[DirectorLabelKey];

			_oscillationInfo = new OscillationInfo(
				"Car",
				1,
				0.5f,
				(float)Math.PI / 2,
				"");
			_carPhysicsService = new OscillationPhysicsService(_oscillationInfo);
			_timeAtEnd = _carPhysicsService.GetTimeAtDistance(6.5f * (float)Math.PI);
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

		public void Reset()
		{
			SimulationTime.Reset();
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

		private SKPaint _circlePaint = new SKPaint() { Color = SKColors.Red, IsStroke = false };

		private void DrawRobot(ISkiaCanvas sender, SKSurface args)
		{
			var renderTime = SimulationTime.TotalTime.TotalSeconds <= _timeAtEnd ? SimulationTime.TotalTime.TotalSeconds : _timeAtEnd;

			var bottomCenterX = 71.14f * _renderingScale * _carPhysicsService.CalculateDistance((float)renderTime);
			var bottomCenterY = _topY + 970 * _renderingScale - 46 * _renderingScale * _carPhysicsService.CalculateY((float)renderTime);

			var period = 1 / _oscillationInfo.Frequency;
			var partOfRotation = renderTime % period;
			var partial = (int)(partOfRotation / period * 18);

			var targetRect = new SKRect(
					bottomCenterX - _renderingScale * _robots[0].Width / 2,
					bottomCenterY - _renderingScale * _robots[0].Height,
					bottomCenterX + _renderingScale * _robots[0].Width / 2,
					bottomCenterY);

			var y = _carPhysicsService.CalculateY((float)renderTime);
			float angle = 0.0f;
			if (y < 0)
			{
				var part = y + 1;
				angle = (part / y) * 45;
			}
			else
			{
				angle = (1 - y / 1) * 45;
			}

			using var image = SkiaHelpers.RotateBitmap(_robots[partial], angle);

			args.Canvas.DrawBitmap(image, targetRect);
		}

		private float CalculateRenderingScale(ISkiaCanvas sender)
		{
			return sender.ScaledSize.Width / 1920;
		}

		private float GetTopY(ISkiaCanvas sender) => sender.ScaledSize.Height / 2 - 1080 * _renderingScale / 2;
	}
}
