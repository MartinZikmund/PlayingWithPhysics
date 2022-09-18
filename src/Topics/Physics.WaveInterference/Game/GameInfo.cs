using GalaSoft.MvvmLight;
using MvvmCross.ViewModels;

namespace Physics.WaveInterference.Game;

public class GameInfo : MvxNotifyPropertyChanged
{
	private static readonly GameTask[] _gameTasks = GameTasks.All;

	private int _currentTaskIndex = 0;

	public GameInfo()
	{
		GamePhysicsService = new(Wave1, Wave2);
		CurrentTask = _gameTasks[0];
	}

	public GameWaveInfo Wave1 { get; } = new();

	public GameWaveInfo Wave2 { get; } = new();

	public GamePhysicsService GamePhysicsService { get; }

	public GameTask CurrentTask { get; private set; }

	public bool ShowWaves { get; set; } = true;

	public bool ShowResultingWave { get; set; } = true;

	public bool ShowGroup { get; set; } = true;

	public int TaskNumber => _currentTaskIndex + 1;

	public void NextTask()
	{
		_currentTaskIndex = (_currentTaskIndex + 1) % _gameTasks.Length;
		CurrentTask = _gameTasks[_currentTaskIndex];

		RaiseAllPropertiesChanged();
	}
}
