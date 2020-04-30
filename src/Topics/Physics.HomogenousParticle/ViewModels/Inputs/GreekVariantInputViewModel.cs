using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class GreekVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            return new GreekMotionSetup(Velocity, Type, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float Velocity { get; set; } = (float)Math.PI / 3; //v = pi/3

        public RadiationType Type { get; set; }

        public override string Label { get; set; }
    }

    public enum RadiationType
    {
        Alfa,
        BetaMinus,
        BetaPlus,
        Neutron
    }
}
