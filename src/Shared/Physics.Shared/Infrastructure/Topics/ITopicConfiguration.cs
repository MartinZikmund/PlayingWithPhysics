using System.Threading.Tasks;

namespace Physics.Shared.UI.Infrastructure.Topics
{
    public interface ITopicConfiguration
    {
        bool HasAdvancedDifficulty { get; }

        bool HasStudyMode { get; }

        bool HasGame { get; }

        Task GoToDifficultyAsync(DifficultyOption option);

        Task GoToGameAsync();

        Task GoToStudyModeAsync();
    }
}