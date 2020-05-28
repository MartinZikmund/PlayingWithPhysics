using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return new PerpendicularMotionSetup(ChargeMultiple, MassMultiple, VelocityMultiple, Induction, InductionOrientation, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float ChargeMultiple { get; set; } = 1;

        public float MassMultiple { get; set; } = 1.67f;

        public float VelocityMultiple { get; set; } = 1;

        public float Induction { get; set; } = 0.1f;

        public PerpendicularInductionOrientation InductionOrientation { get; set; }

        public ObservableCollection<PerpendicularInductionOrientation> InductionOrientations { get; } =
            new ObservableCollection<PerpendicularInductionOrientation>() 
            {
                PerpendicularInductionOrientation.FromPaper,
                PerpendicularInductionOrientation.IntoPaper
            };

        public override string Label { get; set; }
    }
}
