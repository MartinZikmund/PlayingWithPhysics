using Physics.InclinedPlane.Services;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Physics.InclinedPlane.ViewModels
{
    public abstract class VariantInputViewModelBase : ViewModelBase, IVariantInputViewModel
    {
        private Color _color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1");

        public Color[] AvailableColors { get; } = new Color[]
        {
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#2D7D9A"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#E81123"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#881798"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#498205"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#515C6B"),
        };

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                //RaisePropertyChanged();
            }
        }
        public float Elevation { get; set; }
        public float Angle { get; set; }
        public float Mass { get; set; }
        public float Length => (Elevation / (float)Math.Sin(Angle));
        public abstract string Label { get; set; }
        public abstract Task<IMotionSetup> CreateMotionSetupAsync();
        public void OnElevationChanged()
        {
            float maxElevtion = Length * 2;
            if (Elevation > maxElevtion)
            {
                Elevation = maxElevtion;
                new MessageDialog($"Výška v nakloněné může být maximálně dvojnásobkem délky d nakloněné roviny. Vámi zadaná hodnota bude nastavena na: {maxElevtion}.");
            }
        }

        public Visibility CustomDriftCoefficientInputVisibility { get; set; } = Visibility.Collapsed;

        private DriftCoefficientDefault _selectedDriftCoefficient;

        public VariantInputViewModelBase()
        {
            SelectedDriftCoefficient = DriftCoefficientDefaults[0];
        }

        public DriftCoefficientDefault SelectedDriftCoefficient
        {
            get
            {
                return _selectedDriftCoefficient;
            }
            set
            {
                _selectedDriftCoefficient = value;
                DriftCoefficient = _selectedDriftCoefficient.Value;
            }
        }
        public ObservableCollection<DriftCoefficientDefault> DriftCoefficientDefaults { get; } = new ObservableCollection<DriftCoefficientDefault>()
        {
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

        //public float MaxInclinedPlaneElevation => (Length * 2);

        public float DriftCoefficient { get; set; }
    }
}
