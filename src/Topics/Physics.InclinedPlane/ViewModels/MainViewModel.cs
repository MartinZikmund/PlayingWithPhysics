using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using Physics.InclinedPlane.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.InclinedPlane.UserControls;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.InclinedPlane.ViewModels
{
    public class MainViewModel : SimulationViewModelBase<MainViewModel.NavigationModel>
    {
        private DifficultyOption Difficulty;
        public class NavigationModel
        {
            public DifficultyOption Difficulty { get; set; }
        }

        public MainViewModel()
        {
            OnSelectedVariantIndexChanged();
        }

        public override void Prepare(NavigationModel parameter)
        {
            Difficulty = parameter.Difficulty;
        }

        public int SelectedVariantIndex { get; set; }

        //public VelocityVariant SelectedVariant => (VelocityVariant)SelectedVariantIndex;

        public void OnSelectedVariantIndexChanged() { }
        //{
        //    switch (SelectedVariant)
        //    {
        //        case VelocityVariant.Zero:
        //            VariantInputViewModel = new ZeroVariantInputViewModel();
        //            break;
        //        case VelocityVariant.Parallel:
        //            VariantInputViewModel = new ParallelVariantInputViewModel();
        //            break;
        //        case VelocityVariant.Perpendicular:
        //            VariantInputViewModel = new PerpendicularVariantInputViewModel();
        //            break;
        //        case VelocityVariant.Greek:
        //            VariantInputViewModel = new GreekVariantInputViewModel();
        //            break;
        //    }
        //}

        public IVariantInputViewModel VariantInputViewModel
        {
            get
            {
                
                if (Difficulty == DifficultyOption.Easy)
                {
                    return new BasicVariantInputViewModel();
                }
                else
                {
                    return new AdvancedVariantInputViewModel();
                }
            }
        }

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(async () =>
        {
            var dialog = new AddOrUpdateMovement(VariantInputViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                //if (SelectedVariant == VelocityVariant.Zero || SelectedVariant == VelocityVariant.Parallel)
                //{
                //    Motions.Clear();
                //}
                //Motions.Add(dialog.Setup);
                //RestartSimulation();
            }
        });

        //public ObservableCollection<IMotionSetup> Motions { get; } = new ObservableCollection<IMotionSetup>();

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
        }

        public string DrawingContent { get; set; }

        public ICommand ShareCommand => GetOrCreateCommand(DataTransferManager.ShowShareUI);

        public float StepSize { get; set; } = 0.1f;

        public bool IsPaused { get; set; } = true;

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
    }
}
