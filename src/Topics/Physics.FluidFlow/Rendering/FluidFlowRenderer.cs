using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.FluidFlow.Logic;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.FluidFlow.Rendering
{
	public abstract class FluidFlowRenderer : ISkiaVariantRenderer
	{
		protected readonly FluidFlowCanvasController _controller;
		protected readonly ISkiaCanvas _canvas;

		private float _horizontalPadding;

		private float _pixelsPerUnitX;
		private float _pixelsPerUnitY;

		private SKPaint[] _particlePaints;

		public FluidFlowRenderer(FluidFlowCanvasController controller)
		{
			_controller = controller;
			_canvas = controller.Canvas;

			CreatePaints();
		}

		private void CreatePaints()
		{
			var colors = new[]
			{
				"#0063B1",
				"#E81123",
				"#2D7D9A",
				"#881798",
				"#498205",
				"#515C6B"
			};

			var paints = new List<SKPaint>();
			foreach(var colorHex in colors)
			{
				var color = colorHex.ToSKColor();
				paints.Add(new SKPaint()
				{
					IsStroke = false,
					IsAntialias = true,
					FilterQuality = SKFilterQuality.High,
					Color = color
				});
			}
			_particlePaints = paints.ToArray();
		}

		public abstract IPhysicsService PhysicsService { get; }

		internal abstract void StartSimulation(SceneConfiguration sceneConfiguration);

		public void Dispose() { }

		public void Update(ISkiaCanvas sender)
		{
			if (PhysicsService == null)
			{
				return;
			}

			_horizontalPadding = _canvas.ScaledSize.Width / 10;

			_pixelsPerUnitX = ((float)_canvas.ScaledSize.Width - 2 * _horizontalPadding) / PhysicsService.XMax;
			_pixelsPerUnitY = ((float)_canvas.ScaledSize.Height / 2) / (PhysicsService.YMax * 2);
		}

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));
			if (PhysicsService == null)
			{
				return;
			}

			var time = (float)_controller.SimulationTime.TotalTime.TotalSeconds;
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				var position = PhysicsService.GetParticlePosition(time, particleId);
				var x = GetRenderX((float)position.X);
				var y = GetRenderY((float)position.Y);
				args.Canvas.DrawCircle(x, y, 4, _particlePaints[particleId % _particlePaints.Length]);
			}			
		}

		private float GetRenderX(float x) => _horizontalPadding + x * _pixelsPerUnitX;

		private float GetRenderY(float y) => _canvas.ScaledSize.Height / 2  - y * _pixelsPerUnitY;
	}
}
