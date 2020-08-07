using Physics.DragMovement.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.DragMovement.ViewInteractions
{
    public interface IMainViewInteraction
    {
        DragMovementCanvasController Initialize(DifficultyOption difficulty);
    }
}
