using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Game
{
    public class GameInfo
    {
        public GameState State { get; set; }

        public int ThrowCount { get; set; }

        public int TotalThrows => 3;

        public float TotalDistance { get; set; }

        public float AverageDistance => ThrowCount > 0 ? TotalDistance / ThrowCount : float.NaN;
    }
}
