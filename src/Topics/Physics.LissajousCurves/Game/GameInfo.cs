using System;

namespace Physics.LissajousCurves.Game
{
	public class GameInfo
	{
		public GameInfo(float frequency)
		{
			Frequency = frequency;
		}

		public float Frequency { get; set; }

		public GameState State { get; set; }

		public DateTimeOffset? CountdownStart { get; set; }

		public TimeSpan? TimeAtActionStart { get; set; }

		public bool ClapSoundPlayed { get; set; }

		public bool AreSoundsEnabled { get; set; }

		public float Accuracy { get; set; }

		internal void Reset()
		{
			State = GameState.NotStarted;
			CountdownStart = null;
			TimeAtActionStart = null;
			ClapSoundPlayed = false;
		}
	}
}
