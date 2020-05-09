using Physics.HomogenousMovement.Rendering;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.HomogenousMovement.ViewInteractions
{
    public interface IGameViewInteraction
    {
        GamificationCanvasController Initialize(DifficultyOption difficulty, ISoundPlayer soundPlayer);
    }
}
