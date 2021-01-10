using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using Physics.CompoundOscillations.Game;
using Physics.CompoundOscillations.Logic;
using Physics.Shared.Helpers;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.CompoundOscillations.Rendering
{
	public class AngryDirectorController : SkiaCanvasController
	{
		private const string DirectorLabelKey = "Director";
		private const string PlotLabelKey = "Plot";

		private const float ClapTime = 1.5f;
		private const float ClapMoveTime = 0.15f;

		private readonly ISoundPlayer _soundPlayer;

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
		private SKBitmap _stickWide;
		private SKBitmap _stickNarrow;
		private SKBitmap _clapTop;
		private SKBitmap _clapBody;
		private SKBitmap[] _robots = null;

		private SKBitmap _commentHappy = null;
		private SKBitmap _commentAnnoyed = null;
		private SKBitmap _commentAngry = null;


		/// <summary>
		/// Paint for labels
		/// </summary>
		private SKPaint _labelPaint = new SKPaint()
		{
			IsAntialias = true,
			TextSize = 18,
			Color = new SKColor(255, 255, 255),
			TextAlign = SKTextAlign.Left,
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

		private SKPaint _actionShotLinePaint = new SKPaint()
		{
			IsAntialias = true,
			IsStroke = true,
			StrokeWidth = 8,
			Color = new SKColor(255, 0, 0, 80)
		};

		private SKPaint _actionShotThinLinePaint = new SKPaint()
		{
			IsAntialias = true,
			IsStroke = true,
			StrokeWidth = 3,
			Color = new SKColor(0, 255, 0, 80)
		};

		private SKPaint _accuracyTextPaint = new SKPaint()
		{
			IsAntialias = true,
			TextSize = 24,
			Color = new SKColor(255, 255, 255),
			TextAlign = SKTextAlign.Center,
			IsStroke = false
		};

		private SKPaint _idealTextPaint = new SKPaint()
		{
			IsAntialias = true,
			TextSize = 14,
			Color = new SKColor(0, 0, 0),
			TextAlign = SKTextAlign.Center,
			IsStroke = false
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

		private GameInfo _gameInfo;

		public AngryDirectorController(ISkiaCanvas canvasAnimatedControl, ISoundPlayer soundPlayer) : base(canvasAnimatedControl)
		{
			_soundPlayer = soundPlayer;
		}

		public void NewGame(GameInfo gameInfo)
		{
			if (gameInfo is null)
			{
				throw new ArgumentNullException(nameof(gameInfo));
			}
			_gameInfo = gameInfo;
			SimulationTime.Reset();
			Pause();
			_oscillationInfo = new OscillationInfo(
				"Car",
				1,
				0.5f,
				(float)Math.PI / 2,
				"");
			_carPhysicsService = new OscillationPhysicsService(_oscillationInfo);
			_timeAtEnd = _carPhysicsService.GetTimeAtDistance(6.5f * (float)Math.PI);
		}

		public void StartGame()
		{
			ResetGame();
			_gameInfo.CountdownStart = DateTimeOffset.UtcNow;
			_gameInfo.State = GameState.Countdown;
			Restart();
		}

		public void ResetGame()
		{
			_gameInfo?.Reset();
			SimulationTime.Reset();
			_compoundTrajectory.Clear();
			_cameraTrajectory.Clear();
			_robotTrajectory.Clear();
		}

		public float CameraHeight { get; set; } = -1;

		public override void Initialized(ISkiaCanvas sender, SKSurface args)
		{
			// TODO: Allow async initialization in Skia
			_soundPlayer.PreloadSoundAsync(new Uri("ms-appx:///Assets/Game/clap.wav", UriKind.Absolute), "Clap").GetAwaiter().GetResult();

			// Load assets
			var gameAssetsPath = "Assets/Game/";
			var culture = "en";
			if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("cs", StringComparison.InvariantCultureIgnoreCase))
			{
				culture = "cs";
			}

			_background = LoadImageFromPackage($"{gameAssetsPath}background.{culture}.png").DisposeWith(_bitmapsDisposable);
			_commentHappy = LoadImageFromPackage($"{gameAssetsPath}commentHappy.{culture}.png").DisposeWith(_bitmapsDisposable);
			_commentAngry = LoadImageFromPackage($"{gameAssetsPath}commentAngry.{culture}.png").DisposeWith(_bitmapsDisposable);
			_commentAnnoyed = LoadImageFromPackage($"{gameAssetsPath}commentAnnoyed.{culture}.png").DisposeWith(_bitmapsDisposable);

			_camera = LoadImageFromPackage($"{gameAssetsPath}camera.png").DisposeWith(_bitmapsDisposable); ;
			_wheel = LoadImageFromPackage($"{gameAssetsPath}wheel.png").DisposeWith(_bitmapsDisposable); ;
			_robotBody = LoadImageFromPackage($"{gameAssetsPath}robotBody.png").DisposeWith(_bitmapsDisposable); ;
			_directorHappy = LoadImageFromPackage($"{gameAssetsPath}directorHappy.png").DisposeWith(_bitmapsDisposable); ;
			_directorAnnoyed = LoadImageFromPackage($"{gameAssetsPath}directorAnnoyed.png").DisposeWith(_bitmapsDisposable);
			_directorAngry = LoadImageFromPackage($"{gameAssetsPath}directorAngry.png").DisposeWith(_bitmapsDisposable);
			_stickWide = LoadImageFromPackage($"{gameAssetsPath}stickWide.png").DisposeWith(_bitmapsDisposable);
			_stickNarrow = LoadImageFromPackage($"{gameAssetsPath}stickNarrow.png").DisposeWith(_bitmapsDisposable);
			_clapTop = LoadImageFromPackage($"{gameAssetsPath}clapTop.png").DisposeWith(_bitmapsDisposable);
			_clapBody = LoadImageFromPackage($"{gameAssetsPath}clapBody.png").DisposeWith(_bitmapsDisposable);

			var robotBitmaps = new List<SKBitmap>();
			for (int i = 1; i <= 18; i++)
			{
				var robotBitmap = LoadImageFromPackage($"{gameAssetsPath}robot_{i}.png").DisposeWith(_bitmapsDisposable);
				robotBitmaps.Add(robotBitmap);
			}
			_robots = robotBitmaps.ToArray();

			_plotText = Localizer.Instance[PlotLabelKey];
			_directorText = Localizer.Instance[DirectorLabelKey];
		}

		private float _totalSum = 0.0f;

		public override void Update(ISkiaCanvas sender)
		{
			_renderingScale = CalculateRenderingScale(sender);
			_topY = GetTopY(sender);

			_renderTime = 0;

			if (_gameInfo?.State == GameState.Countdown)
			{
				var countdownTime = DateTime.UtcNow - _gameInfo.CountdownStart.Value;
				if (countdownTime.TotalSeconds > (ClapTime - 0.02) && !_gameInfo.ClapSoundPlayed)
				{
					_gameInfo.ClapSoundPlayed = true;
					if (_gameInfo.AreSoundsEnabled)
					{
						_soundPlayer.PlaySound("Clap");
					}
				}
				if (countdownTime.TotalSeconds > ClapTime)
				{
					_totalSum = 0f;
					_gameInfo.State = GameState.Action;
					_gameInfo.TimeAtActionStart = SimulationTime.TotalTime;
				}
			}

			var actionTime = _gameInfo.TimeAtActionStart != null ? SimulationTime.TotalTime - _gameInfo.TimeAtActionStart.Value : (TimeSpan?)null;
			if (_gameInfo?.State == GameState.Action)
			{
				_renderTime = actionTime.Value.TotalSeconds <= _timeAtEnd ? actionTime.Value.TotalSeconds : _timeAtEnd;
				var robotX = _carPhysicsService.CalculateDistance((float)_renderTime);
				var robotY = _carPhysicsService.CalculateY((float)_renderTime);
				_robotTrajectory.Add(new SKPoint(robotX, robotY));
				_cameraTrajectory.Add(new SKPoint(robotX, CameraHeight));
				_compoundTrajectory.Add(new SKPoint(robotX, _robotTrajectory.Last().Y + _cameraTrajectory.Last().Y));

				_totalSum += Math.Abs(_compoundTrajectory[_compoundTrajectory.Count - 1].Y);
				var averageDistance = _totalSum / _compoundTrajectory.Count;
				var invert = 1 - averageDistance / 2;

				_gameInfo.Accuracy = (int)Math.Round(invert * 100);
			}

			if (actionTime?.TotalSeconds > _timeAtEnd  && _gameInfo.State == GameState.Action)
			{
				_gameInfo.State = GameState.ReachedEnd;				
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

			// Draw camera robot
			DrawCamera(sender, args);
			DrawRobot(sender, args);

			// Countdown
			DrawCountdown(sender, args);

			// Draw real-time plot
			DrawPlot(sender, args);

			// Draw director's reaction
			DrawDirector(sender, args);
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

		private void DrawCountdown(ISkiaCanvas sender, SKSurface args)
		{
			if (_gameInfo?.State == GameState.Countdown)
			{
				var centerX = (float)sender.ScaledSize.Width / 2;
				var centerY = GetTopY(sender) + 1080 * _renderingScale / 2;

				var clapBodyX = centerX - _clapBody.Width / 2 * _renderingScale;
				var clapBodyY = centerY - _clapBody.Height / 2 * _renderingScale;
				var clapRect = new SKRect(
					clapBodyX,
					clapBodyY,
					clapBodyX + _clapBody.Width * _renderingScale,
					clapBodyY + _clapBody.Height * _renderingScale);

				args.Canvas.DrawBitmap(_clapBody, clapRect);


				var currentClapTime = Math.Min((DateTimeOffset.UtcNow - _gameInfo.CountdownStart.Value).TotalSeconds, ClapTime);
				var angle = -20f;
				if (currentClapTime > (ClapTime - ClapMoveTime))
				{
					var movePercentage = (currentClapTime - (ClapTime - ClapMoveTime)) / ClapMoveTime;
					angle += (float)movePercentage * 20;
				}
				using var rotatedClapTop = SkiaHelpers.RotateBitmap(_clapTop, angle);

				var clapTopX = centerX + 20 * _renderingScale - rotatedClapTop.Width / 2 * _renderingScale;
				var clapTopY = clapBodyY + 150 * _renderingScale - rotatedClapTop.Height * _renderingScale;
				var clapTopRect = new SKRect(
					clapTopX,
					clapTopY,
					clapTopX + rotatedClapTop.Width * _renderingScale,
					clapTopY + rotatedClapTop.Height * _renderingScale);

				args.Canvas.DrawBitmap(rotatedClapTop, clapTopRect);
			}
		}

		private void DrawCamera(ISkiaCanvas sender, SKSurface args)
		{
			var robotPosition = GetRobotBottomCenterPosition();

			var cameraMidHeight = robotPosition.Y - 278 * _renderingScale;
			var cameraActualHeight = cameraMidHeight - CameraHeight * _renderingScale * 46;
			var renderY = cameraActualHeight - _renderingScale * _camera.Height / 2;
			var renderX = robotPosition.X - _camera.Width * _renderingScale / 3f;
			var renderRect = new SKRect(
				renderX,
				renderY,
				renderX + _renderingScale * _camera.Width,
				renderY + _renderingScale * _camera.Height);

			var robotTopY = robotPosition.Y - (_robots[0].Height / 2);
			var wideStickBottomY = robotPosition.Y - (_robots[0].Height / 2) * _renderingScale;
			var wideStickLeftX = robotPosition.X - _stickWide.Width * _renderingScale / 2;
			var midpointY = (cameraActualHeight - robotTopY) / 4 + robotTopY;
			var wideStickRect = new SKRect(
				wideStickLeftX,
				midpointY,
				wideStickLeftX + _stickWide.Width * _renderingScale,
				wideStickBottomY);
			var narrowStickLeftX = robotPosition.X - _stickNarrow.Width * _renderingScale / 2;
			var narrowStickRect = new SKRect(
				narrowStickLeftX,
				cameraActualHeight,
				narrowStickLeftX + _stickNarrow.Width * _renderingScale,
				midpointY);

			// Camera beam
			var actressX = 1620 * _renderingScale;
			var actionShotLineHeight = cameraActualHeight + 15 * _renderingScale;
			args.Canvas.DrawLine(new SKPoint(robotPosition.X + 50 * _renderingScale, actionShotLineHeight), new SKPoint(actressX, actionShotLineHeight), _actionShotLinePaint);

			var robotStartY = CalculateRobotYFromOscillation(1);
			var cameraIdealHeight = robotStartY - 278 * _renderingScale - (-1 * _renderingScale * 46) + 15 * _renderingScale;

			args.Canvas.DrawLine(new SKPoint(actressX + 100 * _renderingScale, cameraIdealHeight), new SKPoint(actressX + 150 * _renderingScale, cameraIdealHeight), _actionShotThinLinePaint);
			args.Canvas.DrawText("100 %", new SKPoint(actressX + 125, cameraIdealHeight), _idealTextPaint);

			args.Canvas.DrawBitmap(_stickNarrow, narrowStickRect);
			args.Canvas.DrawBitmap(_stickWide, wideStickRect);

			args.Canvas.DrawBitmap(_camera, renderRect);
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

		private void DrawRobot(ISkiaCanvas sender, SKSurface args)
		{
			var bottomCenter = GetRobotBottomCenterPosition();

			var centerX = bottomCenter.X;
			var centerY = bottomCenter.Y - _robots[0].Height * _renderingScale / 2;

			var period = 1 / _oscillationInfo.Frequency;
			var partOfRotation = _renderTime % period;
			var partial = (int)(partOfRotation / period * 18);

			var y = _carPhysicsService.CalculateY((float)_renderTime);

			var angle = (float)Math.Atan(Math.Cos(2 * (float)Math.PI * (float)_renderTime * _oscillationInfo.Frequency + _oscillationInfo.PhaseInRad));
			angle = -MathHelpers.RadiansToDegrees(angle) * .5f;

			using var image = SkiaHelpers.RotateBitmap(_robots[partial], angle);

			var targetRect = new SKRect(
				centerX - _renderingScale * image.Width / 2,
				centerY - _renderingScale * image.Height / 2,
				centerX + _renderingScale * image.Width / 2,
				centerY + _renderingScale * image.Height / 2);

			args.Canvas.DrawBitmap(image, targetRect);
		}

		private void DrawDirector(ISkiaCanvas sender, SKSurface args)
		{
			if (_gameInfo.State == GameState.ReachedEnd || _gameInfo.State == GameState.Action)
			{
				_accuracyTextPaint.TextSize = 50 * _renderingScale;
				SKBitmap directorImage;
				SKBitmap commentImage;
				if (_gameInfo.Accuracy < 60)
				{
					directorImage = _directorAngry;
					commentImage = _commentAngry;
				}
				else if (_gameInfo.Accuracy < 80)
				{
					directorImage = _directorAnnoyed;
					commentImage = _commentAnnoyed;
				}
				else
				{
					directorImage = _directorHappy;
					commentImage = _commentHappy;
				}

				var directorX = 1600 * _renderingScale;
				var directorY = GetTopY(sender) + 150 * _renderingScale;
				var directorRect = new SKRect(
					directorX,
					directorY,
					directorX + directorImage.Width * _renderingScale,
					directorY + directorImage.Height * _renderingScale);

				args.Canvas.DrawBitmap(directorImage, directorRect);


				args.Canvas.DrawText(_gameInfo.Accuracy.ToString().PadLeft(3) + "%", 1530 * _renderingScale, GetTopY(sender) + 300 * _renderingScale, _accuracyTextPaint);

				var commentScale = 0.3f * _renderingScale;
				var commentX = 1740 * _renderingScale;
				var commentY = GetTopY(sender) + 100 * _renderingScale;
				var commentRect = new SKRect(
					commentX,
					commentY,
					commentX + commentImage.Width * commentScale,
					commentY + commentImage.Height * commentScale);
				args.Canvas.DrawBitmap(commentImage, commentRect);
			}
		}

		private SKPoint GetRobotBottomCenterPosition()
		{
			if (_robotTrajectory.Count == 0)
			{
				// Robot starts on top of a hill at PI/2
				return new SKPoint(
					CalculateRobotXFromOscillation((float)Math.PI / 2),
					CalculateRobotYFromOscillation(1));
			}

			var bottomCenterX = CalculateRobotXFromOscillation(_robotTrajectory[_robotTrajectory.Count - 1].X);
			var bottomCenterY = CalculateRobotYFromOscillation(_robotTrajectory[_robotTrajectory.Count - 1].Y);
			return new SKPoint(bottomCenterX, bottomCenterY);
		}

		private float CalculateRobotXFromOscillation(float xInRad) => 71.14f * _renderingScale * xInRad;

		private float CalculateRobotYFromOscillation(float normalizedY) => _topY + 985 * _renderingScale - 42 * _renderingScale * normalizedY;

		private float CalculateRenderingScale(ISkiaCanvas sender)
		{
			return sender.ScaledSize.Width / 1920;
		}

		private float GetTopY(ISkiaCanvas sender) => sender.ScaledSize.Height / 2 - 1080 * _renderingScale / 2;
	}
}
