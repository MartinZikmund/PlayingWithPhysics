using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.ViewModels
{
    public abstract class SimulationViewModelBase<TNavigationModel> : ViewModelBase<TNavigationModel>
        where TNavigationModel : class
    {
        public SimulationPlaybackViewModel SimulationPlayback { get; } = new SimulationPlaybackViewModel();
    }
}
