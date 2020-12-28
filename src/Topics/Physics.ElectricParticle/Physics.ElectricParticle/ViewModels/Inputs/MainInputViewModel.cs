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
        private InputVariant _variant;
        public MainInputViewModel(InputVariant variant)
        {
            _variant = variant;
            SelectedPrimaryPlaneChargePolarity = PrimaryPlaneChargePolarities[0];
            SelectedChargePolarity = ChargePolarities[0];
            SelectedVelocityDirection = VelocityDirections[0];
            //Advanced-1, secondary
            SelectedSecondaryPlaneChargePolarity = SecondaryPlaneChargePolarities[0];
            SelectedEnvironmentSetting = EnvironmentSettings[0];
        }

		public override async Task<IMotionSetup> CreateMotionSetupAsync()
        {
            var colorSerialized = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color);
            return new MotionSetup(
				_variant,
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
                SelectedVelocityDirection,
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
        public Visibility AdvancedFirstOption { get => (_variant == InputVariant.AdvancedVerticalHorizontal) ? Visibility.Visible : Visibility.Collapsed; }

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

        public Visibility DeviationVisibility { get => (_variant != InputVariant.EasyHorizontalWithGravity) ? Visibility.Visible : Visibility.Collapsed; }

        public float ChargeBase { get; set; }

        public float ChargePower { get; set; }

        public float MassBase { get; set; }

        public float MassPower { get; set; }

        public VelocityDirection SelectedVelocityDirection { get; set; }

        public ObservableCollection<VelocityDirection> VelocityDirections { get; } = new ObservableCollection<VelocityDirection>()
        {
            VelocityDirection.VerticallyDown,
            VelocityDirection.VerticallyUp
        };

        public Visibility VelocityDirectionVisibility { get => (_variant == InputVariant.EasyHorizontalWithGravity) ? Visibility.Visible : Visibility.Collapsed; }

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
        public string PrimaryPlaneLabel
        {
            get
            {
                int variantIndex = (int)_variant;
                switch(variantIndex) {
                    case 0:
                    case 3:
                    case 4:
                        return Localizer.Instance["Input_PlaneLabelLeft"]; 
                    default:
                        return Localizer.Instance["Input_PlaneLabelBottom"];
                }
            }
        }

        public string PrimaryPlaneVoltageLabel
        {
            get
            {
                int variantIndex = (int)_variant;
                switch (variantIndex)
                {
                    case 0:
                    case 1:
                        return Localizer.Instance["Input_PlaneVoltageBetweenGeneral"];
                    case 2:
                    case 3:
                        return Localizer.Instance["Input_PlaneVoltageBetweenHorizontal"];
                    default:
                        return Localizer.Instance["Input_PlaneVoltageBetweenVertical"];
                }
            }
        }

        public string PrimaryPlaneDistanceLabel
        {
            get
            {
                int variantIndex = (int)_variant;
                switch (variantIndex)
                {
                    case 0:
                    case 1:
                        return Localizer.Instance["Input_PlaneDistanceGeneral"];
                    case 3:
                    case 4:
                        return Localizer.Instance["Input_PlaneDistanceVertical"];
                    default:
                        return Localizer.Instance["Input_PlaneDistanceHorizontal"];
                }
            }
        }
        public Thickness VariantBasedMargin => (_variant == InputVariant.AdvancedVerticalHorizontal) ? new Thickness(0, 0, 20, 0) : new Thickness(0);
        public override string Label { get; set; }
    }
}
