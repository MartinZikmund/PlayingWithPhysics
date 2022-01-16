using System;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.UI.WebUI;

namespace Physics.HuygensPrinciple.Rendering
{
	public class AdvancedVariantRenderer : IHuygensVariantRenderer
	{
		private readonly HuygensPrincipleCanvasController _controller;

		private SKBitmap _fieldImage = new SKBitmap(RenderingConfiguration.FieldSize, RenderingConfiguration.FieldSize);
		private SKBitmap _waveBorderImage = new SKBitmap(RenderingConfiguration.FieldSize, RenderingConfiguration.FieldSize);

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

			if ((_controller.SimulationTime.TotalTime - _lastUpdate).TotalMilliseconds > 500)
			{
				_lastUpdate = _controller.SimulationTime.TotalTime;
				var step = _controller._manager.NextStep();
				if (step != null)
				{
					DrawStep(step);
				}
			}
		}

		private void DrawStep(StepInfo step)
		{
			foreach (var change in step.CellStateChanges)
			{
				_fieldImage.SetPixel(change.X, change.Y, GetPixelColor(change.NewState));
			}
			using SKCanvas canvas = new SKCanvas(_waveBorderImage);
			canvas.Clear(SKColors.Transparent);
			foreach (var change in step.WaveBorder)
			{
				canvas.DrawCircle(change.X, change.Y, 1, _controller._waveEdgeStrokePaint);
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
				if (_controller._renderConfiguration.ShowWaveEdge)
				{
					args.Canvas.DrawBitmap(_waveBorderImage, new SKRect(topLeft.X, topLeft.Y, topLeft.X + squareSize, topLeft.Y + squareSize));
				}
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
