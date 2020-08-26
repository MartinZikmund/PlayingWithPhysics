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

        public int ThrowCount { get; set; } = 1;        

        public int TotalThrows => 3;

        public float? TotalDistance { get; private set; }

        public int FinishedThrows { get; private set; }

        public void AddFinishedThrow(float distance)
        {
            FinishedThrows++;
            TotalDistance = (TotalDistance ?? 0) + distance;
        }

        public float AverageDistance => TotalDistance != null ? TotalDistance.Value / FinishedThrows : float.NaN;
    }
}
