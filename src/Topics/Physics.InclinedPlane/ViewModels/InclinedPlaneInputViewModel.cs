using MvvmCross.ViewModels;
using Physics.InclinedPlane.Services;
using Physics.Shared.UI.Infrastructure.Topics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.InclinedPlane.ViewModels
{
    public class InclinedPlaneInputViewModel : MvxNotifyPropertyChanged, IVariantInputViewModel
    {
        public InclinedPlaneInputViewModel(DifficultyOption difficulty)
        {
            SelectedGravity = GravityDefaults[0];
            SelectedInclinedDriftCoefficient = DriftCoefficientDefaults[0];
            SelectedHorizontalDriftCoefficient= DriftCoefficientDefaults[0];
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
            new DriftCoefficientDefault("Manuální zadání", 0.0f),
            new DriftCoefficientDefault("Dřevo – dřevo (suché)", 0.4f),
            new DriftCoefficientDefault("Dřevo – dřevo (namydlené)", 0.2f),
            new DriftCoefficientDefault("Dřevo – kámen", 0.4f),
            new DriftCoefficientDefault("Ocel – achát (suché)", 0.2f),
            new DriftCoefficientDefault("Ocel – achát (olej)", 0.11f),
            new DriftCoefficientDefault("Ocel – křemen (suché)", 0.5f),
            new DriftCoefficientDefault("Kov – dubové dřevo (suché)", 0.55f),
            new DriftCoefficientDefault("Kov – dubové dřevo (mokré)", 0.25f),
            new DriftCoefficientDefault("Kov – kov (olej)", 0.05f),
            new DriftCoefficientDefault("Kov – kov (suchý)", 0.17f),
            new DriftCoefficientDefault("Kov – kov (mokrý)", 0.3f),
            new DriftCoefficientDefault("Kůže – dub", 0.32f),
            new DriftCoefficientDefault("Kůže – kov (mokrý)", 0.36f),
            new DriftCoefficientDefault("Kůže – kov (olej)", 0.15f),
            new DriftCoefficientDefault("Kůže – kov (suché)", 0.56f),
            new DriftCoefficientDefault("Pneumatika – beton (suché)", 0.7f),
            new DriftCoefficientDefault("Pneumatika – beton (mokré)", 0.25f),
            new DriftCoefficientDefault("Teflon – teflon", 0.07f),
            new DriftCoefficientDefault("Nylon – nylon", 0.25f),
            new DriftCoefficientDefault("Velmi dobře vyleštěné plochy", 0.03f),
        };

        public ObservableCollection<GravityDefault> GravityDefaults { get; } = new ObservableCollection<GravityDefault>()
        {
            new GravityDefault("Manuální zadání", 0.0f),
            new GravityDefault("Země na rovníku v úrovni mořské hladiny", 9.78f),
            new GravityDefault("Země 45° zeměpisné šířky", 9.80665f),
            new GravityDefault("Země (Zemský pól)", 9.832f),
            new GravityDefault("Země (Praha)", 9.81373f),
            new GravityDefault("Slunce (rovníkové tíhové zrychlení g)", 274f),
            new GravityDefault("Merkur (rovníkové tíhové zrychlení g)", 3.7f),
            new GravityDefault("Venuše (rovníkové tíhové zrychlení g)", 8.87f),
            new GravityDefault("Mars (rovníkové tíhové zrychlení g)", 3.71f),
            new GravityDefault("Jupiter (rovníkové tíhové zrychlení g)", 23.12f),
            new GravityDefault("Saturn (rovníkové tíhové zrychlení g)", 8.96f),
            new GravityDefault("Uran (rovníkové tíhové zrychlení g)", 8.69f),
            new GravityDefault("Neptun (rovníkové tíhové zrychlení g)", 11f)
        };

        public float Mass { get; set; } = 1;

        public float InclinedLength { get; set; } = 5;

        public float InclinedAngle { get; set; } = 45;

        public float InclinedDriftCoefficient { get; set; } = 0.5f;

        public DriftCoefficientDefault SelectedInclinedDriftCoefficient { get; set; }

        public void OnSelectedInclinedDriftCoefficientChanged()
        {
            if (SelectedInclinedDriftCoefficient != DriftCoefficientDefaults[0])
            {
                InclinedDriftCoefficient = SelectedInclinedDriftCoefficient.Value;
            }
        }

        public bool HorizontalEnabled { get; set; }

        public float HorizontalLength { get; set; } = 0;

        public float HorizontalDriftCoefficient { get; set; }

        public DriftCoefficientDefault SelectedHorizontalDriftCoefficient { get; set; }

        public void OnSelectedHorizontalDriftCoefficientChanged()
        {
            if (SelectedHorizontalDriftCoefficient != DriftCoefficientDefaults[0])
            {
                HorizontalDriftCoefficient = SelectedHorizontalDriftCoefficient.Value;
            }
        }

        public float Gravity { get; set; }

        public GravityDefault SelectedGravity { get; set; }

        public void OnSelectedGravityChanged()
        {
            if (SelectedGravity != GravityDefaults[0])
            {
                Gravity = SelectedGravity.Value;
            }
        }

        public Color Color { get; set; } = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1");

        public DifficultyOption Difficulty { get; private set; }

        public async Task<IInclinedPlaneMotionSetup> CreateMotionSetupAsync()
        {
            var isValid = await ValidateAsync();
            if (isValid)
            {
                return new InclinedPlaneMotionSetup(
                    Mass,
                    InclinedLength,
                    InclinedDriftCoefficient,
                    InclinedAngle,
                    HorizontalLength,
                    HorizontalDriftCoefficient,
                    Gravity,
                    Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex(Color));
            }
            else
            {
                return default;
            }
        }

        private Task<bool> ValidateAsync()
        {
            return Task.FromResult(true);
        }
    }
}
