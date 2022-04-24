using System;
using Physics.FluidFlow.Logic;
using SkiaSharp;

namespace Physics.FluidFlow.Rendering
{
	public class BernoulliEquationWithHeightChangeRenderer : FluidFlowRenderer
	{
		private BernoulliWithHeightChangePhysicsService _physicsService;
		private SceneConfiguration _sceneConfiguration = null;

		public BernoulliEquationWithHeightChangeRenderer(FluidFlowCanvasController controller) : base(controller)
		{
		}

		protected override SKPath GetPlumbingStrokePath() => GetPlumbingStroke(false);

		protected override SKPath GetPlumbingFillPath() => GetPlumbingStroke(true);

		private SKPath GetPlumbingStroke(bool isFill)
		{
			var diameter1 = _sceneConfiguration.Diameter1;
			var diameter2 = _sceneConfiguration.Diameter2;

			var heightChange = _sceneConfiguration.HeightChange;

			var startX = -10;
			var endX = _canvas.ScaledSize.Width + 10;
			var x1 = GetRenderX(_physicsService.GetS1LargerX1());

			var x2 = GetRenderX(_physicsService.GetS1LargerX2());
			var x3 = GetRenderX(_physicsService.GetS1LargerX3());


			var firstPartTopRealY = 0f;
			var firstPartBottomRealY = 0f;
			var secondPartTopRealY = 0f;
			var secondPartBottomRealY = 0f;
			if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.S1Larger)
			{
				firstPartTopRealY = _sceneConfiguration.HeightChange + diameter1;
				firstPartBottomRealY = _sceneConfiguration.HeightChange;
				secondPartTopRealY = diameter2;
				secondPartBottomRealY = 0f;
			}
			else
			{
				firstPartTopRealY = diameter1;
				firstPartBottomRealY = 0f;
				secondPartTopRealY = _sceneConfiguration.HeightChange + diameter2;
				secondPartBottomRealY = _sceneConfiguration.HeightChange;
			}

			var firstPartTopY = GetRenderY(firstPartTopRealY);
			var firstPartBottomY = GetRenderY(firstPartBottomRealY);
			var secondPartTopY = GetRenderY(secondPartTopRealY);
			var secondPartBottomY = GetRenderY(secondPartBottomRealY);

			var firstUpX1 = GetRenderX(0.095f);
			var firstUpX2 = GetRenderX(0.105f);
			var secondUpX1 = GetRenderX(0.395f);
			var secondUpX2 = GetRenderX(0.405f);

			var firstUpY = GetRenderY(firstPartTopRealY + _physicsService.H1);
			var secondUpY = GetRenderY(secondPartTopRealY + _physicsService.H2);

			var path = new SKPath();
			path.MoveTo(startX, firstPartTopY);
			path.LineTo(firstUpX1, firstPartTopY);
			path.LineTo(firstUpX1, firstUpY);
			if (isFill)
			{
				path.LineTo(firstUpX2, firstUpY);
			}
			else
			{
				path.MoveTo(firstUpX2, firstUpY);
			}
			path.LineTo(firstUpX2, firstPartTopY);
			path.LineTo(x1, firstPartTopY);
			path.LineTo(x2, secondPartTopY);
			path.LineTo(secondUpX1, secondPartTopY);
			path.LineTo(secondUpX1, secondUpY);
			if (isFill)
			{
				path.LineTo(secondUpX2, secondUpY);
			}
			else
			{
				path.MoveTo(secondUpX2, secondUpY);
			}
			path.LineTo(secondUpX2, secondPartTopY);
			path.LineTo(endX, secondPartTopY);
			path.LineTo(endX, secondPartBottomY);
			path.LineTo(x2, secondPartBottomY);
			path.LineTo(x1, firstPartBottomY);
			path.LineTo(startX, firstPartBottomY);

			if (isFill)
			{
				path.Close();
			}

			return path;
		}

		internal override void StartSimulation(SceneConfiguration sceneConfiguration)
		{
			base.StartSimulation(sceneConfiguration);
			_sceneConfiguration = sceneConfiguration;
			_physicsService = new BernoulliWithHeightChangePhysicsService(sceneConfiguration);
		}

		protected override float GetVelocityVectorSize(int vectorId, int particleId)
		{
			if (vectorId >= 3)
			{
				return 0;
			}

			var range = (MaxRenderX - MinRenderX) / 5;
			if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.S1Larger)
			{
				var unit = range / 6;
				if (vectorId == 0)
				{
					return unit;
				}
				else
				{
					var diameterDiff = _sceneConfiguration.Diameter1 / _sceneConfiguration.Diameter2;
					if (diameterDiff <= 10)
					{
						return 2 * unit;
					}
					else if (diameterDiff <= 100)
					{
						return 4 * unit;
					}
					else
					{
						return 6 * unit;
					}
				}
			}
			else if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.S2Larger)
			{
				var unit = range / 6;
				if (vectorId == 0)
				{
					return 6 * unit;
				}
				else
				{
					var diameterDiff = _sceneConfiguration.Diameter2 / _sceneConfiguration.Diameter1;
					if (diameterDiff <= 10)
					{
						return 4 * unit;
					}
					else if (diameterDiff <= 100)
					{
						return 2 * unit;
					}
					else
					{
						return 1 * unit;
					}
				}
			}
			return 0;
		}

		protected override void DrawTrajectory(SKCanvas canvas)
		{
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				using var path = new SKPath();
				var t1 = _physicsService.T1;
				var t2 = _physicsService.T2;
				var startPoint = PhysicsService.GetParticlePosition(0, particleId);
				path.MoveTo(GetRenderX((float)startPoint.X), GetRenderY((float)startPoint.Y));
				if (GetAdjustedTime() >= t1)
				{
					var firstBreakPosition = PhysicsService.GetParticlePosition(t1, particleId);
					path.LineTo(GetRenderX((float)firstBreakPosition.X), GetRenderY((float)firstBreakPosition.Y));
				}

				if (GetAdjustedTime() >= (t1 + t2))
				{
					var secondBreakPosition = PhysicsService.GetParticlePosition(t1 + t2, particleId);
					path.LineTo(GetRenderX((float)secondBreakPosition.X), GetRenderY((float)secondBreakPosition.Y));
				}

				var endPosition = PhysicsService.GetParticlePosition((float)GetAdjustedTime(), particleId);
				path.LineTo(GetRenderX((float)Math.Min(_physicsService.XMax, endPosition.X)), GetRenderY((float)endPosition.Y));

				canvas.DrawPath(path, GetParticlePathPaint(particleId));
			}
		}

		public override IPhysicsService PhysicsService => _physicsService;
	}
}
