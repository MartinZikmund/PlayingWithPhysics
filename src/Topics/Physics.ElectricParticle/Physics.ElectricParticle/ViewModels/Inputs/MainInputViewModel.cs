using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
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
			SecondaryPlanePolarity = Polarities[0];
			SelectedEnvironmentSetting = EnvironmentSettings[0];
		}

		public MainInputViewModel(InputVariant variant, ElectricParticleSimulationSetup setup) : this(variant)
		{
			Colors.Selected = ColorHelper.ToColor(setup.Color);
			ParticleType = setup.Particle.Type;
			ChargeMultiplier = setup.Particle.ChargeMultiplier;
			MassMultiplier = setup.Particle.MassMultiplier;
			StartVelocity = setup.Particle.StartVelocity;
			StartVelocityDeviation = setup.Particle.StartVelocityDeviation;

			if (Enum.IsDefined(typeof(VelocityDirection), (int)StartVelocityDeviation))
			{
				SelectedVelocityDirection = (VelocityDirection)(int)StartVelocityDeviation;
			}

			if (_inputVariant == InputVariant.EasyHorizontalNoGravity ||
				_inputVariant == InputVariant.EasyHorizontalWithGravity)
			{
				PrimaryPlanePolarity = setup.HorizontalPlane.Polarity;
				PrimaryPlaneVoltage = setup.HorizontalPlane.Voltage;
				PrimaryPlaneDistance = setup.HorizontalPlane.Distance;
			}

			if (_inputVariant == InputVariant.EasyVerticalNoGravity ||
				_inputVariant == InputVariant.AdvancedVerticalWithGravity ||
				_inputVariant == InputVariant.AdvancedVerticalHorizontalNoGravity)
			{
				PrimaryPlanePolarity = setup.VerticalPlane.Polarity;
				PrimaryPlaneVoltage = setup.VerticalPlane.Voltage;
				PrimaryPlaneDistance = setup.VerticalPlane.Distance;
			}

			if (_inputVariant == InputVariant.AdvancedVerticalHorizontalNoGravity)
			{
				SecondaryPlaneDistance = setup.HorizontalPlane.Distance;
				SecondaryPlanePolarity = setup.HorizontalPlane.Polarity;
				SecondaryPlaneVoltage = setup.HorizontalPlane.Voltage;
			}
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

		public override async Task<ElectricParticleSimulationSetup> CreateMotionSetupAsync()
		{
			PlaneSetup horizontalPlane = null;
			if (_inputVariant == InputVariant.EasyHorizontalNoGravity ||
				_inputVariant == InputVariant.EasyHorizontalWithGravity)
			{
				horizontalPlane = new PlaneSetup(PrimaryPlanePolarity, PrimaryPlaneVoltage, PrimaryPlaneDistance);
			}

			PlaneSetup verticalPlane = null;
			if (_inputVariant == InputVariant.EasyVerticalNoGravity ||
				_inputVariant == InputVariant.AdvancedVerticalWithGravity ||
				_inputVariant == InputVariant.AdvancedVerticalHorizontalNoGravity)
			{
				verticalPlane = new PlaneSetup(PrimaryPlanePolarity, PrimaryPlaneVoltage, PrimaryPlaneDistance);
			}

			if (_inputVariant == InputVariant.AdvancedVerticalHorizontalNoGravity)
			{
				horizontalPlane = new PlaneSetup(SecondaryPlanePolarity, SecondaryPlaneVoltage, SecondaryPlaneDistance);
			}

			var chargeMultiplier = ChargeMultiplier;
			var massMultiplier = MassMultiplier;

			var particlePolarity = ParticlePolarity;
			if ((ParticleType)ParticleType == Logic.ParticleType.Electron)
			{
				chargeMultiplier = 1;
				massMultiplier = 1;
				particlePolarity = Polarity.Negative;
			}
			else if ((ParticleType)ParticleType == Logic.ParticleType.AtomNucleus)
			{
				particlePolarity = Polarity.Positive;
			}

			var velocityDeviation = StartVelocityDeviation;
			if (_inputVariant == InputVariant.EasyHorizontalWithGravity)
			{
				velocityDeviation = (int)SelectedVelocityDirection;
			}

			// TODO: handle remaining cases

			var particle = new ParticleSetup(
				(ParticleType)ParticleType,
				particlePolarity,
				chargeMultiplier,
				massMultiplier,
				StartVelocity,
				velocityDeviation);

			var colorSerialized = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Colors.Selected);
			return new ElectricParticleSimulationSetup(
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

		public float PrimaryPlaneVoltage { get; set; } = 1000;

		public float PrimaryPlaneDistance { get; set; } = 0.2f;

		//Advanced-1, secondary options
		public Visibility AdvancedFirstOption { get => (_inputVariant == InputVariant.AdvancedVerticalHorizontalNoGravity) ? Visibility.Visible : Visibility.Collapsed; }

		public Polarity SecondaryPlanePolarity { get; set; }

		public float SecondaryPlaneVoltage { get; set; } = 1000;

		public float SecondaryPlaneDistance { get; set; } = 0.2f;
		//END: Advanced-1, secondary options

		//General input values
		public Polarity ParticlePolarity { get; set; }

		public float StartVelocity { get; set; } = 500;

		public float StartVelocityDeviation { get; set; } = 0;

		public Visibility DeviationVisibility { get => (_inputVariant != InputVariant.EasyHorizontalWithGravity) ? Visibility.Visible : Visibility.Collapsed; }

		public float ChargeMultiplier { get; set; } = 1;

		public float MassMultiplier { get; set; } = 1;

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
