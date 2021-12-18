using System.Collections.Generic;
using System.Linq;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.HuygensPrinciple.Rendering
{
	public class EasyVariantRenderer : IHuygensVariantRenderer
	{
		private readonly HuygensPrincipleCanvasController _controller;
		private IList<SKPoint> _primarySources;

		public EasyVariantRenderer(HuygensPrincipleCanvasController controller)
		{
			_controller = controller;
		}

		public void Dispose() { }

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_controller._manager == null)
			{
				return;
			}

			if (!_controller._drawingState.IsDrawing)
			{
				var topLeft = _controller.GetRenderTopLeft(sender);
				var size = _controller.GetSquareSize(sender);
				var radius = (float)_controller.SimulationTime.TotalTime.TotalSeconds * 10;

				if (_controller._renderConfiguration.ShowWaveEdge)
				{
					foreach (var point in _primarySources)
					{
						args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, radius, _controller._waveEdgeStrokePaint);
					}
				}

				var wavePaint = _controller._renderConfiguration.ShowWave ? _controller._waveFillPaint : _controller._emptyFillPaint;
				foreach (var point in _primarySources)
				{
					args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, radius, wavePaint);
				}

				if (_controller._renderConfiguration.ShowSignificantPoints)
				{
					foreach (var point in _controller._scene.SignificantPoints)
					{
						args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, 2, _controller._significantPointPaint);
						args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, radius, _controller._significantPointEllipsePaint);
					}
				}
			}

			_controller.DrawInitalScene(sender, args);
		}

		public void StartSimulation()
		{
			var manager = _controller._manager;

			_primarySources = manager.GetBorderPoints(manager.OriginalField, Logic.CellState.Source)
				.Select(p => new SKPoint(p.X * 1.0f / (manager.FieldWidth - 1), p.Y * 1.0f / (manager.FieldHeight - 1)))
				.ToList();
		}

		public void Update(ISkiaCanvas sender)
		{
			if (_controller._manager == null)
			{
				return;
			}

			//if ((_controller.SimulationTime.TotalTime - _lastUpdate).TotalMilliseconds > 200)
			//{
			//	_lastUpdate = _controller.SimulationTime.TotalTime;
			//	DrawStep(_controller._manager.NextStep());
			//}
		}
	}
}
