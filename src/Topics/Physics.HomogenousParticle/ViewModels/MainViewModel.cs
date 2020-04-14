using Physics.HomogenousParticle.Services;
using Physics.HomogenousParticle.ViewModels.Inputs;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Physics.HomogenousParticle.ViewModels
{
    public class MainViewModel : ViewModelBase<MainViewModel.NavigationModel>
    {
        private float v;
        public class NavigationModel
        {
        }

        public MainViewModel()
        {
            OnSelectedVariantIndexChanged();
        }

        public override void Prepare(NavigationModel parameter)
        {
        }
        public int SelectedVariantIndex { get; set; }

        public bool IsQEnabled { get; set; }
        public bool IsBEnabled { get; set; }
        public bool IsVEnabled { get; set; }

        public void OnSelectedVariantIndexChanged()
        {
            VelocityVariant variant = (VelocityVariant)SelectedVariantIndex;
            switch (variant)
            {
                case VelocityVariant.Zero:
                    VariantInputViewModel = new ZeroVariantInputViewModel();
                    break;
                case VelocityVariant.Parallel:
                    break;
                case VelocityVariant.Perpendicular:
                    break;
                case VelocityVariant.Greek:
                    break;
            }
        }

        public IVariantInputViewModel VariantInputViewModel { get; set; }

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
            DrawingContent = "Drawing...";
        }

        public string DrawingContent { get; set; }

        public Visibility IsSecondStepVisible { get; set; } = Visibility.Visible;

        public float Q { get; set; }
        public float Orientation { get; set; }
        public float Velocity { get; set; }
        public float Angle { get; set; }
    }
}
