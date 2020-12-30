using System;
using System.Collections.Generic;
using System.Text;
using ExtendedNumerics;

namespace Physics.ElectricParticle.Logic
{
    interface IPhysicsService
    {
		BigDecimal ComputeX(double time);

		BigDecimal ComputeY(double time);

        BigDecimal MaxT { get; }
    }
}
