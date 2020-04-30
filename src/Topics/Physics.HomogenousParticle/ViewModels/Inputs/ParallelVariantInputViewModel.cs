using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class ParallelVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            if (Charge == 0)
            {
                await new MessageDialog("Náboj nesmí být 0").ShowAsync();
                return null;
            }
            return new ParallelMotionSetup(Velocity, Angle, Charge, InductionOrientation, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float Velocity { get; set; } // 10^n: 2<=n<=6

        public float Angle { get; set; } // po směru: 1, proti směru: -1

        public float Charge { get; set; } = 0.1f; // -3<=q<=3, not 0

        public float InductionOrientation { get; set; } // 0 <= B <= 360

        public override string Label { get; set; }
    }
}
