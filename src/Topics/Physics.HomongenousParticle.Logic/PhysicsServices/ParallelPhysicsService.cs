using Physics.HomogenousParticle.Services;
using Physics.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomongenousParticle.Logic.PhysicsServices
{
    public class ParallelPhysicsService : IPhysicsService
    {
        private readonly ParallelMotionSetup _parallelMotionSetup;

        public ParallelPhysicsService(ParallelMotionSetup parallelMotionSetup)
        {
            _parallelMotionSetup = parallelMotionSetup;
        }

        public float MaxT => 30;

        public double ComputeX(double seconds)
        {
            var actualVelocity = _parallelMotionSetup.Velocity;
            var angular = Math.Cos(MathHelpers.DegreesToRadians(_parallelMotionSetup.InductionOrientation));
            var xSize = (float)(actualVelocity * seconds * angular);
            return _parallelMotionSetup.Angle == ParallelVariantOrientation.Normal ?
                xSize : -xSize;
        }

        public double ComputeY(double seconds)
        {
            var actualVelocity = _parallelMotionSetup.Velocity;
            var angular = Math.Sin(-MathHelpers.DegreesToRadians(_parallelMotionSetup.InductionOrientation));
            var ySize = (float)(actualVelocity * seconds * angular);
            return _parallelMotionSetup.Angle == ParallelVariantOrientation.Normal ?
                ySize : -ySize;
        }
    }
}
