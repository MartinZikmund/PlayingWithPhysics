using Physics.CompoundOscillations.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.CompoundOscillations.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<DifficultyNavigationModel>,
		ISetSimulationViewInteraction<CompoundOscillationsController>
	{
		internal DifficultyOption Difficulty { get; private set; }

		public override void Prepare(DifficultyNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

		public void SetViewInteraction(ISimulationViewInteraction<CompoundOscillationsController> controller)
		{

		}
	}
}
