using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Logic.Geometry
{
    /// <summary>
    /// Utility class providing static units for angles
    /// </summary>
    public static class AngleUnit
    {
        /// <summary>
        /// A degree or degree of arc typically denoted by °.  It is defined such that a full rotation is 360 degrees.
        /// </summary>
        public static readonly Degrees Degrees = default(Degrees);

        /// <summary>
        /// The SI unit of angular measure is the Radian.
        /// </summary>
        public static readonly Radians Radians = default(Radians);
    }
}
