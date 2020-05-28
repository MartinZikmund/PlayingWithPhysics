using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.HomongenousParticle.Logic.PhysicsServices
{
    interface IPhysicsService
    {
        double ComputeX(float seconds);

        double ComputeY(float seconds);

        float MaxT { get; }
    }
}
