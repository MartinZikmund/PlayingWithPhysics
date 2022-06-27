#nullable enable

using System;
using System.Windows.Input;
using Physics.RotationalInclinedPlane.Game;
using Physics.RotationalInclinedPlane.Rendering;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class GameViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<RotationalInclinedPlaneCanvasController>
	{
		private RotationalInclinedPlaneCanvasController? _controller;
		private GameRenderer? _renderer = null;

		public override void Prepare(SimulationNavigationModel parameter)
		{
		}

		public GameInfo GameInfo { get; } = new GameInfo();

		public void SetController(RotationalInclinedPlaneCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_renderer = (GameRenderer)controller.Renderer;
			_renderer.GameInfo = GameInfo;
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
