using Physics.DragMovement.Rendering;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.DragMovement.ViewInteractions
{
    public interface IGameViewInteraction
    {
        GamificationCanvasController Initialize(DifficultyOption difficulty, ISoundPlayer soundPlayer);
    }
}
