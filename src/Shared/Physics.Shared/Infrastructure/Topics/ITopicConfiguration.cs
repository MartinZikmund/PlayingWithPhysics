using System.Threading.Tasks;

namespace Physics.Shared.UI.Infrastructure.Topics
{
    public interface ITopicConfiguration
    {
		int Id { get; }

        bool HasAdvancedDifficulty { get; }

        bool HasStudyMode { get; }

        bool HasGame { get; }

		string GameNameOverride { get; }

        Task GoToDifficultyAsync(DifficultyOption option);

        Task GoToGameAsync();

        Task GoToStudyModeAsync();
    }
}
