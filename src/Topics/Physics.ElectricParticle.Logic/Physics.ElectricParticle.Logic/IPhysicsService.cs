using System;
using System.Collections.Generic;
using System.Text;
using ExtendedNumerics;

namespace Physics.ElectricParticle.Logic
{
    interface IPhysicsService
    {
		BigDecimal ComputeX(decimal time);

		BigDecimal ComputeY(decimal time);

        BigDecimal MaxT { get; }
    }
}
