using Physics.HomogenousParticle.Dialogs;
using Physics.HomogenousParticle.Services;
using Physics.HomogenousParticle.ViewModels.Inputs;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
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

        public VelocityVariant SelectedVariant => (VelocityVariant)SelectedVariantIndex;

        public void OnSelectedVariantIndexChanged()
        {
            switch (SelectedVariant)
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
                if (SelectedVariant == VelocityVariant.Zero || SelectedVariant == VelocityVariant.Parallel)
                {
                    Motions.Clear();
                }
                Motions.Add(dialog.Setup);
                RestartSimulation();
            }
        });

        public ObservableCollection<IMotionSetup> Motions { get; }

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
        }

        public string DrawingContent { get; set; }

        public Visibility IsSecondStepVisible { get; set; } = Visibility.Visible;

        public ICommand ShareCommand => GetOrCreateCommand(DataTransferManager.ShowShareUI);

        public float StepSize { get; set; } = 0.1f;

        public bool IsPaused { get; set; }

        public ICommand PauseToggleCommand => GetOrCreateCommand(PauseToggle);

        private void PauseToggle()
        {
            throw new NotImplementedException();
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                //_controller.Pause();
            }
            else
            {
                //_controller.Play();
            }
        }

        private void RestartSimulation()
        {

        }
    }
}
