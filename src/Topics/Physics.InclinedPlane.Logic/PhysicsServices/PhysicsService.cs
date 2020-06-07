using Physics.InclinedPlane.Services;
using Physics.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Logic.PhysicsServices
{
    public class PhysicsService : IPhysicsService
    {
        private FComputeVariant _variant
        {
            get
            {
                float diff = GetDifferenceOfFpFt();
                if (diff > 0)
                {
                    return FComputeVariant.MoreThanZero;
                }
                else if (diff < 0)
                {
                    return FComputeVariant.LessThanZero;
                }
                else
                {
                    return FComputeVariant.Equal;
                }
            }
        }
        public IMotionSetup Setup { get; set; }
        public PhysicsService(IMotionSetup setup)
        {
            Setup = setup;
        }    

        private float GetDifferenceOfFpFt()
        {
            return ComputeFp() - ComputeFt();
        }

        public string GetLabelForFs()
        {
            switch (_variant)
            {
                case FComputeVariant.MoreThanZero:
                    return "Těleso se pohybuje zrychleně po nakloněnné rovině dolů se zrychlením a";
                case FComputeVariant.LessThanZero:
                    return "Tělěso po udělení počáteční rychlosti v0 rovnoměrně zpomalí";
                default:
                    return "Těleso po udělení počáteční rychlosti v0 bude konat rovnoměrný pohyb rychlosti v0.";
            }
        }

        public float ComputeFp()
        {
            return Setup.Mass * Setup.Gravity * (float)Math.Sin(AngleInRad);
        }
        public float ComputeFt()
        {
            return Setup.DriftCoefficient * Setup.Mass * Setup.Gravity * (float)Math.Cos(AngleInRad);
        }

        public float ComputeEk(float time) => Setup.Mass * (float)Math.Pow(ComputeV(time), 2) / 2;

        public float ComputeX(float time)
        {
            return ComputeS(time) * (float)Math.Cos(AngleInRad);
        }

        public float Y0 => Setup.Length * (float)Math.Sin(AngleInRad);
        public float ComputeY(float time)
        {
            return Y0 - ComputeS(time) * (float)Math.Sin(AngleInRad);
        }

        public float ComputeS(float time)
        {
            switch (_variant)
            {
                case FComputeVariant.MoreThanZero:
                case FComputeVariant.LessThanZero:
                    return V0 * time + (Acceleration * (float)Math.Pow(time, 2) / 2);
                default:
                    return time * V0;
            }
        }

        public float ComputeEv(float time) => Setup.Mass * (float)Math.Pow(ComputeV(time), 2) / 2;

        public float ComputeV(float time)
        {
            switch (_variant)
            {
                case FComputeVariant.MoreThanZero:
                case FComputeVariant.LessThanZero:
                    return V0 + Acceleration * time;
                default:
                    return V0;  
            }
        }

        public float ComputeVx(float time)
        {
            return ComputeV(time) * (float)Math.Cos(AngleInRad);
        }

        public float ComputeVy(float time)
        {
            return ComputeV(time) * (float)Math.Sin(AngleInRad);
        }

        public float Acceleration
        {
            get
            {
                if(Setup.FinishLength <= 0)
                {
                    return Setup.Gravity * ((float)Math.Sin(AngleInRad) - Setup.DriftCoefficient * (float)Math.Cos(AngleInRad));
                }
                else
                {
                    return Setup.Gravity * Setup.FinishDriftCoefficient;
                }
            }
        }

        public float Time { get; set; } //To be replaced by drawing controller
        public float MaxT
        {
            get
            {
                switch (_variant)
                {
                    case FComputeVariant.MoreThanZero:
                        return (-V0 + (float)Math.Sqrt((float)Math.Pow(V0, 2) + 2 * Acceleration * Setup.Length)) / Acceleration;
                    case FComputeVariant.LessThanZero:
                        if (MaxS < Setup.Length)
                            return V0 / -Acceleration;
                        else
                            return (-V0 + (float)Math.Sqrt((float)Math.Pow(V0, 2) + 2 * Acceleration * Setup.Length)) / Acceleration;
                    default:
                        return Setup.Length / V0;
                }
            }
        }

        public float V0 => 5;
        public float AngleInRad => MathHelpers.DegreesToRadians(Setup.Angle);
        public float MaxS
        {
            get
            {
                if (Setup.FinishLength > 0)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    return Math.Min(V0*V0 / (-2 * Acceleration), Setup.Length);
                }                
            }
        }
    }

    enum FComputeVariant
    {
        LessThanZero,
        MoreThanZero,
        Equal
    }
}
