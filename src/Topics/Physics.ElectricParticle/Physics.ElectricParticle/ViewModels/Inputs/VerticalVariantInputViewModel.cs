using Physics.ElectricParticle.Logic;
using Physics.Shared.UI.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Physics.ElectricParticle.ViewModels.Inputs
{
    public class VerticalVariantInputViewModel : VariantInputViewModelBase
    {
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            //if (Charge == 0)
            //{
            //    await new MessageDialog("Náboj nesmí být 0").ShowAsync();
            //    return null;
            //}
            var colorSerialized = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color);
            return new MotionSetup(SelectedLeftPlaneChargePolarity, colorSerialized);
        }

        public VerticalLeftPlaneChargePolarity SelectedLeftPlaneChargePolarity { get; set;  }

        public ObservableCollection<VerticalLeftPlaneChargePolarity> LeftPlaneChargePolarities { get; } = new ObservableCollection<VerticalLeftPlaneChargePolarity>()
        {
            VerticalLeftPlaneChargePolarity.Positive,
            VerticalLeftPlaneChargePolarity.Negative
        };

        public float Voltage { get; set; }
        public float PlaneDistance { get; set; }

        public VerticalChargePolarity SelectedChargePolarity { get; set; }

        public ObservableCollection<VerticalChargePolarity> ChargePolarities { get; } = new ObservableCollection<VerticalChargePolarity>()
        {
            VerticalChargePolarity.Positive,
            VerticalChargePolarity.Negative
        };

        public float Velocity { get; set; }
        public float Deviation { get; set; }
        public float ChargeBase { get; set; }
        public float ChargePower { get; set; }
        public float MassBase { get; set; }
        public float MassPower { get; set; }

        public override string Label { get; set; }
    }
}
