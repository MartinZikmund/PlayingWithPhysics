using System;
using Physics.FluidFlow.Logic;
using SkiaSharp;

namespace Physics.FluidFlow.Rendering
{
	public class ContinuityEquationRenderer : FluidFlowRenderer
	{
		private ContinuityEquationPhysicsService _physicsService;
		private SceneConfiguration _sceneConfiguration = null;

		public ContinuityEquationRenderer(FluidFlowCanvasController controller) : base(controller)
		{
		}

		protected override SKPath GetPlumbingPath()
		{
			if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.Equal)
			{
				var diameter = _sceneConfiguration.Diameter1;
				var top = GetRenderY(-diameter / 2);
				var bottom = GetRenderY(diameter / 2);
				var left = -10;
				var right = _canvas.ScaledSize.Width + 10;

				var path = new SKPath();
				path.MoveTo(left, top);
				path.LineTo(right, top);
				path.LineTo(right, bottom);
				path.LineTo(left, bottom);
				path.Close();
				return path;
			}
			else if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.S1Larger)
			{
				var diameter1 = _sceneConfiguration.Diameter1;
				var diameter2 = _sceneConfiguration.Diameter2;
				var startX = -10;
				var endX = _canvas.ScaledSize.Width + 10;
				var x1 = GetRenderX(_physicsService.GetS1LargerX1());
				var x2 = GetRenderX(_physicsService.GetS1LargerX2());
				var x3 = GetRenderX(_physicsService.GetS1LargerX3());
				var d1Top = GetRenderY(diameter1 / 2);
				var d1bottom = GetRenderY(-diameter1 / 2);
				var d2Top = GetRenderY(diameter2 / 2);
				var d2bottom = GetRenderY(-diameter2 / 2);

				var path = new SKPath();
				path.MoveTo(startX, d1Top);
				path.LineTo(x1, d1Top);
				path.LineTo(x2, d2Top);
				path.LineTo(endX, d2Top);
				path.LineTo(endX, d2bottom);
				path.LineTo(x2, d2bottom);
				path.LineTo(x1, d1bottom);
				path.LineTo(startX, d1bottom);
				path.Close();

				return path;
			}
			else
			{
				var diameter1 = _sceneConfiguration.Diameter1;
				var diameter2 = _sceneConfiguration.Diameter2;
				var startX = -10;
				var endX = _canvas.ScaledSize.Width + 10;
				var x1 = GetRenderX(_physicsService.GetS2LargerX1());
				var x2 = GetRenderX(_physicsService.GetS2LargerX2());
				var d1Top = GetRenderY(diameter1 / 2);
				var d1bottom = GetRenderY(-diameter1 / 2);
				var d2Top = GetRenderY(diameter2 / 2);
				var d2bottom = GetRenderY(-diameter2 / 2);

				var path = new SKPath();
				path.MoveTo(startX, d1Top);
				path.LineTo(x1, d1Top);
				path.LineTo(x2, d2Top);
				path.LineTo(endX, d2Top);
				path.LineTo(endX, d2bottom);
				path.LineTo(x2, d2bottom);
				path.LineTo(x1, d1bottom);
				path.LineTo(startX, d1bottom);
				path.Close();

				return path;
			}
		}

		internal override void StartSimulation(SceneConfiguration sceneConfiguration)
		{
			base.StartSimulation(sceneConfiguration);
			_sceneConfiguration = sceneConfiguration;
			_physicsService = new ContinuityEquationPhysicsService(sceneConfiguration);
		}

		protected override float GetVelocityVectorSize(int vectorId, int particleId)
		{
			if (vectorId >= 3)
			{
				return 0;
			}

			var range = (MaxRenderX - MinRenderX) / 5;
			if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.Equal)
			{
				var relativeVelocity = 1f;
				if (_sceneConfiguration.Fluid == FluidDefinitions.Oil)
				{
					relativeVelocity = _sceneConfiguration.Velocity / 4;
				}
				else
				{
					relativeVelocity = _sceneConfiguration.Velocity / 50;
				}
				return relativeVelocity * range;
			}
			else if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.S1Larger)
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
