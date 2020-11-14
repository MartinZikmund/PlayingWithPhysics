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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.ElectricParticle.ViewModels.Inputs
{
    public class MainInputViewModel : InputViewModelBase
    {
        private PlaneOrientation _variant;
        public MainInputViewModel(PlaneOrientation variant)
        {
            _variant = variant;
            SelectedPrimaryPlaneChargePolarity = PrimaryPlaneChargePolarities[0];
            SelectedChargePolarity = ChargePolarities[0];
            //Advanced-1, secondary
            SelectedSecondaryPlaneChargePolarity = SecondaryPlaneChargePolarities[0];
            SelectedEnvironmentSetting = EnvironmentSettings[0];
        }
        public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            var colorSerialized = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color);
            return new MotionSetup(
                SelectedPrimaryPlaneChargePolarity,
                PrimaryVoltage,
                PrimaryPlaneDistance,
                SelectedSecondaryPlaneChargePolarity,
                SecondaryVoltage,
                SecondaryPlaneDistance,
                SelectedChargePolarity,
                ChargeBase,
                ChargePower,
                MassBase,
                MassPower,
                Velocity,
                Deviation,
                SelectedEnvironmentSetting,
                colorSerialized); ;
        }

        public PrimaryPlaneChargePolarity SelectedPrimaryPlaneChargePolarity { get; set; }
        public ObservableCollection<PrimaryPlaneChargePolarity> PrimaryPlaneChargePolarities { get; } = new ObservableCollection<PrimaryPlaneChargePolarity>()
        {
            PrimaryPlaneChargePolarity.Positive,
            PrimaryPlaneChargePolarity.Negative
        };
        public float PrimaryVoltage { get; set; }
        public float PrimaryPlaneDistance { get; set; }

        //Advanced-1, secondary options
        public Visibility AdvancedFirstOption { get => (_variant == PlaneOrientation.AdvancedVerticalHorizontal) ? Visibility.Visible : Visibility.Collapsed; }
        public SecondaryPlaneChargePolarity SelectedSecondaryPlaneChargePolarity { get; set; }
        public ObservableCollection<SecondaryPlaneChargePolarity> SecondaryPlaneChargePolarities { get; } = new ObservableCollection<SecondaryPlaneChargePolarity>()
        {
            SecondaryPlaneChargePolarity.Positive,
            SecondaryPlaneChargePolarity.Negative
        };
        public float SecondaryVoltage { get; set; }
        public float SecondaryPlaneDistance { get; set; }
        //END: Advanced-1, secondary options
        
        //General input values
        public ChargePolarity SelectedChargePolarity { get; set; }
        public ObservableCollection<ChargePolarity> ChargePolarities { get; } = new ObservableCollection<ChargePolarity>()
        {
            ChargePolarity.Positive,
            ChargePolarity.Negative
        };

        public float Velocity { get; set; }
        public float Deviation { get; set; }
        public float ChargeBase { get; set; }
        public float ChargePower { get; set; }
        public float MassBase { get; set; }
        public float MassPower { get; set; }

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

        //Other props
        public override string Label { get; set; }
    }
}
