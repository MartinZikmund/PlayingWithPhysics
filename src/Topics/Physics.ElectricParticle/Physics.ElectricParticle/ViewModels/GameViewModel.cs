#nullable enable

using System;
using Physics.ElectricParticle.Game;
using Physics.ElectricParticle.Models;
using Physics.ElectricParticle.Rendering;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.ElectricParticle.ViewModels
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
	}
}
