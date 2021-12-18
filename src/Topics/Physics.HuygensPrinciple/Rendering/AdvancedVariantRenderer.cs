using System;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.HuygensPrinciple.Rendering
{
	public class AdvancedVariantRenderer : IHuygensVariantRenderer
	{
		private readonly HuygensPrincipleCanvasController _controller;

		private SKBitmap _fieldImage = new SKBitmap(RenderingConfiguration.FieldSize, RenderingConfiguration.FieldSize);

		private TimeSpan _lastUpdate = new TimeSpan();

		public AdvancedVariantRenderer(HuygensPrincipleCanvasController controller)
		{
			_controller = controller;
		}

		public void Update(ISkiaCanvas sender)
		{
			if (_controller._manager == null)
			{
				return;
			}

			if ((_controller.SimulationTime.TotalTime - _lastUpdate).TotalMilliseconds > 200)
			{
				_lastUpdate = _controller.SimulationTime.TotalTime;
				DrawStep(_controller._manager.NextStep());
			}
		}

		private void DrawStep(CellStateChange[] cellStateChanges)
		{
			foreach (var change in cellStateChanges)
			{
				_fieldImage.SetPixel(change.X, change.Y, GetPixelColor(change.NewState));
			}
		}

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_controller._manager == null)
			{
				return;
			}

			if (!_controller._drawingState.IsDrawing)
			{
				var squareSize = _controller.GetSquareSize(sender);
				var topLeft = _controller.GetRenderTopLeft(sender);
				args.Canvas.DrawBitmap(_fieldImage, new SKRect(topLeft.X, topLeft.Y, topLeft.X + squareSize, topLeft.Y + squareSize));
			}
			_controller.DrawInitalScene(sender, args);
		}

		private void DrawFullField()
		{
			for (int x = 0; x < RenderingConfiguration.FieldSize; x++)
			{
				for (int y = 0; y < RenderingConfiguration.FieldSize; y++)
				{
					_fieldImage.SetPixel(x, y, GetPixelColor(_controller._manager.CurrentField[x, y]));
				}
			}
		}

		private SKColor GetPixelColor(CellState cellState) =>
			cellState switch
			{
				CellState.Empty => _controller._emptyColor,
				CellState.Source => _controller._sourceColor,
				CellState.Wave => _controller._waveColor,
				CellState.Wall => _controller._wallColor,
				CellState.WaveEdge => _controller._waveEdgeColor,
				_ => _controller._emptyColor
			};

		public void Dispose() { }

		public void StartSimulation()
		{
			_lastUpdate = TimeSpan.Zero;
			using SKCanvas canvas = new SKCanvas(_fieldImage);
			canvas.Clear(SKColors.White);
		}
	}
}
