using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class PerpendicularVariantInputViewModel : VariantInputViewModelBase
    {
        private float _massMultiple = 1;

        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            if (ChargeMultiple == 0)
            {
                await new MessageDialog("Náboj nesmí být 0").ShowAsync();
                return null;
            }
            return new PerpendicularMotionSetup(ChargeMultiple, MassMultiple, VelocityMultiple, Induction, InductionOrientation, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        }

        public float ChargeMultiple { get; set; } = 1;

        public float MassMultiple
        {
            get
            {
                return _massMultiple;
            }
            set
            {
                if (value < 1f && value != 0.000545f)
                {
                    _massMultiple = 1f;
                }
                else
                {
                    _massMultiple = value;
                }
            }
        }

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
