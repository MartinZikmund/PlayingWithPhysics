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
    public class MainInputViewModel : InputViewModelBase
    {
        public MainInputViewModel()
        {
            //SelectedEnvironmentSettingIndex = 0;
            SelectedEnvironmentSetting = EnvironmentSettings[0];
        }
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

        public int SelectedEnvironmentSettingIndex { get; set; }
        public EnvironmentSetting SelectedEnvironmentSetting { get; set; }
        public ObservableCollection<EnvironmentSetting> EnvironmentSettings { get; } = new ObservableCollection<EnvironmentSetting>()
        {
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_Vacuum"], 1.0f),
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_Air"], 1.0f),
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_Kerosene"], 2.0f),
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_CastorOil"], 5.0f),
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_Ethanol"], 24.0f),
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_Methanol"], 34.0f),
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_Glycerol"], 43.0f),
            new EnvironmentSetting(Localizer.Instance["EnvironmentSetting_Water"], 81.0f)
        };

        public override string Label { get; set; }
    }
}
