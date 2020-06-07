using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class RadiationVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            return new RadiationMotionSetup(Velocity, Type, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float Velocity { get; set; } = (float)Math.PI / 3; //v = pi/3

        public RadiationType Type { get; set; }

        public RadiationType[] Types = new[]
        {
            RadiationType.Alpha,
            RadiationType.BetaPlus,
            RadiationType.BetaMinus,
            RadiationType.Gamma,
            RadiationType.Neutron
        };

        public override string Label { get; set; }
    }
}
