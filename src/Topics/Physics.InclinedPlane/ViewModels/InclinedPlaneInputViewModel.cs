using MvvmCross.ViewModels;
using Physics.InclinedPlane.Services;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.InclinedPlane.ViewModels
{
	public class InclinedPlaneInputViewModel : MvxNotifyPropertyChanged, IVariantInputViewModel
	{
		public InclinedPlaneInputViewModel(DifficultyOption difficulty)
		{
			SelectedGravity = GravityDefaults[1];
			SelectedInclinedDriftCoefficient = DriftCoefficientDefaults[0];
			SelectedHorizontalDriftCoefficient = DriftCoefficientDefaults[0];
			Difficulty = difficulty;
		}

		public bool IsAdvanced => Difficulty == DifficultyOption.Advanced;

		public Color[] AvailableColors { get; } = new Color[]
		{
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#2D7D9A"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#E81123"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#881798"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#498205"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#515C6B"),
		};

		public ObservableCollection<DriftCoefficientDefault> DriftCoefficientDefaults { get; } = new ObservableCollection<DriftCoefficientDefault>()
		{
			new DriftCoefficientDefault(Localizer.Instance["ManualInput"], null),
			new DriftCoefficientDefault(Localizer.Instance["Drift_WoodWoodDry"], 0.4f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_WoodWoodSoaped"], 0.2f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_WoodStone"], 0.4f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_WoodSnow"], 0.035f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SteelAgateDry"], 0.2f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SteelAgateOiled"], 0.11f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SteelSilicaDry"], 0.5f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SteelIce"], 0.027f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_MetalOakDry"], 0.55f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_MetalOakWet"], 0.25f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_MetalMetalDry"], 0.3f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_MetalMetalWet"], 0.17f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_MetalMetalOiled"], 0.05f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SkinOak"], 0.32f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SkinMetalDry"], 0.56f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SkinMetalWet"], 0.36f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_SkinMetalOiled"], 0.15f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_TireConcreteDry"], 0.7f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_TireConcreteWet"], 0.25f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_TeflonTeflon"], 0.07f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_NylonNylon"], 0.25f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_VeryWellPolishedSurfaces"], 0.03f),
			new DriftCoefficientDefault(Localizer.Instance["Drift_StoneIce"], 0.03f)

		};

		public ObservableCollection<GravityDefault> GravityDefaults { get; } = new ObservableCollection<GravityDefault>()
		{
			new GravityDefault(Localizer.Instance["ManualInput"], null),
			new GravityDefault(Localizer.Instance["Gravity_EarthEquatorSeaLevel"], 9.78f),
			new GravityDefault(Localizer.Instance["Gravity_Earth45"], 9.80665f),
			new GravityDefault(Localizer.Instance["Gravity_EarthPole"], 9.832f),
			new GravityDefault(Localizer.Instance["Gravity_EarthPrague"], 9.81373f),
			new GravityDefault(Localizer.Instance["Gravity_Sun"], 274f),
			new GravityDefault(Localizer.Instance["Gravity_Mercury"], 3.7f),
			new GravityDefault(Localizer.Instance["Gravity_Venus"], 8.87f),
			new GravityDefault(Localizer.Instance["Gravity_Mars"], 3.71f),
			new GravityDefault(Localizer.Instance["Gravity_Jupiter"], 23.12f),
			new GravityDefault(Localizer.Instance["Gravity_Saturn"], 8.96f),
			new GravityDefault(Localizer.Instance["Gravity_Uranus"], 8.69f),
			new GravityDefault(Localizer.Instance["Gravity_Neptune"], 11f),
			new GravityDefault(Localizer.Instance["Gravity_Moon"], 1.62f)
		};

		public float Mass { get; set; } = 1;

		public float V0 { get; set; } = 5;

		public float InclinedLength { get; set; } = 5;

		public float InclinedAngle { get; set; } = 45;

		public float InclinedDriftCoefficient { get; set; } = 0.5f;

		public void OnInclinedDriftCoefficientChanged()
		{
			var selectedValue = SelectedInclinedDriftCoefficient?.Value ?? -1;
			if (Math.Abs(InclinedDriftCoefficient - selectedValue) > 0.01)
			{
				SelectedInclinedDriftCoefficient = DriftCoefficientDefaults[0];
			}
		}

		public DriftCoefficientDefault SelectedInclinedDriftCoefficient { get; set; }

		public void OnSelectedInclinedDriftCoefficientChanged()
		{
			if (SelectedInclinedDriftCoefficient != DriftCoefficientDefaults[0])
			{
				InclinedDriftCoefficient = SelectedInclinedDriftCoefficient?.Value ?? 0.5f;
			}
		}

		public int MaximumAngle => HorizontalEnabled ? 60 : 90;

        public bool HorizontalEnabled { get; set; }

        public float HorizontalLength { get; set; } = 10;

        public float HorizontalDriftCoefficient { get; set; } = 0.5f;

        public void OnHorizontalDriftCoefficientChanged()
        {
            var selectedValue = SelectedHorizontalDriftCoefficient?.Value ?? -1;
            if (Math.Abs(HorizontalDriftCoefficient - selectedValue) > 0.01)
            {
                SelectedHorizontalDriftCoefficient = DriftCoefficientDefaults[0];
            }
        }

        public DriftCoefficientDefault SelectedHorizontalDriftCoefficient { get; set; }

        public void OnSelectedHorizontalDriftCoefficientChanged()
        {
            if (SelectedHorizontalDriftCoefficient != DriftCoefficientDefaults[0])
            {
                HorizontalDriftCoefficient = SelectedHorizontalDriftCoefficient?.Value ?? 0.5f;
            }
        }

        public float Gravity { get; set; } = 9.81f;

        public GravityDefault SelectedGravity { get; set; }

        public void OnSelectedGravityChanged()
        {
            if (SelectedGravity != GravityDefaults[0])
            {
                Gravity = SelectedGravity?.Value ?? 9.81f;
            }
        }

        public Color Color { get; set; } = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1");

        public DifficultyOption Difficulty { get; private set; }

        public async Task<IInclinedPlaneMotionSetup> CreateMotionSetupAsync()
        {
            var isValid = await ValidateAsync();
            if (isValid)
            {
                if (HorizontalEnabled)
                {
                    return new InclinedPlaneMotionSetup(
                        Mass,
                        V0,
                        Gravity,
                        InclinedLength,
                        InclinedDriftCoefficient,
                        InclinedAngle,
                        HorizontalLength,
                        HorizontalDriftCoefficient,
                        Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
                }
                else
                {
                    return new InclinedPlaneMotionSetup(
                        Mass,
                        V0,
                        Gravity,
                        InclinedLength,
                        InclinedDriftCoefficient,
                        InclinedAngle,
                        0,
                        0,
                        Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
                }
            }
            else
            {
                return default;
            }
        }

        private Task<bool> ValidateAsync()
        {
            StringBuilder validationErrors = new StringBuilder();
            if (HorizontalEnabled)
            {
                if (HorizontalDriftCoefficient <= 0)
                {
                    validationErrors.Append(Localizer.Instance["Validation_HorizontalDriftCoefficientMustBePositive"]);
                }
            }
            return Task.FromResult(true);
        }
    }
}
