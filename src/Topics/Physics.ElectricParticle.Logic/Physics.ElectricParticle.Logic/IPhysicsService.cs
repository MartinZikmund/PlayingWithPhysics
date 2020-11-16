using System;
using System.Collections.Generic;
using System.Text;
using ExtendedNumerics;

namespace Physics.ElectricParticle.Logic
{
    interface IPhysicsService
    {
		BigDecimal ComputeX(BigDecimal seconds);

		BigDecimal ComputeY(BigDecimal seconds);

        BigDecimal MaxT { get; }
    }
}
