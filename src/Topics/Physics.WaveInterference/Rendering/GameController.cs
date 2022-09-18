using System;
using System.Linq;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using Physics.WaveInterference.Game;
using Physics.WaveInterference.Logic;
using SkiaSharp;

namespace Physics.WaveInterference.Rendering;

public class GameController : SkiaCanvasController
{
	private const float VerticalPadding = 12;
	private const float HorizontalPadding = 12;

	private float _unitsPerPixel = 0;
	private float _pixelsPerUnit = 0;
	private float _stepSize = 0;

	private GameWaveInfo[] _activeWaves;
	private GamePhysicsService _physicsService;
	private SKPaint[] _waveStrokePaints = new[]
	{
		new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			StrokeWidth = 4,
			Color = SKColors.Red,
		},
		new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High,
			StrokeWidth = 4,
			Color = new SKColor(0, 127, 198)
		},
	};
	private SKPaint _resultingWaveStrokePaint = new SKPaint()
	{
		IsStroke = true,
		IsAntialias = true,
		FilterQuality = SKFilterQuality.High,
		StrokeWidth = 4,
		Color = new SKColor(0, 149, 64)
	};

	private SKPaint _wavePackagePaint = new SKPaint()
	{
		IsStroke = true,
		IsAntialias = true,
		StrokeWidth = 4,
		Color = new SKColor(252, 141, 31)
	};

	private SKPaint _axis1Paint = new SKPaint()
	{
		IsStroke = true,
		IsAntialias = true,
		StrokeWidth = 1,
		Color = SKColors.Black
	};

	private SKPaint _axis2Paint = new SKPaint()
	{
		IsStroke = true,
		IsAntialias = true,
		StrokeWidth = 1.5f,
		Color = SKColors.Black
	};

	private float _maxY;
	private float _minY;

	private float _minX;
	private float _maxX;

	private float _maximumFrequency;
	private double _inherentSlowdown = 1.0;

	private GameInfo _gameInfo;

	public GameController(ISkiaCanvas canvasAnimatedControl) :
		base(canvasAnimatedControl)
	{
	}

	public void SetGameInfo(GameInfo gameInfo)
	{
		if (gameInfo == null)
		{
			_activeWaves = null;
			return;
		}

		_gameInfo = gameInfo;

		_activeWaves = new[] { gameInfo.Wave1, gameInfo.Wave2 };
		_physicsService = gameInfo.GamePhysicsService;
		_maxY = 2;
		_minY = -2;

		var strokePaintWidthAdjustment = 2f;
		foreach (var wave in _activeWaves)
		{
			//var slowedDownWaveInfo = new WaveInfo(
			//	wave.WaveInfo.Label,
			//	wave.WaveInfo.Amplitude,
			//	(float)(wave.WaveInfo.Frequency * _inherentSlowdown),
			//	wave.WaveInfo.WaveLength,
			//	WaveDirection.Right,
			//	0.0f,
			//	wave.WaveInfo.Color);

			//var physicsService = new WavePhysicsService(slowedDownWaveInfo);

			//_maxY += physicsService.MaxY;
			//_minY += physicsService.MinY;

			//_waveTrajectories.Add(wave.WaveInfo, new WaveTrajectory(wave.WaveInfo));

			//var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(wave.WaveInfo.Color);
			//var skColor = new SKColor(color.R, color.G, color.B);
			//var fillPaint = new SKPaint()
			//{
			//	Color = skColor,
			//	IsStroke = false,
			//	IsAntialias = true,
			//};
			//_waveFillPaints.Add(wave.WaveInfo, fillPaint);
			//var strokePaint = new SKPaint()
			//{
			//	Color = skColor,
			//	IsStroke = true,
			//	StrokeJoin = SKStrokeJoin.Round,
			//	StrokeWidth = 2 + strokePaintWidthAdjustment,
			//	IsAntialias = true,
			//};
			//_waveStrokePaints.Add(wave.WaveInfo, strokePaint);
			strokePaintWidthAdjustment = 0f;
		}

		//_waveInterferencePhysicsService = new WaveInterferencePhysicsService(_wavePhysicsServices.Values.ToArray());
		//_waveInterferenceTrajectory = new WaveInterferenceTrajectory(_waveTrajectories.Values.ToArray());
	}

	private void SetInherentSlowdown()
	{
		if (_maximumFrequency > 5000)
		{
			_inherentSlowdown = 1 / 10000.0;
		}
		else if (_maximumFrequency > 500)
		{
			_inherentSlowdown = 1 / 1000.0;
		}
		else if (_maximumFrequency > 50)
		{
			_inherentSlowdown = 1 / 100.0;
		}
		else if (_maximumFrequency > 5)
		{
			_inherentSlowdown = 1 / 10.0;
		}
		else
		{
			_inherentSlowdown = 1 / 10.0;
		}
	}

	public void StartSimulation()
	{
		SimulationTime.Restart();
		Play();
	}

	public override void Update(ISkiaCanvas sender)
	{
		if (_activeWaves == null)
		{
			return;
		}

		_maximumFrequency = _activeWaves.Max(o => o.F);
		SetInherentSlowdown();
		_pixelsPerUnit = sender.ScaledSize.Height / 6;
		_unitsPerPixel = 1 / _pixelsPerUnit;
		_stepSize = 2;
		_maxX = sender.ScaledSize.Width;

		//var (minX, maxX) = CalculatePreferredWaveXBounds();
		//var horizontalWidthInMeters = maxX - minX;
		//var screenWidth = sender.ScaledSize.Width - HorizontalPadding * 2;
		//_pixelsPerMeter = screenWidth / horizontalWidthInMeters;
		//_metersPerPixel = horizontalWidthInMeters / screenWidth;

		//if ((_maxY - _minY) * _pixelsPerMeter > GetDisplayHeightInPixels(sender))
		//{
		//	// Vertical axis must fit, adjust X
		//	var verticalPixelsPerMeterRequirement = GetDisplayHeightInPixels(sender) / (_maxY - _minY);
		//	_pixelsPerMeter = verticalPixelsPerMeterRequirement;

		//	var originDistance = _activeWaves[1].WaveInfo.OriginX;
		//	var displayWidthInMeters = GetDisplayWidthInPixels(sender) / _pixelsPerMeter;
		//	if (_activeWaves[0].WaveInfo.Direction == _activeWaves[1].WaveInfo.Direction)
		//	{
		//		if (_activeWaves[0].WaveInfo.Direction == WaveDirection.Left)
		//		{
		//			_maxX = _activeWaves[1].WaveInfo.OriginX;
		//			_minX = _maxX - displayWidthInMeters;
		//		}
		//		else
		//		{
		//			_minX = 0;
		//			_maxX = displayWidthInMeters;
		//		}
		//	}
		//	else
		//	{
		//		var fakeCenter = _activeWaves[1].WaveInfo.OriginX / 2;
		//		_minX = fakeCenter - displayWidthInMeters / 2;
		//		_maxX = fakeCenter + displayWidthInMeters / 2;
		//	}
		//}
		//else
		//{
		//	_minX = minX;
		//	_maxX = maxX;
		//}
	}

	//private (float minX, float maxX) CalculatePreferredWaveXBounds()
	//{
	//	float minX = 0;
	//	float maxX = 0;

	//	var distance = _activeWaves.Max(w => w.WaveInfo.OriginX);
	//	var maxLambda = _activeWaves.Max(w => w.WaveInfo.WaveLength);

	//	var displayWidth = 0f;
	//	if (_activeWaves[0].WaveInfo.Direction == _activeWaves[1].WaveInfo.Direction)
	//	{
	//		displayWidth = distance + 3 * maxLambda;
	//		if (_activeWaves[0].WaveInfo.Direction == WaveDirection.Left)
	//		{
	//			maxX = _activeWaves[1].WaveInfo.OriginX;
	//			minX = _maxX - displayWidth;
	//		}
	//		else
	//		{
	//			minX = 0;
	//			maxX = displayWidth;
	//		}
	//	}
	//	else
	//	{
	//		displayWidth = distance + 4 * maxLambda;

	//		var fakeCenter = _activeWaves[1].WaveInfo.OriginX / 2;
	//		minX = fakeCenter - displayWidth / 2;
	//		maxX = fakeCenter + displayWidth / 2;
	//	}

	//	return (minX, maxX);
	//}

	private float GetDisplayHeightInPixels(ISkiaCanvas sender) => (float)(sender.ScaledSize.Height - sender.ScaledSize.Height * 0.3);

	private float GetDisplayWidthInPixels(ISkiaCanvas sender) => (float)(sender.ScaledSize.Width - 2 * HorizontalPadding);

	public override void Draw(ISkiaCanvas sender, SKSurface args)
	{
		args.Canvas.Clear(new SKColor(255, 244, 244, 244));

		if (_activeWaves == null)
		{
			return;
		}

		//	DrawAxes(sender, args);

		if (_activeWaves.Length > 1)
		{
			//DrawTrajectory(sender, args, _waveInterferenceTrajectory, _interferenceStrokePaint, true);
			//DrawWaveCurrentPoint(sender, args, totalValue, _interferenceFillPaint);
		}

		if (_gameInfo.ShowGroup)
		{
			DrawWave(sender, args, (x, time) => (float)_physicsService.CalculateAbsPackage(x, time), _wavePackagePaint, false);
			DrawWave(sender, args, (x, time) => -(float)_physicsService.CalculateAbsPackage(x, time), _wavePackagePaint, false);
		}
		if (_gameInfo.ShowResultingWave)
		{
			DrawWave(sender, args, (x, time) => (float)_physicsService.CalculateH(x, time), _resultingWaveStrokePaint, false);
		}
		if (_gameInfo.ShowWaves)
		{
			foreach (var wave in _activeWaves)
			{
				var strokePaint = _waveStrokePaints[Array.IndexOf(_activeWaves, wave)];
				DrawWave(sender, args, (x, time) => (float)_physicsService.CalculateSingleWave(x, time, wave), strokePaint, false);
			}
		}
	}

	public double GetSimulationDisplayTime() => SimulationTime.TotalTime.TotalSeconds * _inherentSlowdown;

	private double GetAdjustedTotalTime() => Math.Truncate(SimulationTime.TotalTime.TotalMilliseconds) / 1000;

	private void DrawWave(ISkiaCanvas sender, SKSurface args, Func<float, float, float> waveCalculator, SKPaint paint, bool accurate)
	{
		//Try rendering using https://github.com/PeterWaher/IoTGateway/blob/master/Script/Waher.Script.Graphs/Functions/Plots/Plot2DCurve.cs
		//if (trajectory.StartX >= trajectory.EndX)
		//{
		//	return;
		//}

		var stepSizeInPixels = GetDisplayWidthInPixels(sender) / 800;
		var stepSizeInUnits = stepSizeInPixels * _unitsPerPixel;
		var lastRenderTime = GetSimulationDisplayTime();
		using SKPath path = new SKPath();
		var currentX = _minX;

		float lastRenderX = GetRenderX(currentX);
		float lastRenderY = GetRenderY(waveCalculator(currentX, (float)lastRenderTime));
		path.MoveTo(lastRenderX, lastRenderY);

		while (lastRenderX <= sender.ScaledSize.Width)
		{
			var renderX = GetRenderX(currentX);
			var y = waveCalculator(currentX, (float)lastRenderTime);
			float renderY = GetRenderY(y);

			path.LineTo(renderX, renderY);
			lastRenderX = renderX;
			lastRenderY = renderY;
			currentX += stepSizeInUnits;
		}

		args.Canvas.DrawPath(path, paint);
	}

	private float GetRenderY(float y) => (float)(_canvas.ScaledSize.Height / 2 - y * _pixelsPerUnit);

	private float GetRenderX(float x) => ((x - _minX) * _pixelsPerUnit + HorizontalPadding);

	private SkiaAxesRenderer _axesRenderer = new SkiaAxesRenderer();

	private void DrawAxes(ISkiaCanvas sender, SKSurface args)
	{
		float relativeOriginPosition = Math.Abs(_minX / (_maxX - _minX));

		var axesBounds = new SimulationBounds(HorizontalPadding, 10, (float)sender.ScaledSize.Width - HorizontalPadding, (float)sender.ScaledSize.Height - 10);
		var endRenderX = (float)sender.ScaledSize.Width - HorizontalPadding;
		var horizontalPixelDiff = endRenderX - axesBounds.Left;
		var endTime = GetSimulationDisplayTime();
		var originSeconds = ((float)endTime - horizontalPixelDiff / _pixelsPerUnit) * (float)_inherentSlowdown;
		_axesRenderer.ShouldDrawYAxis = false;
		_axesRenderer.ShouldDrawYMeasure = false;
		_axesRenderer.YUnitSizeInPixels = _pixelsPerUnit;
		_axesRenderer.XUnitSizeInPixels = (float)_pixelsPerUnit;

		var formatString = "0.#";
		if (_maximumFrequency > 5000)
		{
			formatString = "0.#####";
		}
		else if (_maximumFrequency > 500)
		{
			formatString = "0.####";
		}
		else if (_maximumFrequency > 50)
		{
			formatString = "0.###";
		}
		else if (_maximumFrequency > 5)
		{
			formatString = "0.##";
		}
		_axesRenderer.XUnitFormatString = formatString;

		_axesRenderer.OriginUnitCoordinates = new SKPoint(0, 0);
		_axesRenderer.TargetBounds = axesBounds;
		_axesRenderer.OriginRelativePosition = new SKPoint(relativeOriginPosition, 0.5f);
		_axesRenderer.Draw(sender, args);
	}

	private void DrawWaveOriginPoint(ISkiaCanvas sender, SKSurface args, WaveTrajectory wave, SKPaint paint, float pointSize)
	{
		if (wave.GetY(wave.OriginX, (float)GetAdjustedTotalTime(), true) is float y)
		{
			var renderX = GetRenderX(wave.OriginX);
			var renderY = GetRenderY(y);

			args.Canvas.DrawCircle(renderX, renderY, pointSize, paint);
		}
	}
}
