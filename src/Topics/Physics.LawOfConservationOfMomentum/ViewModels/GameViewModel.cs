#nullable enable

using System;
using System.Linq;
using System.Windows.Input;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.LawOfConservationOfMomentum.Rendering;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Physics.LawOfConservationOfMomentum.ViewModels
{
	public class GameViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<GameController>
	{
		private readonly DispatcherTimer _timer = new DispatcherTimer();
		private GameController? _controller;
		internal readonly FontFamily LabelFont = new ("/Assets/Game/Fonts/DigitalNormal-xO6j#Digital");

		public GameViewModel()
		{
			//_timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			//_timer.Tick += TimerTick;
			//GameInfo.StartNewGame();
			//GameInfo.GameStateChanged += GameInfo_GameStateChanged;
			//RaisePropertyChanged(nameof(AllowAngleChanges));
		}

		//private void GameInfo_GameStateChanged(object sender, EventArgs e)
		//{
		//	RaisePropertyChanged(nameof(AllowAngleChanges));
		//	RaisePropertyChanged(nameof(AllowButtonClick));
		//	RaisePropertyChanged(nameof(ButtonText));
		//	RaisePropertyChanged(nameof(DialogText));
		//}

		public override void Prepare(SimulationNavigationModel parameter)
		{
		}

		//public GameInfo GameInfo { get; } = new GameInfo();

		//public bool AllowAngleChanges => GameInfo.State == GameState.SetAngle;

		//public bool AllowButtonClick => GameInfo.State != GameState.Simulation;

		//public string ButtonText
		//{
		//	get
		//	{
		//		if (GameInfo.State == GameState.SimulationEnded)
		//		{
		//			return Localizer.Instance.GetString("NextAttempt");
		//		}
		//		else if (GameInfo.State == GameState.GameEnded)
		//		{
		//			return Localizer.Instance.GetString("NewGame");
		//		}
		//		return Localizer.Instance.GetString("StartRolling");
		//	}
		//}

		//public MotionSetup Setup { get; set; }

		//public MotionViewModel? Motion { get; set; }

		//public string DialogText
		//{
		//	get
		//	{
		//		if (GameInfo.State == GameState.GameEnded)
		//		{
		//			var bestAttempt = GameInfo.BestTime ?? 0;

		//			var relativeDifference = (Math.Abs(GameInfo.DesiredTime - bestAttempt) / GameInfo.DesiredTime) * 100;

		//			var dialog = "";
		//			if (relativeDifference == 0)
		//			{
		//				dialog = string.Format(Localizer.Instance.GetString("Dialog_EndGame_Exact"), bestAttempt.ToString("0.0"));
		//			}
		//			else
		//			{
		//				dialog = string.Format(Localizer.Instance.GetString("Dialog_EndGame_NotExact"), bestAttempt.ToString("0.0"), relativeDifference.ToString("0"));
		//			}
		//			if (relativeDifference <= 5)
		//			{
		//				dialog += " " + Localizer.Instance.GetString("Dialog_EndGame_Under5");
		//			}
		//			else if (relativeDifference <= 10)
		//			{
		//				dialog += " " + Localizer.Instance.GetString("Dialog_EndGame_Under10");
		//			}
		//			else
		//			{
		//				dialog += " " + Localizer.Instance.GetString("Dialog_EndGame_Over10");
		//			}

		//			return dialog;
		//		}
		//		return string.Format(Localizer.Instance.GetString("Dialog_SetAngle"), GameInfo.DesiredTimeText);
		//	}
		//}

		public void SetController(GameController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
		}

		//public ICommand NextShotCommand => GetOrCreateCommand(() =>
		//{

		//});

		//public ICommand StartCommand => GetOrCreateCommand(() =>
		//{
		//	if (GameInfo.State == GameState.SimulationEnded)
		//	{
		//		GameInfo.NextShot();
		//	}
		//	else if (GameInfo.State == GameState.GameEnded)
		//	{
		//		StartNewGame();
		//	}
		//	else
		//	{
		//		RestartSimulation();
		//	}
		//});

		//public ICommand NewGameCommand => GetOrCreateCommand(() =>
		//{
		//	GameInfo.StartNewGame();
		//});

		//public ICommand StartNewGameCommand => GetOrCreateCommand(StartNewGame);

		//private void StartNewGame()
		//{
		//	if (_renderer is null)
		//	{
		//		throw new InvalidOperationException("Game can't start this early");
		//	}
		//	Motion = null;
		//	_timer.Stop();
		//	GameInfo.StartNewGame();
		//	_renderer.StartGame(GameInfo);
		//}

		//private void TimerTick(object sender, object e)
		//{
		//	if (_timer.IsEnabled && _controller != null && GameInfo.State == GameState.Simulation)
		//	{
		//		float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;

		//		Motion?.UpdateCurrentValues(timeElapsed);

		//		if (timeElapsed >= _controller.PhysicsServices.First().CalculateMaxT())
		//		{
		//			GameInfo.AddAttempt(_controller.PhysicsServices.First().CalculateMaxT());
		//			_timer.Stop();
		//		}
		//	}
		//}

		//protected void RestartSimulation()
		//{
		//	Motion = new MotionViewModel(GameInfo.CreateGameMotionSetupWithAngle(GameInfo.CurrentAngle));
		//	if (_controller == null) return;

		//	_controller.StartSimulation(Motion.MotionInfo);
		//	GameInfo.StartAttempt();
		//	_timer.Start();
		//}
	}
}
