using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

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
            var colorSerialized = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color);
            return new ParallelMotionSetup(
                Velocity, 
                SelectedOrientation, 
                Charge, 
                InductionOrientation, 
                colorSerialized);
        }

        public float Velocity { get; set; } // 10^n: 2<=n<=6

        public ParallelVariantOrientation[] VariantOrientations { get; } = new ParallelVariantOrientation[]
        {
            ParallelVariantOrientation.Normal,
            ParallelVariantOrientation.Opposite
        };

        public ParallelVariantOrientation SelectedOrientation { get; set; }

        public float Charge { get; set; } = 1f; // -3<=q<=3, not 0

        public float InductionOrientation { get; set; } // 0 <= B <= 360

        public override string Label { get; set; }
    }
}
