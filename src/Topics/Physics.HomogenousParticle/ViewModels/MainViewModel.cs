using Physics.HomogenousParticle.Models;
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
            DisableInputs();
        }

        private void DisableInputs()
        {
            switch ((VelocityVariant)SelectedVariantIndex)
            {
                case VelocityVariant.Zero:
                    IsQEnabled = true;
                    IsBEnabled = true;
                    IsVEnabled = false;
                    break;
                case VelocityVariant.Parallel:
                case VelocityVariant.Perpendicular:
                    IsQEnabled = true;
                    IsBEnabled = true;
                    IsVEnabled = true;
                    break;
            }
        }

        public float QInput { get; set; }
        public float OrientationInput { get; set; }
        public float VelocityInput { get; set; }

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
            DrawingContent = "Drawing...";
        }

        public string DrawingContent { get; set; }

        public Visibility IsSecondStepVisible { get; set; } = Visibility.Visible;
    }
}
