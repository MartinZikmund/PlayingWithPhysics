using System;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.HuygensPrinciple.Rendering
{
	public class HuygensPrincipleCanvasController : SkiaCanvasController
	{
		internal ScenePreset _scene = null;
		internal HuygensManager _manager = null;

		internal readonly SKColor _waveColor = SKColors.Aqua;
		internal readonly SKColor _waveEdgeColor = SKColors.Blue;
		internal readonly SKColor _emptyColor = SKColors.White;
		internal readonly SKColor _wallColor = SKColors.Brown;
		internal readonly SKColor _sourceColor = SKColors.Orange;

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

		internal readonly SKPaint _sourceFillPaint = new SKPaint()
		{
			IsStroke = false,
			IsAntialias = true
		};

		public HuygensPrincipleCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
			_waveFillPaint.Color = _waveColor;
			_wallFillPaint.Color = _wallColor;
			_sourceFillPaint.Color = _sourceColor;
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
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			Renderer?.Draw(sender, args);
		}

		internal void DrawInitalScene(SKBitmap bitmap)
		{
			using SKCanvas canvas = new SKCanvas(bitmap);

			foreach (var shape in _scene)
			{
				if (shape is Circle circle)
				{
					var width = bitmap.Width;
					var height = bitmap.Height;

					var centerX = width * circle.Center.X;
					var centerY = height * circle.Center.Y;

					var dimension = Math.Min(width, height);
					var radius = circle.Radius * dimension;

					canvas.DrawCircle(centerX, centerY, radius, circle.State == CellState.Source ? _sourceFillPaint : _wallFillPaint);
				}
				else if (shape is Rectangle rectangle)
				{
					var width = bitmap.Width;
					var height = bitmap.Height;

					var top = height * rectangle.TopLeft.Y;
					var bottom = height * rectangle.BottomRight.Y;
					var left = width * rectangle.TopLeft.X;
					var right = width * rectangle.BottomRight.X;

					canvas.DrawRect(left, top, right - left, bottom - top, rectangle.State == CellState.Source ? _sourceFillPaint : _wallFillPaint);
				}
			}
		}
	}
}
