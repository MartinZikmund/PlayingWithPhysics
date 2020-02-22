using Physics.HomogenousMovement.PhysicsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousMovement.Models
{
    public class AvailableMotionNames
    {
        private Dictionary<MovementType, string> _availableMotionNames =
            new Dictionary<MovementType, string>();

        public string GetForType(MovementType type)
        {
            return _availableMotionNames[type];
        }
    }
}
