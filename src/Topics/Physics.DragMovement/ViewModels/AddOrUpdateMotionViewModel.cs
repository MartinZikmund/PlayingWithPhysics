using Physics.DragMovement.Logic.PhysicsServices;
using Physics.Shared.UI.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.DragMovement.ViewModels
{
    public class AddOrUpdateMotionViewModel
    {
        public AddOrUpdateMotionViewModel(DifficultyOption difficulty, params string[] existingNames)
        {
        }

        public AddOrUpdateMotionViewModel(MotionInfo motionInfo, DifficultyOption difficulty, params string[] existingNames)
        {
        }
        public MotionInfo ResultMotionInfo { get; set; }
    }
}
