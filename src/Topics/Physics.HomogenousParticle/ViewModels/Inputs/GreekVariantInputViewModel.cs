using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class GreekVariantInputViewModel : IVariantInputViewModel
    {
        public async Task<IMotionSetup> CreateMotionSetup()
        {
            return new GreekMotionSetup(Velocity, Type);
        }

        public float Velocity { get; set; } = (float)Math.PI / 3; //v = pi/3
        public RadiationType Type { get; set; }
        public string Label { get; set; }
    }

    public enum RadiationType
    {
        Alfa,
        BetaMinus,
        BetaPlus,
        Neutron
    }
}
