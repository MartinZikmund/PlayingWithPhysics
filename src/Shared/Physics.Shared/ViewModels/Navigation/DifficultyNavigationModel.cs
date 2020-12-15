using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.Shared.UI.ViewModels.Navigation
{
	public class DifficultyNavigationModel
	{
		public DifficultyNavigationModel()
		{
		}

		public DifficultyNavigationModel(DifficultyOption difficulty)
		{
			Difficulty = difficulty;
		}

		public DifficultyOption Difficulty { get; set; }
	}
}
