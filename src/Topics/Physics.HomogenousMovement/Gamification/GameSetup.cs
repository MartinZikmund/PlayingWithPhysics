using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousMovement.Gamification
{
    public class GameSetup
    {
        public GameSetup(int castleDistance, int wallDistance, int[] treeDistances)
        {
            CastleDistance = castleDistance;
            WallDistance = wallDistance;
            TreeDistances = treeDistances;
        }

        public int CastleDistance { get; }

        public int WallDistance { get; }

        public int[] TreeDistances { get; }
    }
}
