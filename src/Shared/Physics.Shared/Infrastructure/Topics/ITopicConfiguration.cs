using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Infrastructure.Topics
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