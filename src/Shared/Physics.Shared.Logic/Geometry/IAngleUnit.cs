using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Logic.Geometry
{
    public interface IAngleUnit
    {
        /// Identifier differing only in case is not CLS-compliant
        /// <summary>
        /// Gets the value to multiply radians with to get a value in the current unit.
        /// </summary>
        double ConversionFactor { get; }

        /// <summary>
        /// Gets the name of the unit used in ToString
        /// </summary>
        string ShortName { get; }
    }
}
