using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public class TrajectoryData
    {
        public TrajectoryData(params TrajectoryPoint[] points)
        {
            if (points.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(points));
            Points = points ?? throw new ArgumentNullException(nameof(points));
        }

        public IReadOnlyCollection<TrajectoryPoint> Points { get; }

        public float MinX => Points.Min(p => p.X);

        public float MinY => Points.Min(p => p.Y);

        public float MaxX => Points.Max(p => p.X);

        public float MaxY => Points.Max(p => p.Y);
    }
}
