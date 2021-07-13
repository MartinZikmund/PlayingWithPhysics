using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.LawOfConservationOfMomentum.Rendering;

namespace Physics.LawOfConservationOfMomentum.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<LawOfConservationOfMomentumCanvasController>
	{
		private DifficultyOption _difficulty;
		private LawOfConservationOfMomentumCanvasController _controller;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

		public void SetController(LawOfConservationOfMomentumCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}
	}
}
