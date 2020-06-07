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
        private double Phase1EndTime = 2;

        private readonly RadiationMotionSetup _motionSetup;

        public RadiationPhysicsService(RadiationMotionSetup motionSetup)
        {
            _motionSetup = motionSetup;
        }

        public double MinX =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeX(MaxT),
                RadiationType.BetaPlus => ComputeX(MaxT),
                RadiationType.BetaMinus => ComputeX(0),
                RadiationType.Neutron => ComputeX(0),
                _ => throw new InvalidOperationException()
            };

        public double MinY =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeX(0),
                RadiationType.BetaPlus => ComputeX(0),
                RadiationType.BetaMinus => ComputeX(0),
                RadiationType.Neutron => ComputeY(0),
                _ => throw new InvalidOperationException()
            };

        public double MaxX =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeX(0),
                RadiationType.BetaPlus => ComputeX(0),
                RadiationType.BetaMinus => ComputeX(MaxT),
                RadiationType.Neutron => ComputeX(MaxT),
                _ => throw new InvalidOperationException()
            };

        public double MaxY =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeX(MaxT),
                RadiationType.BetaPlus => ComputeX(MaxT),
                RadiationType.BetaMinus => ComputeX(MaxT),
                RadiationType.Neutron => ComputeY(MaxT),
                _ => throw new InvalidOperationException()
            };

        public float MaxT => 15;

        public double ComputeX(double seconds) =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeParticleCoordinate(seconds, Phase1X, AlphaPhase2X, AlphaPhase3X, ComputeAlphaPhase3StartTime),
                RadiationType.BetaPlus => ComputeParticleCoordinate(seconds, Phase1X, BetaPlusPhase2X, BetaPlusPhase3X, ComputeBetaPhase3StartTime),
                RadiationType.BetaMinus => ComputeParticleCoordinate(seconds, Phase1X, BetaMinusPhase2X, BetaMinusPhase3X, ComputeBetaPhase3StartTime),
                RadiationType.Neutron => ComputeParticleCoordinate(seconds, NeutronX, NeutronX, NeutronX, ()=> Phase1EndTime),
                _ => throw new InvalidOperationException()
            };

        public double ComputeY(double seconds) =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeParticleCoordinate(seconds, Phase1Y, AlphaPhase2Y, AlphaPhase3Y, ComputeAlphaPhase3StartTime),
                RadiationType.BetaPlus => ComputeParticleCoordinate(seconds, Phase1Y, BetaPlusPhase2Y, BetaPlusPhase3Y, ComputeBetaPhase3StartTime),
                RadiationType.BetaMinus => ComputeParticleCoordinate(seconds, Phase1Y, BetaMinusPhase2Y, BetaMinusPhase3Y, ComputeBetaPhase3StartTime),
                RadiationType.Neutron => ComputeParticleCoordinate(seconds, NeutronY, NeutronY, NeutronY, () => Phase1EndTime),
                _ => throw new InvalidOperationException()
            };

        private double ComputeParticleCoordinate(
            double seconds, 
            Func<double, double> phase1,
            Func<double, double> phase2,
            Func<double, double> phase3,
            Func<double> phase3StartTime)
        {
            seconds = seconds - Phase1EndTime;
            if (seconds < Phase1EndTime)
            {
                return phase1(seconds);
            }
            if (seconds < phase3StartTime())
            {
                return phase2(seconds);
            }
            return phase3(seconds);
        }

        private double Phase1X(double seconds) => 0;

        private double Phase1Y(double seconds) => Math.PI / 3 * seconds;

        private double AlphaPhase2X(double seconds)
        {
            var r = 6;
            return r * Math.Cos(Math.PI / 18 * seconds) - r;
        }

        private double AlphaPhase2Y(double seconds)
        {
            var r = 6;
            return r * Math.Sin(Math.PI / 18 * seconds);
        }

        private double ComputeAlphaPhase3StartTime()
        {
            return 18 * Math.Acos(2.0 / 3) / Math.PI;
        }

        private double AlphaPhase3X(double seconds)
        {
            var t1 = ComputeAlphaPhase3StartTime();
            return -((Math.Sqrt(5) * Math.PI) / 9) * (seconds - t1);
        }

        private double AlphaPhase3Y(double seconds)
        {
            var t1 = 18 * Math.Acos(2.0 / 3) / Math.PI;
            return (2 * Math.PI) / 9 * (seconds - t1);
        }

        private double ComputeBetaPhase3StartTime()
        {
            return 3;
        }

        private double BetaPlusPhase2X(double seconds)
        {
            var r = 2;
            return r * Math.Cos(Math.PI / 6 * seconds) - r;
        }

        private double BetaPlusPhase2Y(double seconds)
        {
            var r = 2;
            return r * Math.Sin(Math.PI / 6 * seconds);
        }

        private double BetaPlusPhase3X(double seconds)
        {
            var t1 = 3;
            return -Math.PI / 3 * (seconds - t1);
        }

        private double BetaPlusPhase3Y(double seconds) => 2;

        private double BetaMinusPhase2X(double seconds)
        {
            var r = 2;
            return r * Math.Cos(-Math.PI / 6 * seconds - Math.PI) + r;
        }

        private double BetaMinusPhase2Y(double seconds)
        {
            var r = 2;
            return r * Math.Sin(-Math.PI / 6 * seconds - Math.PI);
        }

        private double BetaMinusPhase3X(double seconds)
        {
            var t1 = 3;
            return Math.PI / 3 * (seconds - t1);
        }

        private double BetaMinusPhase3Y(double seconds)
        {
            return 2;
        }

        private double NeutronX(double seconds) => 0;

        private double NeutronY(double seconds)
        {
            var v = Math.PI / 3;
            return v * seconds;
        }
    }
}
