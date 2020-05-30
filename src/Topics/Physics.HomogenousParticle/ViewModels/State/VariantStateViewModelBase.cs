using Physics.HomogenousParticle.Services;
using Physics.Shared.ViewModels;

namespace Physics.HomogenousParticle.ViewModels.State
{
    public abstract class VariantStateViewModelBase : ViewModelBase, IVariantStateViewModel
    {
        protected VariantStateViewModelBase(IMotionSetup motionSetup)
        {
            Motion = motionSetup;
        }

        public IMotionSetup Motion { get; }
    }
}
