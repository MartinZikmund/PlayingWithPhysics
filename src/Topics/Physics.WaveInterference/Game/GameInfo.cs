using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Physics.Shared.UI.Localization;
using Windows.UI.Xaml.Controls;

namespace Physics.WaveInterference.Game;

public class GameInfo : MvxNotifyPropertyChanged
{
	private static readonly GameTask[] _gameTasks = GameTasks.All;

	private int _currentTaskIndex = 0;

	public GameInfo()
	{
		GamePhysicsService = new(Wave1, Wave2);
		CurrentTask = _gameTasks[0];
		CheckResultCommand = new MvxAsyncCommand(CheckResultAsync);
		Wave1.PropertyChanged += WavePropertyChanged;
		Wave2.PropertyChanged += WavePropertyChanged;
	}

	private void WavePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		EvaluatedVf = "?";
		EvaluatedVg = "?";
	}

	public GameWaveInfo Wave1 { get; } = new(3.3f, 1.3f);

	public GameWaveInfo Wave2 { get; } = new(1.7f, 2.3f);

	public GamePhysicsService GamePhysicsService { get; }

	public GameTask CurrentTask { get; private set; }

	public bool ShowWaves { get; set; } = false;

	public bool ShowResultingWave { get; set; } = true;

	public bool ShowGroup { get; set; } = true;

	public int TaskNumber => _currentTaskIndex + 1;

	public string EvaluatedVg { get; private set; } = "?";

	public string EvaluatedVf { get; private set; } = "?";

	public void NextTask()
	{
		_currentTaskIndex = (_currentTaskIndex + 1) % _gameTasks.Length;
		CurrentTask = _gameTasks[_currentTaskIndex];

		RaiseAllPropertiesChanged();
		EvaluatedVf = "?";
		EvaluatedVg = "?";
	}

	public ICommand CheckResultCommand { get; }

	public async Task CheckResultAsync()
	{
		var contentDialog = new ContentDialog();

		var vf = (float)GamePhysicsService.CalculateVf();
		var vg = (float)GamePhysicsService.CalculateVg();

		EvaluatedVf = vf.ToString("0.0");
		EvaluatedVg = vg.ToString("0.0");

		contentDialog.Title = Localizer.Instance.GetString("GameEvaluation");
		if (CurrentTask.Evaluate(vf, vg))
		{
			contentDialog.Content = Localizer.Instance.GetString("GameSuccessMessage");
			contentDialog.Title = Localizer.Instance.GetString("GameSuccessTitle");
		}
		else
		{
			contentDialog.Content = Localizer.Instance.GetString("GameFailureMessage");
			contentDialog.Title = Localizer.Instance.GetString("GameFailureTitle");
		}

		contentDialog.IsPrimaryButtonEnabled = true;
		contentDialog.PrimaryButtonText = Localizer.Instance.GetString("Ok");

		await contentDialog.ShowAsync();
	}
}
