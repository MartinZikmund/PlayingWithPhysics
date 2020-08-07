using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public struct ValueRow
    {
        public float Time;
        public float Acceleration;
        public float Speed;
        public float Y;
    }

    public class PhysicsService : IPhysicsService
    {
        public List<ValueRow> MotionValues = new List<ValueRow>();
        private const float Delta = 1 / 1000;
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
            FillTable(throwInfo);
            MinX = MotionInfo.Origin.X;
            MaxT = ComputeTMax();
            MaxX = ComputeXMax();
            MaxY = ComputeYMax();
        }
        public void FillTable(MotionInfo throwInfo)
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
            while (lastY >= 0)
            {
                float lastSpeed = MotionValues.Last().Speed;
                float newTime = lastTime + Delta;
                float newAcceleration = throwInfo.G - (float)(0.5 * throwInfo.EnvironmentDensity * throwInfo.Resistance * throwInfo.Area * Math.Pow(lastSpeed, 2)/throwInfo.Mass);
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
                lastTime = newTime;
                lastY = newY;
                MotionValues.Add(row);
            }
        }

        public float ComputeX(float timeMoment)
        {
            if (MotionInfo.Type == MovementType.FreeFall)
            {
                return MotionInfo.Origin.X;
            }
            return 0f;
        }

        public float ComputeY(float timeMoment)
        {
            if (timeMoment < MaxT)
            {
                return MotionValues[(int)(timeMoment / Delta)].Y;
            }
            return 0f;
        }

        public float ComputeV(float timeMoment)
        {
            if (timeMoment < MaxT)
            {
                return MotionValues[(int)(timeMoment / Delta)].Speed;
            }
            return 0f;
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
