using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Physics.ElectricParticle.Game;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Gaming.Input;
using Windows.System;
using Windows.UI.Core;

namespace Physics.ElectricParticle.Rendering
{
	public class GameCanvasController : SkiaCanvasController
	{
		private readonly KeyboardState _keyboardState;

		private readonly SKPaint _penUpPoint = new SKPaint()
		{
			Color = SKColors.Black,
			IsAntialias = true,
			IsStroke = true
		};

		private readonly SKPaint _penDownPoint = new SKPaint()
		{
			Color = SKColors.Black,
			IsAntialias = true,
			IsStroke = false
		};

		private readonly SKPaint _backgroundPaint = new SKPaint()
		{
			Color = new SKColor(0, 0, 0, 60),
			IsAntialias = true,
		};

		private readonly SKPaint _pathPaint = new SKPaint()
		{
			Color = SKColors.Black,
			IsAntialias = true,
			StrokeWidth = 4,
			IsStroke = true,
		};

		private SKBitmap[] _backgrounds;

		private Size _previousSize = Size.Empty;
		private SKBitmap _existingDrawing = null;
		private (DrawingPath, int) _lastDrawnPoint = (null, -1);

		public GameCanvasController(ISkiaCanvas control, KeyboardState keyboardState) : base(control)
		{
			_keyboardState = keyboardState;
		}

		public GameInfo GameInfo { get; internal set; }

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (GameInfo == null)
			{
				return;
			}
			DrawBackground(sender, args);
			DrawDrawing(sender, args);
			DrawPenPoint(sender, args);
		}

		private void DrawPenPoint(ISkiaCanvas sender, SKSurface args)
		{
			var x = sender.ScaledSize.Width * GameInfo.PenState.Position.X;
			var y = sender.ScaledSize.Height * GameInfo.PenState.Position.Y;
			args.Canvas.DrawCircle(x, y, 4, GameInfo.IsPenDown ? _penDownPoint : _penUpPoint);
		}

		private void DrawDrawing(ISkiaCanvas sender, SKSurface args)
		{
			if (_existingDrawing == null)
			{
				_existingDrawing = new SKBitmap((int)sender.NativeSize.Width, (int)sender.NativeSize.Height);
				using var canvas = new SKCanvas(_existingDrawing);
				// Need to draw everything again
				foreach (var drawingPath in GameInfo.Drawing.Paths.Where(p => p.IsClosed))
				{
					using var path = new SKPath();
					if (drawingPath.Points.Any())
					{
						path.MoveTo(RelativeToCanvas(drawingPath.Points[0], sender));
					}
					for (int i = 1; i < drawingPath.Points.Count; i++)
					{
						var point = drawingPath.Points[i];
						path.LineTo(RelativeToCanvas(point, sender));
					}
					canvas.DrawPath(path, _pathPaint);
				}
			}

			using var inProgressCanvas = new SKCanvas(_existingDrawing);
			foreach (var inProgressPath in GameInfo.Drawing.Paths.Where(p => !p.IsClosed))
			{
				int startIndex = _lastDrawnPoint.Item1 == inProgressPath ? _lastDrawnPoint.Item2 : 0;
				using var path = new SKPath();
				if (inProgressPath.Points.Any())
				{
					path.MoveTo(RelativeToCanvas(inProgressPath.Points[startIndex], sender));
				}
				for (int i = startIndex; i < inProgressPath.Points.Count; i++)
				{
					var point = inProgressPath.Points[i];
					path.LineTo(RelativeToCanvas(point, sender));
				}
				_lastDrawnPoint = (inProgressPath, inProgressPath.Points.Count - 1);
				inProgressCanvas.DrawPath(path, _pathPaint);
			}

			args.Canvas.DrawBitmap(_existingDrawing, SKPoint.Empty, _pathPaint);

			if (GameInfo.IsPenDown = true && GameInfo.State == GameState.Drawing && GameInfo.Drawing.Paths.Any())
			{
				var lastPoint = GameInfo.Drawing.Paths.Last().Points.LastOrDefault();
				if (lastPoint != null)
				{
					// Line to pen position for better visuals
					args.Canvas.DrawLine(RelativeToCanvas(lastPoint, sender), RelativeToCanvas(GameInfo.PenState.Position, sender), _pathPaint);
				}
			}
		}

		private SKPoint RelativeToCanvas(Vector2 point, ISkiaCanvas canvas)
		{
			return new SKPoint(point.X * canvas.ScaledSize.Width, point.Y * canvas.ScaledSize.Height);
		}

		public override void Update(ISkiaCanvas sender)
		{
			if (GameInfo == null)
			{
				return;
			}

			var newSize = new Size(sender.ScaledSize.Width, sender.ScaledSize.Height);
			if (newSize != _previousSize)
			{
				// Clear drawing cache, need full redraw
				_existingDrawing = null;
				_lastDrawnPoint = (null, -1);
			}
			_previousSize = newSize;

			EnsureBitmaps();

			HandleKeyboardInput();
			HandleGamepadInput();

			if ((GameInfo.Ux != 0 || GameInfo.Uy != 0) && GameInfo.State == GameState.Idle)
			{
				Play();
				GameInfo.Start();
			}

			if (GameInfo.State == GameState.Drawing)
			{
				GameInfo.UpdatePenPosition((float)SimulationTime.ElapsedTime.TotalSeconds);
			}
		}

		private void DrawBackground(ISkiaCanvas sender, SKSurface args)
		{
			var bitmap = _backgrounds[GameInfo.Level];
			args.Canvas.DrawBitmap(
				bitmap,
				new SKRect(0, 0, bitmap.Width, bitmap.Height),
				new SKRect(0, 0, sender.ScaledSize.Width, sender.ScaledSize.Height),
				_backgroundPaint);
		}

		private void EnsureBitmaps()
		{
			if (_backgrounds == null)
			{
				var backgrounds = new List<SKBitmap>();
				var gameAssetsPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Game", "Shapes");
				for (int i = 0; i < 11; i++)
				{
					var background = SKBitmap.Decode(Path.Combine(gameAssetsPath, $"Shape_{i + 1}.png"));
					backgrounds.Add(background);
				}
				_backgrounds = backgrounds.ToArray();
			}
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private void HandleKeyboardInput()
		{
			if (_keyboardState.IsUpPressed)
			{
				GameInfo.Uy += 1;
			}

			if (_keyboardState.IsDownPressed)
			{
				GameInfo.Uy -= 1;
			}

			if (_keyboardState.IsLeftPressed)
			{
				GameInfo.Ux -= 1;
			}

			if (_keyboardState.IsRightPressed)
			{
				GameInfo.Ux += 1;
			}
		}

		private void HandleGamepadInput()
		{
			var gamepadReading = Gamepad.Gamepads.FirstOrDefault()?.GetCurrentReading();
			if (gamepadReading != null)
			{
				GameInfo.Uy = (int)MathHelpers.Clamp(gamepadReading.Value.LeftThumbstickY * 100, -100, 100);
				GameInfo.Ux = (int)MathHelpers.Clamp(gamepadReading.Value.LeftThumbstickX * 100, -100, 100);
			}
		}
	}
}
