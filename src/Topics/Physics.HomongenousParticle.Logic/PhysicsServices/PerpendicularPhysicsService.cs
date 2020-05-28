using Physics.HomogenousParticle.Services;
using System;

namespace Physics.HomongenousParticle.Logic.PhysicsServices
{
    public class PerpendicularPhysicsService : IPhysicsService
    {
        private readonly PerpendicularMotionSetup _perpenducilarMotion;

        public PerpendicularPhysicsService(PerpendicularMotionSetup perpenducilarMotion)
        {
            _perpenducilarMotion = perpenducilarMotion;
        }

        private void Calc()
        {
            //
            //var y =
            //    radius *
            //    Math.Sin(-(Math.PI / 2) * t - (3 * Math.PI / 2) + angle) +
            //    radius *
            //    Math.Cos(angle - Math.PI / 2);
            //return new Vector2((float)x, (float)y);
        }

        public float MaxT => throw new NotImplementedException();

        public double ComputeX(float seconds)
        {
            if (_perpenducilarMotion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper)
            {
                return
                    ComputeRadius() *
                    Math.Cos(ComputeOmega() * seconds - Math.PI / 2);
            }
            else
            {
                return
                    ComputeRadius() *
                    Math.Cos(-ComputeOmega() * seconds - (3 * Math.PI / 2));
            }
        }

        public double ComputeY(float seconds)
        {
            var r = ComputeRadius();
            if (_perpenducilarMotion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper)
            {
                return
                    r *
                    Math.Sin(ComputeOmega() * seconds - Math.PI / 2) +
                    r;
            }
            else
            {
                return
                    r *
                    Math.Cos(-ComputeOmega() * seconds - (3 * Math.PI / 2)) -
                    r;
            }
        }

        public double ComputeRadius()
        {
            var mv = _perpenducilarMotion.MassMultiple * _perpenducilarMotion.VelocityMultiple;
            var Bq = _perpenducilarMotion.Induction * _perpenducilarMotion.ChargeMultiple * 1.6;
            var fraction = mv / Bq;
            var tenMultiple = (-27 + 6) - (0 - 19);
            var tenPow = Math.Pow(10, tenMultiple);
            return fraction * tenPow;
        }

        public double ComputeOmega()
        {
            return _perpenducilarMotion.VelocityMultiple * Math.Pow(10, 6) / ComputeRadius();
        }

        public double ComputeT()
        {
            return ComputeOmega() / (2 * Math.PI);
        }
    }
}
