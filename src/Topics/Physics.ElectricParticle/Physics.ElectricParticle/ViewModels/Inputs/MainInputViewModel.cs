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

			PrimaryPlanePolarity = Polarities[0];
			ParticlePolarity = Polarities[0];
			SelectedVelocityDirection = VelocityDirections[0];
			//Advanced-1, secondary
			SelectedSecondaryPlaneChargePolarity = Polarities[0];
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
			PlaneSetup horizontalPlane = null;
			if (_inputVariant == InputVariant.EasyHorizontalNoGravity ||
				_inputVariant == InputVariant.EasyHorizontalWithGravity)
			{
				horizontalPlane = new PlaneSetup(PrimaryPlanePolarity, PrimaryPlaneVoltage, PrimaryPlaneDistance);
			}

			PlaneSetup verticalPlane = null;
			if (_inputVariant == InputVariant.EasyVerticalNoGravity ||
				_inputVariant == InputVariant.AdvancedVerticalWithGravity)
			{
				verticalPlane = new PlaneSetup(PrimaryPlanePolarity, PrimaryPlaneVoltage, PrimaryPlaneDistance);
			}

			// TODO: handle remaining cases

			var particle = new ParticleSetup(
				(ParticleType)ParticleType,
				ParticlePolarity,
				ChargeMultiplier,
				MassMultiplier,
				StartVelocity,
				StartVelocityDeviation); //TODO: Handle velocity direction for edge case

			var colorSerialized = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Colors.Selected);
			return new MotionSetup(
				_inputVariant,
				horizontalPlane,
				verticalPlane,
				particle,
				SelectedEnvironmentSetting,
				colorSerialized);
		}

		public ParticleType[] ParticleTypes { get; private set; }

		public object ParticleType { get; set; }

		public VariantConfiguration VariantConfiguration { get; set; }

		public Polarity PrimaryPlanePolarity { get; set; }

		public ObservableCollection<Polarity> Polarities { get; } = new ObservableCollection<Polarity>()
		{
			Polarity.Positive,
			Polarity.Negative
		};

		public float PrimaryPlaneVoltage { get; set; }

		public float PrimaryPlaneDistance { get; set; }

		//Advanced-1, secondary options
		public Visibility AdvancedFirstOption { get => (_inputVariant == InputVariant.AdvancedVerticalHorizontalNoGravity) ? Visibility.Visible : Visibility.Collapsed; }

		public Polarity SelectedSecondaryPlaneChargePolarity { get; set; }

		public float SecondaryPlaneVoltage { get; set; }

		public float SecondaryPlaneDistance { get; set; }
		//END: Advanced-1, secondary options

		//General input values
		public Polarity ParticlePolarity { get; set; }

		public float StartVelocity { get; set; }

		public float StartVelocityDeviation { get; set; }

		public Visibility DeviationVisibility { get => (_inputVariant != InputVariant.EasyHorizontalWithGravity) ? Visibility.Visible : Visibility.Collapsed; }

		public float ChargeMultiplier { get; set; }

		public float MassMultiplier { get; set; }

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

		internal void OnParticleTypeChanged()
		{
			VariantConfiguration = VariantConfigurations.All.FirstOrDefault(
				c => c.InputVariant == _inputVariant &&
				c.ParticleType == (ParticleType?)ParticleType);
		}
	}
}
