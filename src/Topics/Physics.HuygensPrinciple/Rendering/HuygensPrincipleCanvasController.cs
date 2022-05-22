using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Physics.HuygensPrinciple.Logic;
using Physics.HuygensPrinciple.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.HuygensPrinciple.Rendering
{
	public class HuygensPrincipleCanvasController : SkiaCanvasController
	{
		internal ScenePreset _scene = null;
		internal HuygensManager _manager = null;
		internal RenderConfigurationViewModel _renderConfiguration;
		internal DrawingStateViewModel _drawingState;
		internal readonly SKColor _waveColor = SKColors.Aqua;
		internal readonly SKColor _waveEdgeColor = SKColors.Blue;
		internal readonly SKColor _emptyColor = SKColors.White;
		internal readonly SKColor _wallColor = SKColors.Brown;
		internal readonly SKColor _sourceColor = SKColors.DarkOrange;

		internal readonly SKPaint _waveFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true
		};

		internal readonly SKPaint _wallFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true
		};

		internal readonly SKPaint _waveEdgeStrokePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 4
		};

		internal readonly SKPaint _significantPointPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true,
		};

		internal readonly SKPaint _significantPointEllipsePaint = new SKPaint()
		{
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 2
		};

		internal readonly SKPaint _sourceFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true
		};

		internal readonly SKPaint _emptyFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true
		};

		internal float GetSquareSize(ISkiaCanvas canvas) => Math.Min(canvas.ScaledSize.Width, canvas.ScaledSize.Height);

		internal SKPoint GetRenderTopLeft(ISkiaCanvas canvas)
		{
			var squareSize = GetSquareSize(canvas);
			return new SKPoint(canvas.ScaledSize.Width / 2 - squareSize / 2, canvas.ScaledSize.Height / 2 - squareSize / 2);
		}

		internal void SetRenderConfiguration(RenderConfigurationViewModel renderConfiguration)
		{
			_renderConfiguration = renderConfiguration;
		}

		internal void SetDrawingState(DrawingStateViewModel drawingState)
		{
			_drawingState = drawingState;
		}

		public HuygensPrincipleCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
			_waveFillPaint.Color = _waveColor;
			_wallFillPaint.Color = _wallColor;
			_sourceFillPaint.Color = _sourceColor;
			_waveEdgeStrokePaint.Color = _waveEdgeColor;
			_emptyFillPaint.Color = _emptyColor;
			_significantPointEllipsePaint.Color = SKColors.Red;
			_significantPointPaint.Color = SKColors.Red;
		}

		public IHuygensVariantRenderer Renderer { get; private set; }

		public void StartSimulation(HuygensManager manager, ScenePreset scene)
		{
			SimulationTime.Restart();
			_manager = manager ?? throw new ArgumentNullException(nameof(manager));
			_scene = scene ?? throw new ArgumentNullException(nameof(scene));
			Renderer.StartSimulation();
		}

		public void SetVariantRenderer(IHuygensVariantRenderer renderer) => Renderer = renderer;

		public override void Update(ISkiaCanvas sender) => Renderer?.Update(sender);

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			Renderer?.Draw(sender, args);
		}

		internal void DrawInitalScene(ISkiaCanvas sender, SKSurface args)
		{
			var sourcePaint = _renderConfiguration.ShowObject ? _sourceFillPaint : _emptyFillPaint;

			var squareSize = GetSquareSize(sender);
			var topLeft = GetRenderTopLeft(sender);

			if (_scene is BitmapScenePreset bitmapPreset)
			{
				var key = bitmapPreset.Name;
				if (_demoBitmaps.ContainsKey(key))
				{
					args.Canvas.DrawBitmap(_demoBitmaps[key], new SKRect(0, 0, _demoBitmaps[key].Width, _demoBitmaps[key].Height), new SKRect(topLeft.X, topLeft.Y, topLeft.X + squareSize, topLeft.Y + squareSize));
				}
			}

			foreach (var shape in _scene)
			{
				var paint = shape.State switch
				{
					CellState.Source => sourcePaint,
					CellState.Empty => _emptyFillPaint,
					CellState.Wall => _wallFillPaint,
					_ => throw new InvalidOperationException()
				};

				if (shape is Circle circle)
				{
					var width = squareSize;
					var height = squareSize;

					var centerX = width * circle.Center.X;
					var centerY = height * circle.Center.Y;

					var dimension = Math.Min(width, height);

					var radius = circle.Radius * dimension;

					if (circle.Radius < 0.0001)
					{
						// Fake point
						radius = 2;
					}

					args.Canvas.DrawCircle(topLeft.X + centerX, topLeft.Y + centerY, radius, paint);
				}
				else if (shape is Rectangle rectangle)
				{
					var width = squareSize;
					var height = squareSize;

					var top = height * rectangle.TopLeft.Y;
					var bottom = height * rectangle.BottomRight.Y;
					var left = width * rectangle.TopLeft.X;
					var right = width * rectangle.BottomRight.X;

					args.Canvas.DrawRect(topLeft.X + left, topLeft.Y + top, right - left, bottom - top, paint);
				}
			}
		}

		protected Dictionary<string, SKBitmap> _demoBitmaps = new Dictionary<string, SKBitmap>();

		private CompositeDisposable _bitmapsDisposable = new CompositeDisposable();

		public override void Initialized(ISkiaCanvas sender, SKSurface args)
		{
			// TODO: Allow async initialization in Skia

			// Load assets
			var demoAssetsPath = "Assets/Demo/";

			foreach (var demo in DemoScenarios.Scenarios)
			{
				_demoBitmaps.Add(demo.Key, LoadImageFromPackage($"{demoAssetsPath}{demo.Key}.png").DisposeWith(_bitmapsDisposable));
			}
		}

		public override void Dispose()
		{
			_bitmapsDisposable.Dispose();
		}
	}
}
