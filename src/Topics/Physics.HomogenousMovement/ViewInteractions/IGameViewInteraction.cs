using Physics.HomogenousMovement.Rendering;
using Physics.Shared.Infrastructure.Topics;
using Physics.Shared.Services.Sounds;

namespace Physics.HomogenousMovement.ViewInteractions
{
    public interface IGameViewInteraction
    {
        GamificationCanvasController Initialize(DifficultyOption difficulty, ISoundPlayer soundPlayer);
    }
}
