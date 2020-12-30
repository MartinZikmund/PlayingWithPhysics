using Physics.Shared.ViewModels;
using System.Threading.Tasks;
using Windows.UI;
using Physics.ElectricParticle.Logic;
using Physics.Shared.UI.Models.Input;

namespace Physics.ElectricParticle.ViewModels.Inputs
{
	public abstract class InputViewModelBase : ViewModelBase, IInputViewModel
    {
		public SimulationColors Colors { get; } = new SimulationColors();

        public abstract Task<IMotionSetup> CreateMotionSetupAsync();
    }
}
