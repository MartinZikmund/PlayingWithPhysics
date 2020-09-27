using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.ElectricParticle.Logic
{
    interface IPhysicsService
    {
        double ComputeX(double seconds);

        double ComputeY(double seconds);

        float MaxT { get; }
    }
}
