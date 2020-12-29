using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Physics.ElectricParticle.Logic;
using Physics.Shared.UI.Localization;
using Windows.UI.Xaml;

namespace Physics.ElectricParticle.ViewModels.Inputs
{
	public class MainInputViewModel : InputViewModelBase
	{
		private readonly InputVariant _inputVariant;

		public MainInputViewModel(InputVariant variant)
		{
			_inputVariant = variant;

			InitializeParticleTypes();			

			SelectedPrimaryPlaneChargePolarity = PrimaryPlaneChargePolarities[0];
			SelectedChargePolarity = ChargePolarities[0];
			SelectedVelocityDirection = VelocityDirections[0];
			//Advanced-1, secondary
			SelectedSecondaryPlaneChargePolarity = SecondaryPlaneChargePolarities[0];
			SelectedEnvironmentSetting = EnvironmentSettings[0];
		}

		private void InitializeParticleTypes()
		{
			ParticleTypes = VariantConfigurations.All
				.Where(v => v.InputVariant == _inputVariant)
				.Select(v => v.ParticleType)
				.OrderBy(v => (int)v)
				.ToArray();
			ParticleType = ParticleTypes[0];
		}

		public override async Task<IMotionSetup> CreateMotionSetupAsync()
		{
			var colorSerialized = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color);
			return new MotionSetup(
				_inputVariant,
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
				colorSerialized);
		}

		public ParticleType[] ParticleTypes { get; private set; }

		public object ParticleType { get; set; }

		public VariantConfiguration VariantConfiguration { get; set; }

		public PrimaryPlaneChargePolarity SelectedPrimaryPlaneChargePolarity { get; set; }

		public ObservableCollection<PrimaryPlaneChargePolarity> PrimaryPlaneChargePolarities { get; } = new ObservableCollection<PrimaryPlaneChargePolarity>()
		{
			PrimaryPlaneChargePolarity.Positive,
			PrimaryPlaneChargePolarity.Negative
		};

		public float PrimaryVoltage { get; set; }

		public float PrimaryPlaneDistance { get; set; }

		//Advanced-1, secondary options
		public Visibility AdvancedFirstOption { get => (_inputVariant == InputVariant.AdvancedVerticalHorizontal) ? Visibility.Visible : Visibility.Collapsed; }

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

		public Visibility DeviationVisibility { get => (_inputVariant != InputVariant.EasyHorizontalWithGravity) ? Visibility.Visible : Visibility.Collapsed; }

		public float ChargeBase { get; set; }

		public float ChargePower { get; set; }

		public float ChargeMultiplier { get; set; }

		public float MassBase { get; set; }

		public float MassPower { get; set; }

		public VelocityDirection SelectedVelocityDirection { get; set; }

		public ObservableCollection<VelocityDirection> VelocityDirections { get; } = new ObservableCollection<VelocityDirection>()
		{
			VelocityDirection.VerticallyDown,
			VelocityDirection.VerticallyUp
		};

		public Visibility VelocityDirectionVisibility { get => (_inputVariant == InputVariant.EasyHorizontalWithGravity) ? Visibility.Visible : Visibility.Collapsed; }

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
				int variantIndex = (int)_inputVariant;
				switch (variantIndex)
				{
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
				int variantIndex = (int)_inputVariant;
				switch (variantIndex)
				{
					case 0:
					case 1:
					case 2:
						return Localizer.Instance["Input_PlaneVoltageBetweenGeneral"];
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
				int variantIndex = (int)_inputVariant;
				switch (variantIndex)
				{
					case 0:
					case 1:
					case 2:
						return Localizer.Instance["Input_PlaneDistanceGeneral"];
					case 3:
						return Localizer.Instance["Input_PlaneDistanceVertical"];
					default:
						return Localizer.Instance["Input_PlaneDistanceHorizontal"];
				}
			}
		}
		public Thickness VariantBasedMargin => (_inputVariant == InputVariant.AdvancedVerticalHorizontal) ? new Thickness(0, 0, 20, 0) : new Thickness(0);
		public override string Label { get; set; }

		private void OnParticleTypeChanged()
		{
			VariantConfiguration = VariantConfigurations.All.FirstOrDefault(
				c => c.InputVariant == _inputVariant &&
				c.ParticleType == (ParticleType?)ParticleType);
		}
	}
}
