using Physics.Shared.ViewModels;

namespace Physics.Shared.UI.ViewModels
{
	public abstract class SimulationViewModelBase<TNavigationModel> : ViewModelBase<TNavigationModel>
        where TNavigationModel : class
    {
        public SimulationPlaybackViewModel SimulationPlayback { get; } = new SimulationPlaybackViewModel();
    }
}
