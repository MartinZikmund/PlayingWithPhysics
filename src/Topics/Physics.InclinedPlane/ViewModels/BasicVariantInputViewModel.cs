using Physics.InclinedPlane.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Physics.InclinedPlane.ViewModels
{
    public class BasicVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            return new BasicMotionSetup(Elevation, Length, DriftCoefficient, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float Elevation { get; set; }

        public float Length { get; set; }
        public float DriftCoefficient { get; set; }

        public override string Label { get; set; }
    }
}
