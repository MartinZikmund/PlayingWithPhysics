using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using Physics.InclinedPlane.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.InclinedPlane.UserControls;
using Physics.Shared.UI.Infrastructure.Topics;
using System.Collections.ObjectModel;
using Physics.InclinedPlane.Services;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;

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
                Setup = dialog.Setup;
                Motion = new MotionViewModel(Setup);
                //RestartSimulation();
            }
        });

        public IMotionSetup Setup { get; set; }

        public MotionViewModel Motion {get; set;}

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

        //private async Task ShowValuesTableAsync(MotionViewModel viewModel)
        ////{
        ////    if (_tableWindowIds.TryGetValue(viewModel, out var window))
        ////    {
        ////        await window.TryShowAsync();
        ////        return;
        ////    };
        //    var newWindow = await AppWindow.TryCreateAsync();
        //    var appWindowContentFrame = new Frame();
        //    appWindowContentFrame.Navigate(typeof(ValuesTablePage));
        //    var physicsService = new PhysicsService(viewModel.MotionInfo);
        //    var valuesTableService = new TableService(physicsService);
        //    var movementType = viewModel.MotionInfo.Type;
        //    var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, movementType);
        //    valuesTableViewModel.TimeInterval = (float)(physicsService.MaxT / 30);
        //    (appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
        //    // Attach the XAML content to the window.
        //    ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
        //    newWindow.Closed += NewWindow_Closed;
        //    newWindow.Title = viewModel.Label;

        //    newWindow.TitleBar.BackgroundColor = ColorHelper.ToColor(viewModel.MotionInfo.Color);
        //    newWindow.TitleBar.ForegroundColor = Colors.White;
        //    newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
        //    newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
        //    newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
        //    newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
        //    newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
        //    newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
        //    newWindow.RequestSize(new Size(600, 400));
        //    var shown = await newWindow.TryShowAsync();
        //    if (shown)
        //    {
        //        _tableWindowIds.Add(viewModel, newWindow);
        //    }
        //}
    }
}
