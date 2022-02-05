using System;
using System.Security.Cryptography;
using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Physics;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.CyclicProcesses.Rendering
{
	public class CyclicProcessesCanvasController : SkiaCanvasController
	{
		private const float Padding = 0.1f;
		private const float ChartPadding = 1.2f;

		private readonly SkiaAxesRenderer _axesRenderer = new SkiaAxesRenderer();
		private readonly SKPaint _diagramPaint = new SKPaint()
		{
			Color = SKColors.Black,
			StrokeWidth = 2,
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High
		};

		private IInputConfiguration _inputConfiguration;
		private IPhysicsService _physicsService;

		private SKBitmap _diagramCanvasCache;

		private float _volumeToPixels = 1;
		private float _pressureToPixels = 1;

		private float _minAxisV = 0;
		private float _maxAxisV = 0;
		private float _minAxisP = 0;
		private float _maxAxisP = 0;

		public CyclicProcessesCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public void StartSimulation(IInputConfiguration inputConfiguration)
		{
			_inputConfiguration = inputConfiguration ?? throw new ArgumentNullException(nameof(inputConfiguration));
			_physicsService = PhysicsServiceFactory.GetPhysicsService(inputConfiguration);
			_diagramCanvasCache?.Dispose();
			_diagramCanvasCache = null;
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_physicsService == null)
			{
				return;
			}

			if (_diagramCanvasCache == null)
			{
				CacheDiagram(sender);
			}

			args.Canvas.DrawBitmap(_diagramCanvasCache, new SKPoint(0, 0));
		}

		private void CacheDiagram(ISkiaCanvas sender)
		{
			_diagramCanvasCache = new SKBitmap((int)sender.ScaledSize.Width, (int)sender.ScaledSize.Height);
			using var canvas = new SKCanvas(_diagramCanvasCache);

			// Draw process
			DrawAxes(sender, canvas);
			DrawProcess(sender, canvas);
		}

		private void DrawAxes(ISkiaCanvas sender, SKCanvas canvas)
		{
			var horizontalPadding = (int)(sender.ScaledSize.Width * Padding);
			var verticalPadding = (int)(sender.ScaledSize.Height * Padding);

			var yUnitInPixels = _pressureToPixels * 1000;
			var xUnitInPixels = _volumeToPixels / 1000;

			var right = (float)Math.Ceiling(horizontalPadding + xUnitInPixels * (_maxAxisV * 1000 - _minAxisV * 1000)) + 1;
			var bottom = (float)Math.Ceiling(verticalPadding + yUnitInPixels * (_maxAxisP / 1000 - _minAxisP / 1000)) + 1;

			var axesBounds = new SimulationBounds(horizontalPadding, verticalPadding, right, bottom);

			_axesRenderer.XUnitSizeInPixels = (float)xUnitInPixels;
			_axesRenderer.YUnitSizeInPixels = (float)yUnitInPixels;
			_axesRenderer.ShouldDrawYAxis = true;
			_axesRenderer.ShouldDrawYMeasure = true;
			_axesRenderer.XUnitFormatString = "0.## dm³";
			_axesRenderer.YUnitFormatString = "0.## kPa";

			_axesRenderer.OriginUnitCoordinates = new SKPoint(_minAxisV * 1000, _minAxisP / 1000);
			_axesRenderer.TargetBounds = axesBounds;
			_axesRenderer.OriginRelativePosition = new SKPoint(0, 0f);
			_axesRenderer.Draw(sender, canvas);
		}

		private void DrawProcess(ISkiaCanvas sender, SKCanvas canvas)
		{
			switch (_physicsService.Process)
			{
				case Logic.ProcessType.Isotermic:
					DrawNonLinearProcess(canvas, _physicsService);
					break;
				case Logic.ProcessType.Isochoric:
					DrawIsochoricProcess(canvas);
					break;
				case Logic.ProcessType.Isobaric:
					DrawIsobaricProcess(canvas);
					break;
				case Logic.ProcessType.Adiabatic:
					DrawNonLinearProcess(canvas, _physicsService);
					break;
				case Logic.ProcessType.StirlingEngine:
					DrawStirling(canvas);
					break;
			}
		}

		private void DrawStirling(SKCanvas canvas)
		{
			var stirling = (StirlingEnginePhysicsService)_physicsService;
			var stirlingInput = (StirlingEngineInputConfiguration)_inputConfiguration;
			// Draw two parts
			DrawNonLinearProcess(canvas, stirling.Isotermic1);
			DrawNonLinearProcess(canvas, stirling.Isotermic2);

			var vertical1X = GetRenderX(stirlingInput.V1);
			var vertical2X = GetRenderX(stirlingInput.V2);
			var vertical1FromY = GetRenderY(stirling.P1);
			var vertical1ToY = GetRenderY(stirling.P4);
			var vertical2FromY = GetRenderY(stirling.P2);
			var vertical2ToY = GetRenderY(stirling.P3);
			canvas.DrawLine(vertical1X, vertical1FromY, vertical1X, vertical1ToY, _diagramPaint);
			canvas.DrawLine(vertical2X, vertical2FromY, vertical2X, vertical2ToY, _diagramPaint);
		}

		private void DrawNonLinearProcess(SKCanvas canvas, IPhysicsService physicsService)
		{
			float stepSizeInUnits = 0.01f;

			using SKPath path = new SKPath();
			var currentTime = 0.0f;
			var maxX = PhysicsService.CycleLengthInSeconds / 2;
			float lastRenderX = GetRenderX(physicsService.CalculateV(currentTime));
			float lastRenderY = GetRenderY(physicsService.CalculateP(currentTime));
			path.MoveTo(lastRenderX, lastRenderY);
			currentTime += stepSizeInUnits;

			while (currentTime <= maxX)
			{
				float renderX = GetRenderX(physicsService.CalculateV(currentTime));
				float renderY = GetRenderY(physicsService.CalculateP(currentTime));
				path.LineTo(renderX, renderY);

				if (currentTime >= maxX)
				{
					break;
				}

				currentTime = Math.Min(currentTime + stepSizeInUnits, maxX);
			}

			canvas.DrawPath(path, _diagramPaint);
		}

		private void DrawIsobaricProcess(SKCanvas canvas)
		{
			var input = _inputConfiguration as IsobaricInputConfiguration;

			var fromX = GetRenderX(input.V1);
			var toX = GetRenderX(input.V2);
			var y = GetRenderY(input.P);

			canvas.DrawLine(fromX, y, toX, y, _diagramPaint);
		}

		private void DrawIsochoricProcess(SKCanvas canvas)
		{
			var input = _inputConfiguration as IsochoricInputConfiguration;

			var x = GetRenderX(input.V);
			var fromY = GetRenderY(_physicsService.CalculateP(0));
			var toY = GetRenderY(_physicsService.CalculateP(PhysicsService.CycleLengthInSeconds / 2));

			canvas.DrawLine(x, fromY, x, toY, _diagramPaint);
		}

		public override void Update(ISkiaCanvas sender)
		{
			if (_physicsService == null)
			{
				return;
			}

			if (sender.ScaledSize.Width != _diagramCanvasCache?.Width ||
				sender.ScaledSize.Height != _diagramCanvasCache?.Height)
			{
				// Invalidate image
				_diagramCanvasCache = null;

				var effectiveHeight = _canvas.ScaledSize.Height - (_canvas.ScaledSize.Height * Padding * 2);
				var effectiveWidth = _canvas.ScaledSize.Width - (_canvas.ScaledSize.Width * Padding * 2);

				var vDelta = _physicsService.MaxV - _physicsService.MinV;
				if (vDelta == 0)
				{
					vDelta = 0.01f; // Arbitrary
				}

				_minAxisV = Math.Max(0, _physicsService.MinV - vDelta / 10);
				_maxAxisV = Math.Min(0.1f, _physicsService.MaxV + vDelta / 10);

				var pDelta = _physicsService.MaxP - _physicsService.MinP;
				if (pDelta == 0)
				{
					pDelta = 1000f; // Arbitrary
				}

				_minAxisP = Math.Max(0, _physicsService.MinP - pDelta / 10);
				_maxAxisP = Math.Min(1000000f, _physicsService.MaxP + pDelta / 10);
				_volumeToPixels = (float)(effectiveWidth / (_maxAxisV - _minAxisV));
				_pressureToPixels = (float)(effectiveHeight / (_maxAxisP - _minAxisP));
			}
		}

		private float GetRenderX(float volume) => _canvas.ScaledSize.Width * Padding + _volumeToPixels * (volume - _minAxisV);

		private float GetRenderY(float pressure) => _canvas.ScaledSize.Height - _canvas.ScaledSize.Height * Padding - _pressureToPixels * (pressure - _minAxisP);
	}
}
