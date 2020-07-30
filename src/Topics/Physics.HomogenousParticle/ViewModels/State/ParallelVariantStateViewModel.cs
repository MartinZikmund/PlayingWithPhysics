using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic.PhysicsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.State
{
    public class ParallelVariantStateViewModel : VariantStateViewModelBase
    {
        private readonly ParallelPhysicsService _physicsService;
        private readonly ParallelMotionSetup _motionSetup;
        public ParallelVariantStateViewModel(IMotionSetup motionSetup) : base(motionSetup)
        {
            _motionSetup = (ParallelMotionSetup)motionSetup;
            _physicsService = new ParallelPhysicsService(_motionSetup);   
        }

        public string Multiple => _motionSetup.Velocity.ToString();
    }
}
