using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.HomongenousParticle.Logic.PhysicsServices
{
    interface IPhysicsService
    {
        float ComputeX();

        float ComputeY();

        float MaxT { get; }
    }
}
