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
			else
			{
				return new SKPath();
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

			var range = (MaxRenderX - MinRenderX) / 10;
			if (_sceneConfiguration.DiameterRelationType == DiameterRelationType.Equal)
			{
				var relativeVelocity = (_sceneConfiguration.Velocity / 50) * range;
				return relativeVelocity;
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

		public override IPhysicsService PhysicsService => _physicsService;
	}
}
