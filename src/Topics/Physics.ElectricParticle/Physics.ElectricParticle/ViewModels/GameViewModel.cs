using Physics.ElectricParticle.Models;
using Physics.ElectricParticle.Rendering;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.ElectricParticle.ViewModels
{
	public class GameViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<GameCanvasController>
	{
		public override void Prepare(SimulationNavigationModel parameter) { }
		public void SetController(GameCanvasController controller) { }
	}
}
