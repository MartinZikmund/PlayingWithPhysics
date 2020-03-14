using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Helpers
{
    public static class MathHelpers
    {
        public static float DegreesToRadians(float angleInDegrees) => (float) Math.PI * angleInDegrees / 180.0f;
    }
}
