using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Physics.Shared.ViewModels;
using System.Numerics;
using System.Resources;
using System.ServiceModel.Channels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Physics.Shared.Logic.Constants;
using Windows.ApplicationModel.DataTransfer;
using System.Collections.ObjectModel;
using Physics.InclinedPlane.Dialogs;

namespace Physics.InclinedPlane.ViewModels
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

        //public IVariantInputViewModel VariantInputViewModel { get; set; }

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(async () =>
        {
            var dialog = new AddOrUpdateMovement();
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
