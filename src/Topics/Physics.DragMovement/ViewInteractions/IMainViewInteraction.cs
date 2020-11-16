using Physics.DragMovement.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.DragMovement.ViewInteractions
{
	public interface IMainViewInteraction
    {
        DragMovementCanvasController Initialize(DifficultyOption difficulty);
    }
}
