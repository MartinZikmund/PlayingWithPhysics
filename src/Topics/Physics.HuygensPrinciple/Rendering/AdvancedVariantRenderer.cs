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
		private readonly Random _randomizer = new Random();

		private SKBitmap _fieldImage;
		private SKBitmap _waveBorderImage;
		private SKBitmap _significantPointImage;

		private TimeSpan _lastUpdate = new TimeSpan();

		private StepInfo _lastStep = null;

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

			if ((_controller.SimulationTime.TotalTime - _lastUpdate).TotalMilliseconds > 1000)
			{
				var step = _controller._manager.NextStep();
				if (step != null)
				{
					DrawStep(step);
					_lastStep = step;
					_lastUpdate = _controller.SimulationTime.TotalTime;
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

			DrawSignificantPoints(_lastStep);
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
				if (_controller._renderConfiguration.ShowSignificantPoints)
				{
					args.Canvas.DrawBitmap(_significantPointImage, new SKRect(topLeft.X, topLeft.Y, topLeft.X + squareSize, topLeft.Y + squareSize));
				}
			}
			_controller.DrawInitalScene(sender, args);
		}

		private void DrawFullField()
		{
			for (int x = 0; x < _controller._manager.FieldWidth; x++)
			{
				for (int y = 0; y < _controller._manager.FieldHeight; y++)
				{
					_fieldImage.SetPixel(x, y, GetPixelColor(_controller._manager.CurrentField[x, y]));
				}
			}
		}

		private void DrawSignificantPoints(StepInfo step)
		{
			using SKCanvas canvas = new SKCanvas(_significantPointImage);
			canvas.Clear(SKColors.Transparent);
			if (_lastStep == null || _lastStep.WaveBorder.Length == 0)
			{
				foreach (var point in _controller._scene.SignificantPoints)
				{
					canvas.DrawCircle(point.X * _controller._manager.FieldWidth, point.Y * _controller._manager.FieldWidth, 2, _controller._significantPointPaint);
					canvas.DrawCircle(point.X * _controller._manager.FieldHeight, point.Y * _controller._manager.FieldHeight, _controller._manager.StepRadius, _controller._significantPointEllipsePaint);
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					var random = step.WaveBorder[_randomizer.Next(step.WaveBorder.Length)];
					canvas.DrawCircle(random.X, random.Y, 2, _controller._significantPointPaint);
					canvas.DrawCircle(random.X, random.Y, _controller._manager.StepRadius, _controller._significantPointEllipsePaint);
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
			_lastStep = null;
			_fieldImage?.Dispose();
			_fieldImage = new SKBitmap(_controller._manager.FieldWidth, _controller._manager.FieldHeight);
			_waveBorderImage?.Dispose();
			_waveBorderImage = new SKBitmap(_controller._manager.FieldWidth, _controller._manager.FieldHeight); 
			_significantPointImage = new SKBitmap(_controller._manager.FieldWidth, _controller._manager.FieldHeight); 
			using SKCanvas canvas = new SKCanvas(_fieldImage);
			canvas.Clear(SKColors.White);
		}
	}
}
