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
                RadiationType.Alpha => ComputeX(MaxT),
                RadiationType.BetaPlus => ComputeX(MaxT),
                RadiationType.BetaMinus => ComputeX(-phase1Length),
                _ => ComputeX(-phase1Length),
            };

        public double MinY(double phase1Length) =>
            _motionSetup.Type switch
            {
                RadiationType.Alpha => ComputeY(-phase1Length),
                RadiationType.BetaPlus => ComputeY(-phase1Length),
                RadiationType.BetaMinus => ComputeY(-phase1Length),
                _ => ComputeY(-phase1Length),
            };

        public double MaxX(double phase1Length) =>
            _motionSetup.Type switch
            {
                RadiationType.Alpha => ComputeX(-phase1Length),
                RadiationType.BetaPlus => ComputeX(-phase1Length),
                RadiationType.BetaMinus => ComputeX(MaxT),
                _ => ComputeX(MaxT),
            };

        public double MaxY(double phase1Length) =>
            _motionSetup.Type switch
            {
                RadiationType.Alpha => ComputeY(MaxT),
                RadiationType.BetaPlus => ComputeY(MaxT),
                RadiationType.BetaMinus => ComputeY(MaxT),
                _ => ComputeY(MaxT),
            };

        public float MaxT => 15;

        public double ComputeX(double seconds) =>
            _motionSetup.Type switch
            {
                RadiationType.Alpha => ComputeParticleCoordinate(seconds, Phase1X, AlphaPhase2X, AlphaPhase3X, ComputeAlphaPhase3StartTime),
                RadiationType.BetaPlus => ComputeParticleCoordinate(seconds, Phase1X, BetaPlusPhase2X, BetaPlusPhase3X, ComputeBetaPhase3StartTime),
                RadiationType.BetaMinus => ComputeParticleCoordinate(seconds, Phase1X, BetaMinusPhase2X, BetaMinusPhase3X, ComputeBetaPhase3StartTime),     
                RadiationType.Gamma => ComputeParticleCoordinate(seconds, GammaX, GammaX, GammaX, () => 0),
                _ => ComputeParticleCoordinate(seconds, NeutronX, NeutronX, NeutronX, ()=> 0),                
            };

        public double ComputeY(double seconds) =>
            _motionSetup.Type switch
            {
                RadiationType.Alpha => ComputeParticleCoordinate(seconds, Phase1Y, AlphaPhase2Y, AlphaPhase3Y, ComputeAlphaPhase3StartTime),
                RadiationType.BetaPlus => ComputeParticleCoordinate(seconds, Phase1Y, BetaPlusPhase2Y, BetaPlusPhase3Y, ComputeBetaPhase3StartTime),
                RadiationType.BetaMinus => ComputeParticleCoordinate(seconds, Phase1Y, BetaMinusPhase2Y, BetaMinusPhase3Y, ComputeBetaPhase3StartTime),
                RadiationType.Gamma => ComputeParticleCoordinate(seconds, GammaY, GammaY, GammaY, () => -2),
                _ => ComputeParticleCoordinate(seconds, NeutronY, NeutronY, NeutronY, () => -2),
            };

        private double ComputeParticleCoordinate(
            double seconds, 
            Func<double, double> phase1,
            Func<double, double> phase2,
            Func<double, double> phase3,
            Func<double> phase3StartTime)
        {           
            if (_motionSetup.Type == RadiationType.Gamma || _motionSetup.Type == RadiationType.Neutron)
            {
                return phase1(seconds);
            }

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

        private double GammaX(double seconds) => 0;

        private double GammaY(double seconds)
        {
            var startY = GammaMinCalc();
            var neutronY = NeutronY(seconds);

            var diff = neutronY - startY;
            return startY + diff * 1.2;
        }

        private double NeutronX(double seconds) => 0;

        private double NeutronY(double seconds)
        {var v = Math.PI / 3;
            return v * seconds;
            
        }

        private double GammaMinCalc()
        {
            var v = Math.PI / 3;
            return v * -2;
        }
    }
}
