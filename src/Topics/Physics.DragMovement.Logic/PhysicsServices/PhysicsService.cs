using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public class PhysicsService : IPhysicsService
    {
        private const int MinTrajectoryJumps = 50;
        private const int MaxTrajectoryJumps = 200;
        public MotionInfo MotionInfo { get; }
        public float MaxT { get; }

        public float MaxX { get; }

        public float MaxV { get; }

        public float MaxY { get; }

        public float MinX { get; }

        public float ComputeX(float timeMoment)
        {
            if (timeMoment > MaxT) return MaxX;

            //x = x0 + v0 * t
            float x = MotionInfo.Origin.X + MotionInfo.V0 * timeMoment * (float)Math.Cos(AngleInRad);
            return x;
        }

        public float ComputeY(float timeMoment)
        {
            if (timeMoment > MaxT) return 0f;
            //y = y0 + v0 * t * sin(a)-0.5 * g * (t^2)
            float y = MotionInfo.Origin.Y + MotionInfo.V0 * timeMoment * (float)Math.Sin(AngleInRad) - 0.5f * MotionInfo.G * (float)Math.Pow(timeMoment, 2);
            return y;
        }

        public float ComputeV(float timeMoment)
        {
            if (timeMoment > MaxT) return 0f;

            float vSquared = (float)Math.Pow(ComputeVX(timeMoment), 2) + (float)Math.Pow(ComputeVY(timeMoment), 2);
            float v = (float)Math.Sqrt(vSquared);
            return v;
        }

        public float ComputeVX(float timeMoment)
        {
            if (timeMoment > MaxT) return 0f;

            float vx = MotionInfo.V0 * (float)Math.Cos(AngleInRad);
            return vx;
        }

        public float ComputeVY(float timeMoment)
        {
            if (timeMoment > MaxT) return 0f;

            float vy = MotionInfo.V0 * (float)Math.Sin(AngleInRad) - MotionInfo.G * timeMoment;
            return vy;
        }

        public float ComputeEP(float timeMoment)
        {
            float ep = MotionInfo.Mass * MotionInfo.G * ComputeY(timeMoment);
            return ep;
        }

        public float ComputeEK(float timeMoment)
        {
            float ek = 0.5f * MotionInfo.Mass * (float)Math.Pow(ComputeV(timeMoment), 2);
            return ek;
        }

        public float ComputeEPEK(float timeMoment)
        {
            float epek = ComputeEP(timeMoment) + ComputeEK(timeMoment);
            return epek;
        }

        public PhysicsService(MotionInfo throwInfo)
        {
            MotionInfo = throwInfo;
            MinX = MotionInfo.Origin.X;
            MaxT = ComputeTMax();
            MaxX = ComputeXMax();
            MaxY = ComputeYMax();
        }

        public float AngleInRad => (float)Math.PI * MotionInfo.Angle / 180.0f;

        private float ComputeTMax()
        {
            var vy = MotionInfo.V0 * (float)Math.Sin(AngleInRad);
            return (vy + (float)Math.Sqrt((float)Math.Pow(vy, 2) + 2 * MotionInfo.G * MotionInfo.Origin.Y)) / MotionInfo.G;

        }

        private float ComputeXMax()
        {
            return ComputeX(ComputeTMax());
        }

        private float ComputeYMax()
        {
            var vy = MotionInfo.V0 * (float)Math.Sin(AngleInRad);
            return MotionInfo.Origin.Y + (float)Math.Pow(vy, 2) / (2 * MotionInfo.G);
        }

        //public TrajectoryData CreateTrajectoryData()
        //{
        //    if (Math.Abs(MaxT) < 0.00001)
        //    {
        //        return new TrajectoryData(new TrajectoryPoint(TimeSpan.Zero, ComputeX(0), ComputeY(0)));
        //    }
        //    var frameTime = 1 / 60f;
        //    var jumpCount = (int)Math.Ceiling(MaxT / frameTime);
        //    if (jumpCount > MaxTrajectoryJumps)
        //    {
        //        jumpCount = MaxTrajectoryJumps;
        //    }

        //    if (jumpCount < MinTrajectoryJumps)
        //    {
        //        jumpCount = MinTrajectoryJumps;
        //    }
        //    var jumpSize = MaxT / jumpCount;
        //    var currentTime = 0f;
        //    var points = new List<TrajectoryPoint>();
        //    while (currentTime <= MaxT)
        //    {
        //        points.Add(new TrajectoryPoint(TimeSpan.FromSeconds(currentTime), ComputeX(currentTime), ComputeY(currentTime)));
        //        currentTime += jumpSize;
        //    }
        //    points.Add(new TrajectoryPoint(TimeSpan.FromSeconds(MaxT), ComputeX(MaxT), ComputeY(MaxT)));
        //    return new TrajectoryData(points.ToArray());
        //}
    }
}
