using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    /// <summary>
    /// Point of a trajectory.
    /// </summary>
    public class TrajectoryPoint
    {
        public TrajectoryPoint(TimeSpan time, float x, float y)
        {
            Time = time;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Time at point.
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// X in meters.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y in meters.
        /// </summary>
        public float Y { get; set; }
    }
}
