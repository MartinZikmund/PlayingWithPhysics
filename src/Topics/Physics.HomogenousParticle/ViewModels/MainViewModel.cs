using Physics.HomogenousParticle.Dialogs;
using Physics.HomogenousParticle.Services;
using Physics.HomogenousParticle.ViewInteractions;
using Physics.HomogenousParticle.ViewModels.Inputs;
using Physics.HomogenousParticle.Views;
using Physics.Shared.UI.ViewModels;
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
    public class MainViewModel : SimulationViewModelBase<MainViewModel.NavigationModel>
    {
        private IMainViewInteraction _interaction;

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
                    VariantInputViewModel = new RadiationVariantInputViewModel();
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
                if (SelectedVariant == VelocityVariant.Zero || 
                    SelectedVariant == VelocityVariant.Parallel || 
                    SelectedVariant == VelocityVariant.Perpendicular)
                {
                    Motions.Clear();
                }
                Motions.Add(dialog.Setup);
                RestartSimulation();
            }
        });

        internal void SetViewInteraction(IMainViewInteraction interaction)
        {
            _interaction = interaction;
        }

        public ObservableCollection<IMotionSetup> Motions { get; } = new ObservableCollection<IMotionSetup>();

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
        }

        public string DrawingContent { get; set; }

        public Visibility IsSecondStepVisible { get; set; } = Visibility.Visible;

        private void RestartSimulation()
        {
            if (_interaction == null) return;
            var controller = _interaction.PrepareController(SelectedVariant);
            SimulationPlayback.SetController(controller);
            controller.StartSimulation(Motions.ToArray());
        }
    }
}
