using Physics.HomogenousParticle.Dialogs;
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
using Windows.UI.Xaml.Controls;

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
            VelocityVariant variant = (VelocityVariant)SelectedVariantIndex;
            switch (variant)
            {
                case VelocityVariant.Zero:
                    VariantInputViewModel = new ZeroVariantInputViewModel();
                    break;
                case VelocityVariant.Parallel:
                    VariantInputViewModel = new ParallelVariantInputViewModel();
                    break;
                case VelocityVariant.Perpendicular:
                    VariantInputViewModel = new PerpendicularVariantInputViewModel();
                    break;
                case VelocityVariant.Greek:
                    VariantInputViewModel = new GreekVariantInputViewModel();
                    break;
            }
        }

        public IVariantInputViewModel VariantInputViewModel { get; set; }

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(async () =>
        {
            var dialog = new AddOrUpdateMotionDialog(VariantInputViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                DrawingContent = dialog.Setup.ToString();
            }
        });

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
        }

        public string DrawingContent { get; set; }

        public Visibility IsSecondStepVisible { get; set; } = Visibility.Visible;

        public float Q { get; set; }
        public float Orientation { get; set; }
        public float Velocity { get; set; }
        public float Angle { get; set; }
    }
}
