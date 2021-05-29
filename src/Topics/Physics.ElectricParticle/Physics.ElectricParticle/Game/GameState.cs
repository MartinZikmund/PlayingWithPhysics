using MvvmCross.ViewModels;

namespace Physics.ElectricParticle.Game
{
	public class GameState : MvxNotifyPropertyChanged
    {
		private const int LevelCount = 11;

        public int Level { get; set; }

		public bool UseGravity { get; set; }

		public bool IsPenDown { get; set; }

		public void GoToNextLevel() => Level = (Level + 1) % LevelCount;
	}
}
