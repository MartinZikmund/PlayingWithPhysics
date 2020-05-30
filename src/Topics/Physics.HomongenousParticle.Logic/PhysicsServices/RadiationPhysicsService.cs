using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomongenousParticle.Logic.PhysicsServices
{
    public class RadiationPhysicsService : IPhysicsService
    {
        private readonly RadiationMotionSetup _motionSetup;

        public RadiationPhysicsService(RadiationMotionSetup motionSetup)
        {
            _motionSetup = motionSetup;
        }

        public float MaxT => throw new NotImplementedException();

        public double ComputeX(double seconds)
        {
            throw new NotImplementedException();
        }

        public double ComputeY(double seconds)
        {
            throw new NotImplementedException();
        }
    }
}
