using Physics.FluidFlow.Logic;

namespace Physics.FluidFlow.Rendering
{
	public class BernoulliEquationWithoutHeightDecreaseRenderer : FluidFlowRenderer
	{
		private BernoulliWithHeightChangePhysicsService _physicsService;
		private SceneConfiguration _sceneConfiguration;

		public BernoulliEquationWithoutHeightDecreaseRenderer(FluidFlowCanvasController controller) :
			base(controller)
		{
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
