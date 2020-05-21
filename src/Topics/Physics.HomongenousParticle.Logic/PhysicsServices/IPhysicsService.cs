using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.HomongenousParticle.Logic.PhysicsServices
{
    interface IPhysicsService
    {
        float ComputeX(float seconds);

        float ComputeY(float seconds);

        float MaxT { get; }
    }
}
