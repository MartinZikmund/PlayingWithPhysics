using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Logic.Geometry
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct Radians : IAngleUnit
    {
        /// <summary>
        /// internal name
        /// </summary>
        internal const string Name = "rad";

        /// <inheritdoc />
        public double ConversionFactor => 1.0;

        /// <inheritdoc />
        public string ShortName => Name;
    }
}
