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

		});

		public ICommand ShootCommand => GetOrCreateCommand(() =>
		{

		});

		public ICommand NewGameCommand => GetOrCreateCommand(() =>
		{
			GameInfo.StartNewGame();
		});
	}
}
