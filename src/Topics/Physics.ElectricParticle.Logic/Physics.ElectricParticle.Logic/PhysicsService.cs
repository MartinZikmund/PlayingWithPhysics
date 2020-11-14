using Physics.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.ElectricParticle.Logic
{
    public class PhysicsService : IPhysicsService
    {
        private readonly MotionSetup _motionSetup;

        public PhysicsService(MotionSetup motionSetup)
        {
            _motionSetup = motionSetup;
        }

        public float MaxT => 30;

        public double ComputeX(double seconds)
        {
            //var actualVelocity = _motionSetup.Velocity;
            //var angular = Math.Cos(MathHelpers.DegreesToRadians(_motionSetup.InductionOrientation));
            //var xSize = (float)(actualVelocity * seconds * angular);
            //return _motionSetup.Angle == ParallelVariantOrientation.Normal ?
            //    xSize : -xSize;
            return 0;
        }

        public double ComputeY(double seconds)
        {
            //var actualVelocity = _motionSetup.Velocity;
            //var angular = Math.Sin(-MathHelpers.DegreesToRadians(_motionSetup.InductionOrientation));
            //var ySize = (float)(actualVelocity * seconds * angular);
            //return _motionSetup.Angle == ParallelVariantOrientation.Normal ?
            //    ySize : -ySize;
            return 0;
        }
    }
}