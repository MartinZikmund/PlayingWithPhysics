using Physics.HomogenousParticle.Services;
using Physics.HomogenousParticle.Views;
using Physics.HomongenousParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.State
{
    public class RadiationVariantStateViewModel : VariantStateViewModelBase
    {
        private readonly MainViewModel _viewModel;

        public RadiationVariantStateViewModel(MainViewModel viewModel, IMotionSetup motionSetup) : base(motionSetup)
        {
            _viewModel = viewModel;
        }

        public RadiationType Type => ((RadiationMotionSetup)Motion).Type;

        public void Delete()
        {
            _viewModel.Delete(this);
        }
    }
}