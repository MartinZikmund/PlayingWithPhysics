using Physics.InclinedPlane.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return new BasicMotionSetup(Angle, Mass, DriftCoefficient, Length, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }
        public override string Label { get; set; }
    }
}
