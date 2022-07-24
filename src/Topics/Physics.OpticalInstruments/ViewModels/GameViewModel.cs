#nullable enable

using System;
using System.Windows.Input;
using Physics.OpticalInstruments.Game;
using Physics.OpticalInstruments.Rendering;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.OpticalInstruments.ViewModels
{
	public class GameViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<GameCanvasController>
	{
		private GameCanvasController? _controller;

		public GameViewModel()
		{
			GameInfo.StartNewGame();
			GameInfo.GameStateChanged += GameInfo_GameStateChanged;
		}

		private void GameInfo_GameStateChanged(object sender, EventArgs e)
		{
			RaisePropertyChanged(nameof(IsNewGameVisible));
			RaisePropertyChanged(nameof(IsShootVisible));
			RaisePropertyChanged(nameof(IsNextShotVisible));
		}

		public bool IsNewGameVisible => GameInfo.State == GameState.GameEnded;

		public bool IsNextShotVisible => GameInfo.State == GameState.Fired;

		public bool IsShootVisible => GameInfo.State == GameState.SetAngle;

		public override void Prepare(SimulationNavigationModel parameter)
		{
		}

		public GameInfo GameInfo { get; } = new GameInfo();

		public void SetController(GameCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_controller.GameInfo = GameInfo;
		}
		
		public float Angle { get; set; }

		public ICommand NextShotCommand => GetOrCreateCommand(() =>
		{
			GameInfo.NextShot();
		});

		public ICommand ShootCommand => GetOrCreateCommand(() =>
		{
			GameInfo.Shoot();
		});

		public ICommand NewGameCommand => GetOrCreateCommand(() =>
		{
			GameInfo.StartNewGame();
		});
	}
}
