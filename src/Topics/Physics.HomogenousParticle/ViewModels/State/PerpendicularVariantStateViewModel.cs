using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic.PhysicsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.State
{
    public class PerpendicularVariantStateViewModel : VariantStateViewModelBase
    {
        private readonly PerpendicularPhysicsService _physicsService;

        public PerpendicularVariantStateViewModel(IMotionSetup motionSetup) : base(motionSetup)
        {
            _physicsService = new PerpendicularPhysicsService((PerpendicularMotionSetup)motionSetup);
        }

        public string T => _physicsService.ComputeT().ToString("0.#########");

        public string Velocity => _physicsService.ComputeVelocity().ToString();

        public string Omega => _physicsService.ComputeOmega().ToString("0");

        public string Radius => _physicsService.ComputeRadius().ToString("0.#######");
    }
}
