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
        private float? _maxT = null;
        private float? _maxS = null;
        private float? _maxX = null;
        private bool? _willReachInclinedEnd = null;
        private float? _inclinedMaxT = null;
        private float? _inclinedAcceleration = null;
        private float? _horizontalStartTime = null;
        private float? _horizontalStartVelocity = null;
        private float? _horizontalStartX = null;
        private float? _horizontalAcceleration = null;
        private float? _horizontalMaxX = null;
        private float? _ft = null;
        private float? _fp = null;

        public PhysicsService(IInclinedPlaneMotionSetup setup)
        {
            Setup = setup;
        }

        public IInclinedPlaneMotionSetup Setup { get; set; }

        public float AngleInRad => MathHelpers.DegreesToRadians(Setup.InclinedAngle);

        #region Full motion calculations        

        public float CalculateMaxX()
        {
            if (_maxX == null)
            {
                var maxT = CalculateMaxT();
                _maxX = CalculateX(maxT);
            }
            return _maxX.Value;
        }

        public float CalculateTotalWidth()
        {
            return CalculateInclinedWidth() + Setup.HorizontalLength;
        }

        public float CalculateS(float time)
        {
            time = Math.Min(time, CalculateMaxT());

            var horizontalStartTime = CalculateHorizontalStartTime();
            if (time <= horizontalStartTime)
            {
                return CalculateInclinedS(time);
            }
            else
            {
                return Setup.InclinedLength + CalculateHorizontalX(CalculateHorizontalTime(time));
            }
        }

        public float CalculateX(float time)
        {
            time = Math.Min(time, CalculateMaxT());

            var horizontalStartTime = CalculateHorizontalStartTime();
            if (time <= horizontalStartTime)
            {
                return CalculateInclinedX(time);
            }
            else
            {
                return CalculateHorizontalStartX() + CalculateHorizontalX(CalculateHorizontalTime(time));
            }
        }

        public float CalculateRemainingInclinedLength(float time)
        {
            time = Math.Min(time, CalculateMaxT());
            
            var x = CalculateInclinedX(time);
            var horizontalStartX = CalculateHorizontalStartX();
            var xDiff = horizontalStartX - x;
            return xDiff / (float)Math.Cos(MathHelpers.DegreesToRadians(Setup.InclinedAngle));
        }

        public float CalculateRemainingInclinedX(float time)
        {
            time = Math.Min(time, CalculateMaxT());

            var x = CalculateInclinedX(time);
            var horizontalStartX = CalculateHorizontalStartX();
            return horizontalStartX - x;
        }

        public float CalculateY(float time)
        {
            time = Math.Min(time, CalculateMaxT());

            var horizontalStartTime = CalculateHorizontalStartTime();
            if (time <= horizontalStartTime)
            {
                return CalculateInclinedY(time);
            }
            else
            {
                return 0;
            }
        }

        public float CalculateV(float time)
        {
            time = Math.Min(time, CalculateMaxT());

            var horizontalStartTime = CalculateHorizontalStartTime();
            if (time <= horizontalStartTime)
            {
                return CalculateInclinedV(time);
            }
            else
            {
                return CalculateHorizontalVx(CalculateHorizontalTime(time));
            }
        }

        public float CalculateMaxT()
        {
            if (_maxT == null)
            {
                // inclined plane
                var inclinedPlaneMaxT = CalculateInclinedMaxT();
                var inclinedPlaneEndReached = WillReachInclinedEnd();

                if (!inclinedPlaneEndReached || !Setup.HasHorizontal)
                {
                    return inclinedPlaneMaxT;
                }

                // get horizontal end time
                var horizontalEndTime = CalculateHorizontalEndTime();

                return inclinedPlaneMaxT + horizontalEndTime;
            }
            return _maxT.Value;
        }


        public float CalculateVx(float time)
        {
            if (time > CalculateMaxT())
            {
                return 0;
            }
            var horizontalStartTime = CalculateHorizontalStartTime();
            if (time <= horizontalStartTime)
            {
                return CalculateInclinedVx(time);
            }
            return CalculateHorizontalVx(CalculateHorizontalTime(time));
        }

        public float CalculateVy(float time)
        {
            if (time > CalculateMaxT() || time > CalculateHorizontalStartTime())
            {
                return 0;
            }
            return CalculateInclinedVy(time);
        }

        public float CalculateMaxS()
        {
            if (_maxS == null)
            {
                throw new NotImplementedException();
                //_maxS = Math.Min(Setup.V0 * Setup.V0 / (-2 * CalculateInclinedAcceleration()), Setup.InclinedLength);
            }
            return _maxS.Value;
        }

        #endregion

        #region Inclined plane calculations

        public float CalculateInclinedWidth()
        {
            return Setup.InclinedLength * (float)Math.Cos(AngleInRad);
        }

        public float CalculateInclinedMaxT()
        {
            if (_inclinedMaxT == null)
            {
                var D = Setup.V0 * Setup.V0 + 2 * CalculateInclinedAcceleration() * Setup.InclinedLength;
                switch (CalculateInclinedPlaneMovementType())
                {
                    case InclinedPlaneMovementType.Accelerating:
                    case InclinedPlaneMovementType.Decelerating:
                        if (D < 0)
                        {
                            // will stop before end
                            _inclinedMaxT = Setup.V0 / -CalculateInclinedAcceleration();
                        }
                        else
                        {
                            _inclinedMaxT = (-Setup.V0 + (float)Math.Sqrt(D)) / CalculateInclinedAcceleration();
                        }
                        break;
                    default:
                        _inclinedMaxT = Setup.InclinedLength / Setup.V0;
                        break;
                }
            }
            return _inclinedMaxT.Value;
        }

        public bool WillReachInclinedEnd()
        {
            if (_willReachInclinedEnd == null)
            {
                var D = Setup.V0 * Setup.V0 + 2 * CalculateInclinedAcceleration() * Setup.InclinedLength;
                switch (CalculateInclinedPlaneMovementType())
                {
                    case InclinedPlaneMovementType.Accelerating:
                    case InclinedPlaneMovementType.Decelerating:
                        if (D < 0)
                        {
                            // will stop before end                           
                            _willReachInclinedEnd = false;
                        }
                        else
                        {
                            _willReachInclinedEnd = true;
                        }
                        break;
                    default:
                        _willReachInclinedEnd = true;
                        break;
                }
            }
            return _willReachInclinedEnd.Value;
        }

        public float CalculateInclinedAcceleration()
        {
            if (_inclinedAcceleration == null)
            {
                _inclinedAcceleration = Setup.Gravity * ((float)Math.Sin(AngleInRad) - Setup.InclinedDirftCoefficient * (float)Math.Cos(AngleInRad));
            }
            return _inclinedAcceleration.Value;
        }

        public float CalculateInclinedV(float time)
        {
            switch (CalculateInclinedPlaneMovementType())
            {
                case InclinedPlaneMovementType.Accelerating:
                case InclinedPlaneMovementType.Decelerating:
                    return Setup.V0 + CalculateInclinedAcceleration() * time;
                default:
                    return Setup.V0;
            }
        }

        public float CalculateInclinedX(float time)
        {
            return CalculateInclinedS(time) * (float)Math.Cos(AngleInRad);
        }

        public float CalculateInclinedY(float time)
        {
            return CalculateStartY() - CalculateS(time) * (float)Math.Sin(AngleInRad);
        }

        public float CalculateStartY() => Setup.InclinedLength * (float)Math.Sin(AngleInRad);

        public float CalculateInclinedVx(float time)
        {
            //TODO
            return CalculateInclinedV(time) * (float)Math.Cos(AngleInRad);
        }

        public float CalculateInclinedVy(float time)
        {
            //TODO
            return CalculateV(time) * (float)Math.Sin(AngleInRad);
        }

        public float CalculateInclinedS(float time)
        {
            switch (CalculateInclinedPlaneMovementType())
            {
                case InclinedPlaneMovementType.Accelerating:
                case InclinedPlaneMovementType.Decelerating:
                    return Setup.V0 * time + (CalculateInclinedAcceleration() * (float)Math.Pow(time, 2) / 2);
                default:
                    return time * Setup.V0;
            }
        }

        public float CalculateEk(float time) => Setup.Mass * (float)Math.Pow(CalculateV(time), 2) / 2;
        public float CalculateEp(float time) => Setup.Mass * Setup.Gravity * CalculateY(time);
        public float CalculateEm(float time) => CalculateEk(time) + CalculateEp(time);
        public float CalculateE() => CalculateEm(0f);
        public float CalculateU(float time) => CalculateE() - CalculateEm(time);

        #endregion

        #region Horizontal calculations

        public float CalculateHorizontalTime(float time)
        {
            return time - CalculateHorizontalStartTime();
        }

        public float CalculateHorizontalStartTime()
        {
            if (_horizontalStartTime == null)
            {
                if (WillReachInclinedEnd())
                {
                    _horizontalStartTime = CalculateInclinedMaxT();
                }
                else
                {
                    //"invalid" time
                    _horizontalStartTime = float.MaxValue;
                }
            }
            return _horizontalStartTime.Value;
        }

        public float CalculateHorizontalStartVelocity()
        {
            if (_horizontalStartVelocity == null)
            {
                if (WillReachInclinedEnd())
                {
                    _horizontalStartVelocity = CalculateInclinedV(CalculateInclinedMaxT());
                }
                else
                {
                    _horizontalStartVelocity = 0;
                }
            }
            return _horizontalStartVelocity.Value;
        }

        public float CalculateHorizontalVx(float horizontalSeconds)
        {
            return CalculateHorizontalStartVelocity() + CalculateHorizontalAcceleration() * horizontalSeconds;
        }

        public float CalculateHorizontalStartX()
        {
            if (_horizontalStartX == null)
            {
                if (WillReachInclinedEnd())
                {
                    _horizontalStartX = CalculateInclinedX(CalculateInclinedMaxT());
                }
                else
                {
                    _horizontalStartX = 0;
                }
            }
            return _horizontalStartX.Value;
        }

        /// <summary>
        /// Acceleration on horizontal plane.
        /// </summary>
        /// <returns>Acceleration.</returns>
        public float CalculateHorizontalAcceleration()
        {
            if (_horizontalAcceleration == null)
            {
                _horizontalAcceleration = -Setup.Gravity * Setup.HorizontalDriftCoefficient;
            }
            return _horizontalAcceleration.Value;
        }

        public float CalculateHorizontalX(float horizontalSeconds)
        {
            var v2 = CalculateHorizontalStartVelocity();
            var x = v2 * horizontalSeconds + CalculateHorizontalAcceleration() * horizontalSeconds * horizontalSeconds / 2;
            return Math.Min(CalculateHorizontalMaxX(), x);
        }

        public float CalculateHorizontalMaxX()
        {
            if (_horizontalMaxX == null)
            {
                var stopX = CalculateHorizontalStopX();
                _horizontalMaxX = Math.Max(stopX, Setup.HorizontalLength);
            }
            return _horizontalMaxX.Value;
        }

        public float CalculateHorizontalStopX()
        {
            var horizontalStartVelocity = CalculateHorizontalStartVelocity();
            return horizontalStartVelocity * horizontalStartVelocity / (-2 * CalculateHorizontalAcceleration());
        }

        public float CalculateHorizontalEndTime()
        {
            var v2 = CalculateHorizontalStartVelocity();
            var D = v2 * v2 + 2 * CalculateHorizontalAcceleration() * Setup.HorizontalLength;
            if (D >= 0)
            {
                return (-v2 + (float)Math.Sqrt(D)) / CalculateHorizontalAcceleration();
            }
            else
            {
                return v2 / (-CalculateHorizontalAcceleration());
            }
        }

        #endregion

        private InclinedPlaneMovementType CalculateInclinedPlaneMovementType()
        {
            float diff = CalculateInclinedFp() - CalculateInclinedFt();
            if (diff > 0)
            {
                return InclinedPlaneMovementType.Accelerating;
            }
            else if (diff < 0)
            {
                return InclinedPlaneMovementType.Decelerating;
            }
            else
            {
                return InclinedPlaneMovementType.Static;
            }
        }

        private string GetLabelForFs()
        {
            switch (CalculateInclinedPlaneMovementType())
            {
                case InclinedPlaneMovementType.Accelerating:
                    return "Těleso se pohybuje zrychleně po nakloněnné rovině dolů se zrychlením a";
                case InclinedPlaneMovementType.Decelerating:
                    return "Tělěso po udělení počáteční rychlosti v0 rovnoměrně zpomalí";
                default:
                    return "Těleso po udělení počáteční rychlosti v0 bude konat rovnoměrný pohyb rychlosti v0.";
            }
        }

        public float CalculateFp(float time)
        {
            if (time > CalculateMaxT())
            {
                return 0;
            }
            var horizontalStartTime = CalculateHorizontalStartTime();
            if (time <= horizontalStartTime)
            {
                return CalculateInclinedFp();
            }
            return CalculateHorizontalFp();
        }

        public float CalculateFt(float time)
        {
            if (time > CalculateMaxT())
            {
                return 0;
            }
            var horizontalStartTime = CalculateHorizontalStartTime();
            if (time <= horizontalStartTime)
            {
                return CalculateInclinedFt();
            }
            return CalculateHorizontalFt();
        }

        public float CalculateHorizontalFp() => 0;


        private float CalculateInclinedFp()
        {
            if (_fp == null)
            {
                _fp = Setup.Mass * Setup.Gravity * (float)Math.Sin(AngleInRad);
            }
            return _fp.Value;
        }

        private float CalculateInclinedFt()
        {
            if (_ft == null)
            {
                _ft = Setup.InclinedDirftCoefficient * Setup.Mass * Setup.Gravity * (float)Math.Cos(AngleInRad);
            }
            return _ft.Value;
        }

        private float CalculateHorizontalFt()
        {
            return Setup.HorizontalDriftCoefficient * Setup.Mass * Setup.Gravity;
        }
    }
}
