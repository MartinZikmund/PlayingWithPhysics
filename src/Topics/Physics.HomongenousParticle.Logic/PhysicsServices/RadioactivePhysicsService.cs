using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomongenousParticle.Logic.PhysicsServices
{
    public class RadioactivePhysicsService : IPhysicsService
    {
        private readonly RadioactiveMotionSetup _motionSetup;

        public RadioactivePhysicsService(RadioactiveMotionSetup motionSetup)
        {
            _motionSetup = motionSetup;
        }

        public float MaxT => throw new NotImplementedException();

        public float ComputeX(float seconds)
        {
            throw new NotImplementedException();
        }

        public float ComputeY(float seconds)
        {
            throw new NotImplementedException();
        }
    }
}
