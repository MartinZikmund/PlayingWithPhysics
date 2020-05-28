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
        private IMotionSetup _setup;
        public PhysicsService(IMotionSetup setup)
        {
            _setup = setup;
        }

    

        public float GetDifferenceOfFpFt()
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
            return _setup.Mass * _setup.Gravity * (float)Math.Sin(AngleInRad);
        }
        public float ComputeFt()
        {
            return _setup.DriftCoefficient * _setup.Mass * _setup.Gravity * (float)Math.Cos(AngleInRad);
        }

        public float ComputeEk(float time) => _setup.Mass * (float)Math.Pow(ComputeV(time), 2) / 2;

        public float ComputeX(float time)
        {
            return 0.0f;
        }

        public float ComputeY(float time)
        {
            return 0.0f;
        }

        public float ComputeS(float time)
        {
            switch (_variant)
            {
                case FComputeVariant.MoreThanZero:
                    return Acceleration * (float)Math.Pow(time, 2) / 2;
                case FComputeVariant.LessThanZero:
                    return V0 * time + (Acceleration * (float)Math.Pow(time, 2) / 2);
                default:
                    return time * V0;
            }
        }

        public float ComputeEv(float time) => _setup.Mass * (float)Math.Pow(ComputeV(time), 2) / 2;

        public float ComputeV(float time)
        {
            switch (_variant)
            {
                case FComputeVariant.MoreThanZero:
                    return Acceleration * time;
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

        public float Acceleration => _setup.Gravity * ((float)Math.Sin(AngleInRad) - _setup.DriftCoefficient * (float)Math.Cos(AngleInRad));
        public float Time { get; set; } //To be replaced by drawing controller
        public float MaxT
        {
            get
            {
                switch (_variant)
                {
                    case FComputeVariant.MoreThanZero:
                        return (float)Math.Sqrt(2 * _setup.Length / Acceleration);
                    case FComputeVariant.LessThanZero:
                        if (TotalLength > _setup.Length)
                            return V0 / -Acceleration;
                        else
                            return (float)Math.Pow(V0, 2) + 2 * Acceleration * _setup.Length;
                    default:
                        return _setup.Length / V0;
                }
            }
        }

        public float V0 => 5;
        public float AngleInRad => MathHelpers.DegreesToRadians(_setup.Angle);
        public float TotalLength => (_variant == FComputeVariant.LessThanZero) ? ((float)Math.Pow(V0, 2) / (-2 * Acceleration)) : _setup.Length;
    }

    enum FComputeVariant
    {
        LessThanZero,
        MoreThanZero,
        Equal
    }
}
