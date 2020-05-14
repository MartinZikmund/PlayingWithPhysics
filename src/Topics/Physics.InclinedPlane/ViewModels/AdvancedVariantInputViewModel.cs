using Physics.InclinedPlane.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.ViewModels
{
    public class AdvancedVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            return new AdvancedMotionSetup(Elevation, Length, DriftCoefficient, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float Elevation { get; set; }

        public float Length { get; set; }
        public float DriftCoefficient { get; set; }
        public float Gravity { get; set; }

        public override string Label { get; set; }
    }
}
