using Physics.Shared.Mathematics;

namespace Physics.ElectricParticle.Logic
{
	interface IPhysicsService
    {
		BigNumber ComputeX(BigNumber time);

		BigNumber ComputeY(BigNumber time);
    }
}
