using Physics.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public class ValueRow
    {
        public float Time;
        public float Acceleration;
        public float Speed;
        public float X;
        public float Y;
        public float Vx;
        public float Vy;
        public float Ep;
        public float Ek;
    }

    public class PhysicsService : IPhysicsService
    {
        public List<ValueRow> MotionValues = new List<ValueRow>();
        private const float Delta = 1 / 1000.0f;
        private float LastVelocty = 0f;

        private const int MinTrajectoryJumps = 50;
        private const int MaxTrajectoryJumps = 200;

        public MotionInfo MotionInfo { get; }
        public float MaxT { get; }
        public float MaxX { get; }
        public float MaxV { get; }
        public float MaxY { get; }
        public float MinX { get; }

        public PhysicsService(MotionInfo throwInfo)
        {
            MotionInfo = throwInfo;
            if (throwInfo.Type == MovementType.FreeFall)
            {
                FillFreeFallInfo(throwInfo);
            }
            else
            {
                FillProjectileMotionInfo(throwInfo);
            }
            MinX = MotionInfo.Origin.X;
            MaxT = ComputeTMax();
            MaxX = ComputeXMax();
            MaxY = ComputeYMax();
        }
        public void FillFreeFallInfo(MotionInfo throwInfo)
        {
            //Fill t = 0 row
            ValueRow rowZero = new ValueRow
            {
                Time = 0f,
                Acceleration = throwInfo.G,
                Speed = 0f,
                Y = throwInfo.Origin.Y
            };
            MotionValues.Add(rowZero);

            //Fill rest of table
            //lastY is Origin.Y at the beginning, goes down to 0 with FreeFall
            float lastY = MotionValues.Last().Y;
            float lastTime = MotionValues.Last().Time;
            float lastSpeed = MotionValues.Last().Speed;
            while (lastY > 0)
            {
                float newTime = lastTime + Delta;
                float newAcceleration = throwInfo.G - (float)(0.5 * throwInfo.EnvironmentDensity * throwInfo.Resistance * throwInfo.Area * Math.Pow(lastSpeed, 2) / throwInfo.Mass);
                float newSpeed = lastSpeed + newAcceleration * Delta;
                float newY = lastY - newSpeed * Delta;

                //Fill row of values
                ValueRow row = new ValueRow
                {
                    Time = newTime,
                    Acceleration = newAcceleration,
                    Speed = newSpeed,
                    Y = newY
                };
                MotionValues.Add(row);

                lastTime = newTime;
                lastY = newY;
                lastSpeed = newSpeed;
            }
        }

        public void FillProjectileMotionInfo(MotionInfo throwInfo)
        {
            //Fill t = 0 row
            ValueRow rowZero = new ValueRow
            {
                X = throwInfo.Origin.X,
                Y = throwInfo.Origin.Y,
                Vx = throwInfo.OriginSpeed * (float)Math.Cos(MathHelpers.DegreesToRadians(throwInfo.ElevationAngle)),
                Vy = throwInfo.OriginSpeed * (float)Math.Sin(MathHelpers.DegreesToRadians(throwInfo.ElevationAngle)),
                Time = 0f,
                Acceleration = throwInfo.G,
                Speed = 0f,
                Ep = throwInfo.Mass * throwInfo.G * throwInfo.Origin.Y,
                Ek = 0.5f * throwInfo.Mass * (float)Math.Pow(throwInfo.OriginSpeed, 2)
            };
            MotionValues.Add(rowZero);

            //Fill rest of table
            //lastY is Origin.Y at the beginning, goes down to 0 with FreeFall
            var lastValue = MotionValues.Last();

            float lastX = lastValue.X;
            float lastY = lastValue.Y;
            
            float lastVx = lastValue.Vx;
            float lastVy = lastValue.Vy;
            
            float lastTime = lastValue.Time;
            float lastSpeed = lastValue.Speed;
            while (lastY >   0)
            {
                float newVx = lastVx - ((float)(0.5 * throwInfo.EnvironmentDensity * throwInfo.Resistance * throwInfo.Area * Math.Pow(lastVx, 2) / throwInfo.Mass)) * Delta;
                float newVy;
                if (lastVy > 0)
                {
                    newVy = lastVy - (throwInfo.G + (float)(0.5 * throwInfo.EnvironmentDensity * throwInfo.Resistance * throwInfo.Area * Math.Pow(lastVy, 2) / throwInfo.Mass)) * Delta;
                }
                else if (lastVy < 0)
                {
                    newVy = lastVy - (throwInfo.G - (float)(0.5 * throwInfo.EnvironmentDensity * throwInfo.Resistance * throwInfo.Area * Math.Pow(lastVy, 2) / throwInfo.Mass)) * Delta;
                }
                else
                {
                    newVy = lastVy;
                }

                float newX = lastX + newVx * Delta;
                float newY = lastY + newVy * Delta;
                if (newY < 0)
                {
                    newY = 0;
                }

                float newTime = lastTime + Delta;
                float newAcceleration = throwInfo.G - (float)(0.5 * throwInfo.EnvironmentDensity * throwInfo.Resistance * throwInfo.Area * Math.Pow(lastSpeed, 2) / throwInfo.Mass);
                float newSpeed = lastSpeed + newAcceleration * Delta;

                //Fill row of values
                ValueRow row = new ValueRow
                {
                    X = newX,
                    Y = newY,
                    Vx = newVx,
                    Vy = newVy,
                    Time = newTime,
                    Acceleration = newAcceleration,
                    Speed = newSpeed,
                };
                MotionValues.Add(row);

                lastX = newX;
                lastY = newY;
                lastVx = newVx;
                lastVy = newVy;
                lastTime = newTime;
                lastSpeed = newSpeed;
            }
        }

        public float ComputeX(float timeMoment)
        {
            if (MotionInfo.Type == MovementType.FreeFall)
            {
                return MotionInfo.Origin.X;
            }

            if (timeMoment <= MaxT)
            {
                var foundRow = FindRow(timeMoment);
                if (foundRow != null)
                {
                    return foundRow.X;
                }
            }
            return MotionValues.Last().X;
        }

        public float ComputeY(float timeMoment)
        {
            if (timeMoment <= MaxT)
            {
                var foundRow = FindRow(timeMoment);
                if (foundRow != null)
                {
                    return foundRow.Y;
                }
            }
            return MotionValues.Last().Y;
        }

        public float ComputeV(float timeMoment)
        {
            var foundRow = FindRow(timeMoment);
            if (foundRow != null)
            {
                return foundRow.Speed;
            }
            return 0f;
        }

        public float ComputeAcceleration(float timeMoment)
        {
            var foundRow = FindRow(timeMoment);
            if (foundRow != null)
            {
                return foundRow.Acceleration;
            }
            return 0f;
        }

        public float Resistance => MotionInfo.Resistance;

        public ValueRow FindRow(float timeMoment)
        {
            if (timeMoment < MaxT)
            {
                for (int rowIndex = 0; rowIndex < MotionValues.Count; rowIndex++)
                {
                    var row = MotionValues[rowIndex];
                    if (row.Time > timeMoment)
                    {
                        if (rowIndex == 0)
                        {
                            return row;
                        }

                        return MotionValues[rowIndex - 1];
                    }
                }
                return MotionValues.Last();
            }
            return null;
        }
        public float AngleInRad => (float)Math.PI * MotionInfo.ElevationAngle / 180.0f;

        private float ComputeTMax()
        {
            return MotionValues.Last().Time;
        }

        private float ComputeXMax()
        {
            return ComputeX(ComputeTMax());
        }

        private float ComputeYMax()
        {
            return MotionValues.Last().Y;
        }

        public TrajectoryData CreateTrajectoryData()
        {
            if (Math.Abs(MaxT) < 0.00001)
            {
                return new TrajectoryData(new TrajectoryPoint(TimeSpan.Zero, ComputeX(0), ComputeY(0)));
            }
            var frameTime = 1 / 60f;
            var jumpCount = (int)Math.Ceiling(MaxT / frameTime);
            if (jumpCount > MaxTrajectoryJumps)
            {
                jumpCount = MaxTrajectoryJumps;
            }

            if (jumpCount < MinTrajectoryJumps)
            {
                jumpCount = MinTrajectoryJumps;
            }
            var jumpSize = MaxT / jumpCount;
            var currentTime = 0f;
            var points = new List<TrajectoryPoint>();
            while (currentTime <= MaxT)
            {
                points.Add(new TrajectoryPoint(TimeSpan.FromSeconds(currentTime), ComputeX(currentTime), ComputeY(currentTime)));
                currentTime += jumpSize;
            }
            points.Add(new TrajectoryPoint(TimeSpan.FromSeconds(MaxT), ComputeX(MaxT), ComputeY(MaxT)));
            return new TrajectoryData(points.ToArray());
        }
    }
}
