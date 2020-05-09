using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.HomogenousMovement.Models
{
    public class SimulationNavigationModel
    {
        public DifficultyOption Difficulty { get; set; }
        public LaunchInfo LaunchInfo { get; set; }
    }
}
