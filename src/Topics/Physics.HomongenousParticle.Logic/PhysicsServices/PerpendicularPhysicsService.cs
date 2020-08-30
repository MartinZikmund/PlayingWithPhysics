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

        public float MaxT => (float)ComputeT();

        public double ComputeX(double seconds) => (double)ComputeXDecimal((decimal)seconds);

        public decimal ComputeXDecimal(decimal seconds)
        {
            if ((_perpenducilarMotion.InductionOrientation == PerpendicularInductionOrientation.FromPaper && _perpenducilarMotion.ChargeMultiple > 0) ||
                (_perpenducilarMotion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper && _perpenducilarMotion.ChargeMultiple < 0))
            {
                return
                    ComputeRadius() *
                    (decimal)Math.Cos((double)(ComputeOmega() * seconds - (decimal)Math.PI / 2));
            }
            else
            {
                return
                    ComputeRadius() *
                    (decimal)Math.Cos((double)(-ComputeOmega() * seconds - (3 * (decimal)Math.PI / 2)));
            }
        }

        public double ComputeY(double seconds) => (double)ComputeYDecimal((decimal)seconds);

        public decimal ComputeYDecimal(decimal seconds)
        {
            var r = ComputeRadius();
            if ((_perpenducilarMotion.InductionOrientation == PerpendicularInductionOrientation.FromPaper && _perpenducilarMotion.ChargeMultiple > 0) ||
                (_perpenducilarMotion.InductionOrientation == PerpendicularInductionOrientation.IntoPaper && _perpenducilarMotion.ChargeMultiple < 0))
            {
                return
                    r *
                    (decimal)Math.Sin((double)(ComputeOmega() * seconds - (decimal)Math.PI / 2)) +
                    r;
            }
            else
            {
                return
                    r *
                    (decimal)Math.Sin((double)(-ComputeOmega() * seconds - (3 * (decimal)Math.PI / 2))) -
                    r;
            }
        }

        public decimal ComputeRadius()
        {
            var mv = (decimal)_perpenducilarMotion.MassMultiple * 1.67m * (decimal)_perpenducilarMotion.VelocityMultiple;
            var Bq = (decimal)_perpenducilarMotion.Induction * (decimal)Math.Abs(_perpenducilarMotion.ChargeMultiple) * 1.6m;
            var fraction = mv / Bq;   
            var tenPow = 0.01m; //10 to power of (-27 + 6) - (0 - 19)
            return fraction * tenPow;
        }

        public decimal ComputeOmega()
        {
            var radius = ComputeRadius();
            return (decimal)_perpenducilarMotion.VelocityMultiple * 1000000 / radius;
        }

        public decimal ComputeT()
        {
            return (2 * (decimal)Math.PI) / ComputeOmega();
        }

        public decimal ComputeVelocity() => (decimal)_perpenducilarMotion.VelocityMultiple * 1000000;

        public double MinX() => -(double)ComputeRadius();

        public double MaxX() => (double)ComputeRadius();

        public double MinY() => -(double)ComputeRadius();

        public double MaxY() => (double)ComputeRadius();
    }
}
