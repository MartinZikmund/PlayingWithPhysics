using System;
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
		public Random _randomizer = new Random();
		List<SKPoint> _initialSignificantPoints = new List<SKPoint>();
		private double _lastRadius = 0;

		public EasyVariantRenderer(HuygensPrincipleCanvasController controller)
		{
			_controller = controller;
		}

		public void Dispose() { }

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_controller._manager == null)
			{
				args.Canvas.Clear(new SKColor(255, 255, 255, 255));
				return;
			}

			args.Canvas.Clear(new SKColor(255, 255, 255, 255));

			if (!_controller._drawingState.IsDrawing)
			{
				var topLeft = _controller.GetRenderTopLeft(sender);
				var size = _controller.GetSquareSize(sender);
				var radius = (float)_controller.SimulationTime.TotalTime.TotalSeconds * 10;
				_lastRadius = radius;

				var maxRadius = Math.Sqrt(sender.ScaledSize.Width * sender.ScaledSize.Width + sender.ScaledSize.Height * sender.ScaledSize.Height);
				var filtering = 1;

				//TODO: Make sure this does not skip any shape (like a single point) - maybe take filter sources by each shape?
				// Filtering could take into account distance of points - if we already rendered a point close to this one, no need to render it
				if (_primarySources.Count > 30)
				{
					if (radius > maxRadius * 0.75)
					{
						filtering = 10;
					}
					else if (radius > maxRadius * 0.5)
					{
						filtering = 5;
					}
					else if (radius > maxRadius * 0.25)
					{
						filtering = 2;
					}
				}

				if (_controller._renderConfiguration.ShowWaveEdge)
				{
					foreach (var point in _primarySources.Where((x, i) => i % filtering == 0))
					{
						args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, radius, _controller._waveEdgeStrokePaint);
					}
				}

				var wavePaint = _controller._renderConfiguration.ShowWave ? _controller._waveFillPaint : _controller._emptyFillPaint;
				foreach (var point in _primarySources.Where((x, i) => i % filtering == 0))
				{
					args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, radius, wavePaint);
				}

				if (_controller._renderConfiguration.ShowSignificantPoints)
				{
					if (!_controller._scene.Redrawn)
					{
						foreach (var point in _controller._scene.SignificantPoints)
						{
							args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, 2, _controller._significantPointPaint);
							args.Canvas.DrawCircle(point.X * size + topLeft.X, point.Y * size + topLeft.Y, radius, _controller._significantPointEllipsePaint);
						}
					}
					else
					{
						for (int i = 0; i < _initialSignificantPoints.Count; i++)
						{
							args.Canvas.DrawCircle(_initialSignificantPoints[i].X * size + topLeft.X, _initialSignificantPoints[i].Y * size + topLeft.Y, 2, _controller._significantPointPaint);
							args.Canvas.DrawCircle(_initialSignificantPoints[i].X * size + topLeft.X, _initialSignificantPoints[i].Y * size + topLeft.Y, radius, _controller._significantPointEllipsePaint);
						}
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

			if (_primarySources.Count > 0)
			{
				_initialSignificantPoints = Enumerable.Range(0, 10).Select(_ => _primarySources[_randomizer.Next(_primarySources.Count)]).ToList();
			}

		}

		public void Update(ISkiaCanvas sender)
		{
			if (_controller._manager == null)
			{
				return;
			}


			if (_controller.IsPaused)
			{
				sender.ShouldRender = false;
			}

			var radius = (float)_controller.SimulationTime.TotalTime.TotalSeconds * 10;
			var maxRadius = Math.Sqrt(sender.ScaledSize.Width * sender.ScaledSize.Width + sender.ScaledSize.Height * sender.ScaledSize.Height);
			if (radius > maxRadius && _lastRadius > maxRadius)
			{
				sender.ShouldRender = false;
			}
		}
	}
}
