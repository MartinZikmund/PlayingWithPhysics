using Physics.HomogenousMovement.Rendering;
using Physics.Shared.Infrastructure.Topics;

namespace Physics.HomogenousMovement.ViewInteractions
{
    public interface IGameViewInteraction
    {
        GamificationCanvasController Initialize(DifficultyOption difficulty);
    }
}
