using Physics.HomogenousMovement.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.HomogenousMovement.ViewInteractions
{
    public interface IMainViewInteraction
    {
        HomogenousMovementCanvasController Initialize(DifficultyOption difficulty);
    }
}
