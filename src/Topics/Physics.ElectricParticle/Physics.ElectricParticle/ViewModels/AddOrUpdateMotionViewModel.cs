using Physics.ElectricParticle.Logic;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Physics.ElectricParticle.ViewModels
{
    public class AddOrUpdateMotionViewModel : ViewModelBase
    {
        private float _massMultiple = 1;

        //public override async Task<IMotionSetup> CreateMotionSetupAsync()
        //{
        //    if (ChargeMultiple == 0)
        //    {
        //        await new MessageDialog("Násebek nesmí být 0").ShowAsync();
        //        return null;
        //    }
        //    return new MotionSetup(ChargeMultiple, MassMultiple, VelocityMultiple, Induction, InductionOrientation, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
        //}

        public async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            if (ChargeMultiple == 0)
            {
                await new MessageDialog("Násebek nesmí být 0").ShowAsync();
                return null;
            }
            //return new MotionSetup(ChargeMultiple, MassMultiple, VelocityMultiple, Induction, InductionOrientation, Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
            return new MotionSetup(ChargePolarity, "#FF0000");

        }
        public VerticalLeftPlaneChargePolarity ChargePolarity { get; set; }
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

        //public override string Label { get; set; }
    }
}
