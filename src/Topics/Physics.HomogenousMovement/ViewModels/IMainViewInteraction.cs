using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.HomogenousMovement.Rendering;
using Physics.Shared.Infrastructure.Topics;
using Physics.Shared.Rendering;

namespace Physics.HomogenousMovement.ViewModels
{
    public interface IMainViewInteraction
    {
        HomogenousMovementCanvasController Initialize(DifficultyOption difficulty);
    }
}
