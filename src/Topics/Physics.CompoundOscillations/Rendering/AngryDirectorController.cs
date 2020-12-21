using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

		private SKPaint _robotPlotPaint = new SKPaint()
		{
			IsAntialias = true,
			IsStroke = true,
			StrokeWidth = 2,
			Color = SKColors.Blue
		};

		private SKPaint _cameraPlotPaint = new SKPaint()
		{
			IsAntialias = true,
			IsStroke = true,
			StrokeWidth = 2,
			Color = SKColors.Green
		};

		private SKPaint _compoundPlotPaint = new SKPaint()
		{
			IsAntialias = true,
			IsStroke = true,
			StrokeWidth = 4,
			Color = SKColors.Black
		};

		private string _plotText;
		private string _directorText;
		private OscillationInfo _oscillationInfo;
		private float _renderingScale;
		private float _topY;
		private double _renderTime;
		private bool _isDisposed;

		private OscillationPhysicsService _carPhysicsService = null;
		private float _timeAtEnd = 0;

		private List<SKPoint> _cameraTrajectory = new List<SKPoint>();
		private List<SKPoint> _robotTrajectory = new List<SKPoint>();
		private List<SKPoint> _compoundTrajectory = new List<SKPoint>();

		public AngryDirectorController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public float CameraHeight { get; set; }

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

			_renderTime = SimulationTime.TotalTime.TotalSeconds <= _timeAtEnd ? SimulationTime.TotalTime.TotalSeconds : _timeAtEnd;
			if (SimulationTime.TotalTime.TotalSeconds <= _timeAtEnd)
			{
				var robotX = _carPhysicsService.CalculateDistance((float)_renderTime);
				var robotY = _carPhysicsService.CalculateY((float)_renderTime);
				_robotTrajectory.Add(new SKPoint(robotX, robotY));
				_cameraTrajectory.Add(new SKPoint(robotX, CameraHeight));
				_compoundTrajectory.Add(new SKPoint(robotX, _robotTrajectory.Last().Y + _cameraTrajectory.Last().Y));
			}
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
			if (_robotTrajectory.Count == 0)
			{
				return;
			}
			
			var robotPath = new SKPath();
			var cameraPath = new SKPath();
			var compoundPath = new SKPath();
			var baseY = 260;
			for (int i = 0; i < _robotTrajectory.Count; i++)
			{
				var x = _robotTrajectory[i].X * 71.14f * _renderingScale;
				var robotY = _topY + baseY * _renderingScale - 46 * _renderingScale * _robotTrajectory[i].Y;
				var cameraY = _topY + baseY * _renderingScale - 46 * _renderingScale * _cameraTrajectory[i].Y;
				var compoundY = _topY + baseY * _renderingScale - 46 * _renderingScale * _compoundTrajectory[i].Y;
				if (i == 0)
				{
					robotPath.MoveTo(new SKPoint(x, robotY));
					cameraPath.MoveTo(new SKPoint(x, cameraY));
					compoundPath.MoveTo(new SKPoint(x, compoundY));
				}
				else
				{
					robotPath.LineTo(new SKPoint(x, robotY));
					cameraPath.LineTo(new SKPoint(x, cameraY));
					compoundPath.LineTo(new SKPoint(x, compoundY));
				}
			}
			args.Canvas.DrawPath(robotPath, _robotPlotPaint);
			args.Canvas.DrawPath(cameraPath, _cameraPlotPaint);
			args.Canvas.DrawPath(compoundPath, _compoundPlotPaint);
		}

		public void Reset()
		{
			SimulationTime.Reset();
			_compoundTrajectory.Clear();
			_cameraTrajectory.Clear();
			_robotTrajectory.Clear();
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

			var bottomCenterX = 71.14f * _renderingScale * _robotTrajectory[_robotTrajectory.Count - 1].X;
			var bottomCenterY = _topY + 970 * _renderingScale - 46 * _renderingScale * _robotTrajectory[_robotTrajectory.Count - 1].Y;

			var period = 1 / _oscillationInfo.Frequency;
			var partOfRotation = _renderTime % period;
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
