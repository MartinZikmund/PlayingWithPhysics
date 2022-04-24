using System;

namespace Physics.FluidFlow.Logic;

public static class PhysicsServiceFactory
{
	public static IPhysicsService Create(SceneConfiguration sceneConfiguration) =>
		sceneConfiguration.InputVariant switch
		{
			InputVariant.ContinuityEquation => new ContinuityEquationPhysicsService(sceneConfiguration),
			InputVariant.BernoulliEquationWithoutHeightChange => new BernoulliWithoutHeightChangePhysicsService(sceneConfiguration),
			InputVariant.BernoulliEquationWithHeightChange => new BernoulliWithHeightChangePhysicsService(sceneConfiguration),
			InputVariant.RealFluidMovement => new RealFluidFlowPhysicsService(sceneConfiguration),
			_ => throw new InvalidOperationException()
		};
}
