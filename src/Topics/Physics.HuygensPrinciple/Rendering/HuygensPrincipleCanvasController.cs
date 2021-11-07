using System;
using System.Diagnostics;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.HuygensPrinciple.Rendering
{
	public class HuygensPrincipleCanvasController : SkiaCanvasController
	{
		private const int FieldWidth = 500;
		private const int FieldHeight = 500;

		private readonly SKColor _waveColor = SKColors.Aqua;
		private readonly SKColor _waveEdgeColor = SKColors.Blue;
		private readonly SKColor _emptyColor = SKColors.White;
		private readonly SKColor _wallColor = SKColors.Brown;
		private readonly SKColor _sourceColor = SKColors.Orange;

		private HuygensStepper _stepper = null;
		private SKBitmap _fieldImage = new SKBitmap(FieldWidth, FieldHeight);

		public HuygensPrincipleCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public void StartSimulation(ScenePreset scenePreset)
		{
			_stepper = new HuygensStepper(FieldWidth, FieldHeight, 5);
			scenePreset.Draw(_stepper);
			_stepper.SaveOriginalField();
			_stepper.PrecalculateSteps();
			DrawFullField();
			SimulationTime.Restart();
		}

		private TimeSpan _lastUpdate = new TimeSpan();

		public override void Update(ISkiaCanvas sender)
		{
			if (_stepper == null)
			{
				return;
			}

			if ((SimulationTime.TotalTime - _lastUpdate).TotalMilliseconds > 200)
			{
				_lastUpdate = SimulationTime.TotalTime;
				DrawStep(_stepper.NextStep());
			}
		}

		private void DrawStep(CellStateChange[] cellStateChanges)
		{
			foreach(var change in cellStateChanges)
			{
				_fieldImage.SetPixel(change.X, change.Y, GetPixelColor(change.NewState));
			}
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_stepper == null)
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
					_fieldImage.SetPixel(x, y, GetPixelColor(_stepper.CurrentField[x, y]));
				}
			}
		}

		private SKColor GetPixelColor(CellState cellState) =>
			cellState switch
			{
				CellState.Empty => _emptyColor,
				CellState.Source => _sourceColor,
				CellState.Wave => _waveColor,
				CellState.Wall => _wallColor,
				CellState.WaveEdge => _waveEdgeColor,
				_ => _emptyColor
			};
	}
}
