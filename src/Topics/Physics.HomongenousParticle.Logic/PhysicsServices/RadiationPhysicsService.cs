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

        public double MinX(double phase1Length) =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeX(MaxT),
                RadiationType.BetaPlus => ComputeX(MaxT),
                RadiationType.BetaMinus => ComputeX(-phase1Length),
                RadiationType.Neutron => ComputeX(-phase1Length),
                _ => throw new InvalidOperationException()
            };

        public double MinY(double phase1Length) =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeY(-phase1Length),
                RadiationType.BetaPlus => ComputeY(-phase1Length),
                RadiationType.BetaMinus => ComputeY(-phase1Length),
                RadiationType.Neutron => ComputeY(-phase1Length),
                _ => throw new InvalidOperationException()
            };

        public double MaxX(double phase1Length) =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeX(-phase1Length),
                RadiationType.BetaPlus => ComputeX(-phase1Length),
                RadiationType.BetaMinus => ComputeX(MaxT),
                RadiationType.Neutron => ComputeX(MaxT),
                _ => throw new InvalidOperationException()
            };

        public double MaxY(double phase1Length) =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeY(MaxT),
                RadiationType.BetaPlus => ComputeY(MaxT),
                RadiationType.BetaMinus => ComputeY(MaxT),
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
                RadiationType.Neutron => ComputeParticleCoordinate(seconds, NeutronX, NeutronX, NeutronX, ()=> 0),
                _ => throw new InvalidOperationException()
            };

        public double ComputeY(double seconds) =>
            _motionSetup.Type switch
            {
                RadiationType.Alfa => ComputeParticleCoordinate(seconds, Phase1Y, AlphaPhase2Y, AlphaPhase3Y, ComputeAlphaPhase3StartTime),
                RadiationType.BetaPlus => ComputeParticleCoordinate(seconds, Phase1Y, BetaPlusPhase2Y, BetaPlusPhase3Y, ComputeBetaPhase3StartTime),
                RadiationType.BetaMinus => ComputeParticleCoordinate(seconds, Phase1Y, BetaMinusPhase2Y, BetaMinusPhase3Y, ComputeBetaPhase3StartTime),
                RadiationType.Neutron => ComputeParticleCoordinate(seconds, NeutronY, NeutronY, NeutronY, () => 0),
                _ => throw new InvalidOperationException()
            };

        private double ComputeParticleCoordinate(
            double seconds, 
            Func<double, double> phase1,
            Func<double, double> phase2,
            Func<double, double> phase3,
            Func<double> phase3StartTime)
        {            
            if (seconds < 0)
            {
                return phase1(seconds);
            }
            var phase3Start = phase3StartTime();
            if (seconds <= phase3Start)
            {
                return phase2(seconds);
            }
            var phase3StartCoordinate = phase2(phase3Start);
            var phase3Time = seconds - phase3Start;
            return phase3StartCoordinate + phase3(phase3Time);
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

        private double AlphaPhase3X(double phase3Time)
        {
            return -((Math.Sqrt(5) * Math.PI) / 9) * phase3Time;
        }

        private double AlphaPhase3Y(double phase3Time)
        {
            return (2 * Math.PI) / 9 * phase3Time;
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

        private double BetaPlusPhase3X(double phase3Time)
        {
            return -Math.PI / 3 * phase3Time;
        }

        private double BetaPlusPhase3Y(double seconds) => 0;

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

        private double BetaMinusPhase3X(double phase3Time)
        {
            return Math.PI / 3 * phase3Time;
        }

        private double BetaMinusPhase3Y(double seconds)
        {
            return 0;
        }

        private double NeutronX(double seconds) => 0;

        private double NeutronY(double seconds)
        {
            var v = Math.PI / 3;
            return v * seconds;
        }
    }
}
