using System;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.HuygensPrinciple.Rendering
{
	public class AdvancedVariantRenderer : IHuygensVariantRenderer
	{
		private const int FieldWidth = 500;
		private const int FieldHeight = 500;

		private readonly HuygensPrincipleCanvasController _controller;

		private SKBitmap _fieldImage = new SKBitmap(FieldWidth, FieldHeight);

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

			args.Canvas.DrawBitmap(_fieldImage, new SKRect(0, 0, sender.ScaledSize.Width, sender.ScaledSize.Height));
		}

		private void DrawFullField()
		{
			for (int x = 0; x < FieldWidth; x++)
			{
				for (int y = 0; y < FieldHeight; y++)
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
			//_fieldImage.Reset();
			_controller.DrawInitalScene(_fieldImage);
		}
	}
}
