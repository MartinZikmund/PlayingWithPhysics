using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class PerpendicularVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            return new PerpendicularMotionSetup(Velocity, Angle, Charge, InductionOrientation, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float Velocity { get; set; } // n^6: 1<=n<=10

        public float Angle { get; set; } // 0<=alfa<=360, skoky po deseti stupních

        public float Charge { get; set; } // -1 nebo 1, bez velikosti

        public PerpendicularInductionOrientation InductionOrientation { get; set; } // 180: do papíru, 0: z papíru

        public override string Label { get; set; }
    }
}
