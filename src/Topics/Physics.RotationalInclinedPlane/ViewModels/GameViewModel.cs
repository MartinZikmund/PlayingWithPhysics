#nullable enable

using System;
using System.Linq;
using System.Windows.Input;
using Physics.RotationalInclinedPlane.Game;
using Physics.RotationalInclinedPlane.Logic;
using Physics.RotationalInclinedPlane.Rendering;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Windows.UI.Xaml;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class GameViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<RotationalInclinedPlaneCanvasController>
	{
		private readonly DispatcherTimer _timer = new DispatcherTimer();
		private RotationalInclinedPlaneCanvasController? _controller;
		private GameRenderer? _renderer = null;


		public GameViewModel()
		{
			_timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			_timer.Tick += TimerTick;
			GameInfo.StartNewGame();
			GameInfo.GameStateChanged += GameInfo_GameStateChanged;
			RaisePropertyChanged(nameof(AllowAngleChanges));
		}

		private void GameInfo_GameStateChanged(object sender, EventArgs e)
		{
			RaisePropertyChanged(nameof(AllowAngleChanges));
		}

		public override void Prepare(SimulationNavigationModel parameter)
		{
		}

		public GameInfo GameInfo { get; } = new GameInfo();

		public bool AllowAngleChanges => GameInfo.State == GameState.SetAngle;

		public MotionSetup Setup { get; set; }

		public MotionViewModel? Motion { get; set; }

		public void SetController(RotationalInclinedPlaneCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_renderer = (GameRenderer)controller.Renderer;
			_renderer.StartGame(GameInfo);			
		}

		public ICommand NextShotCommand => GetOrCreateCommand(() =>
		{

		});

		public ICommand ShootCommand => GetOrCreateCommand(() =>
		{
			RestartSimulation();
		});

		public ICommand NewGameCommand => GetOrCreateCommand(() =>
		{
			GameInfo.StartNewGame();
		});

		public ICommand StartNewGameCommand => GetOrCreateCommand(StartNewGame);

		private void StartNewGame()
		{
			if (_renderer is null)
			{
				throw new InvalidOperationException("Game can't start this early");
			}
			Motion = null;
			_timer.Stop();
			GameInfo.StartNewGame();
			_renderer.StartGame(GameInfo);
		}

		private void TimerTick(object sender, object e)
		{
			if (_timer.IsEnabled && _controller != null && GameInfo.State == GameState.Simulation)
			{
				float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;

				Motion?.UpdateCurrentValues(timeElapsed);

				if (timeElapsed >= _controller.PhysicsServices.First().CalculateMaxT())
				{
					var renderer = (GameRenderer)_controller.Renderer;
					//TODO:MZ
					//GameInfo.AddAttempt(renderer.CalculateDistanceFromTarget());
					if (GameInfo.Attempts.Count != GameInfo.TotalThrows)
					{
						GameInfo.State = GameState.SimulationEnded;
					}
					else
					{
						GameInfo.State = GameState.GameEnded;
					}
					_timer.Stop();
				}
			}
		}

		protected void RestartSimulation()
		{
			Motion = new MotionViewModel(GameInfo.CreateGameMotionSetupWithAngle(GameInfo.CurrentAngle));
			if (_controller == null) return;

			_controller.StartSimulation(Motion.MotionInfo);
			GameInfo.State = GameState.Simulation;
			_timer.Start();
		}
	}
}
