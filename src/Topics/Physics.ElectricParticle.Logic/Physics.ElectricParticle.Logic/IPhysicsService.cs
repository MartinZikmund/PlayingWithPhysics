using ExtendedNumerics;
using Physics.Shared.Math;

namespace Physics.ElectricParticle.Logic
{
	interface IPhysicsService
    {
		BigNumber ComputeX(BigNumber time);

		BigNumber ComputeY(BigNumber time);
    }
}
