using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Infrastructure.Topics
{
    public interface ITopicNavigator
    {
        Task GoToDifficultyAsync(DifficultyOption option);

        Task GoToGameAsync();

        Task OpenStudyTextAsync();
    }
}
