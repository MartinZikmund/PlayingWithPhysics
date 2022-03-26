using Physics.ElectricParticle.Logic;
using System.Threading.Tasks;

namespace Physics.ElectricParticle.ViewModels.Inputs
{
	public interface IInputViewModel
    {
        Task<ElectricParticleSimulationSetup> CreateMotionSetupAsync();
    }
}
