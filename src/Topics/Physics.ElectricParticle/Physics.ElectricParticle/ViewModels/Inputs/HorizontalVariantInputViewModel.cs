using Physics.ElectricParticle.Logic;
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
    public class HorizontalVariantInputViewModel : VariantInputViewModelBase
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
        public VerticalLeftPlaneChargePolarity SelectedLeftPlaneChargePolarity { get; set; }
        public ObservableCollection<VerticalLeftPlaneChargePolarity> LeftPlaneChargePolarities { get; } = new ObservableCollection<VerticalLeftPlaneChargePolarity>()
        {
            VerticalLeftPlaneChargePolarity.Positive,
            VerticalLeftPlaneChargePolarity.Negative
        };

        public override string Label { get; set; }
    }
}
