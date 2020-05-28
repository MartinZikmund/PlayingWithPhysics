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
using Physics.InclinedPlane.Views;
using Physics.InclinedPlane.Logic.PhysicsServices;
using Physics.InclinedPlane.ValuesTable;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI;
using Windows.Foundation;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Windows.UI.Xaml.Hosting;
using Windows.ApplicationModel.Core;
using MvvmCross.Base;
using System.Collections.Generic;
using System.Linq;

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

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<MotionViewModel>(ShowValuesTableAsync);
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

        private Dictionary<MotionViewModel, AppWindow> _tableWindowIds =
            new Dictionary<MotionViewModel, AppWindow>();

        private async Task ShowValuesTableAsync(MotionViewModel viewModel)
        {
            ////if (_tableWindowIds.TryGetValue(viewModel, out var window))
            ////{
            ////    await window.TryShowAsync();
            ////    return;
            ////};
            //var newWindow = await AppWindow.TryCreateAsync();
            //var appWindowContentFrame = new Frame();
            //appWindowContentFrame.Navigate(typeof(ValuesTablePage));
            ////var physicsService = new PhysicsService(Setup);
            ////var valuesTableService = new TableService(physicsService);
            ////var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, Difficulty);
            ////valuesTableViewModel.TimeInterval = (float)(physicsService.MaxT / 30);
            ////(appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
            //// Attach the XAML content to the window.
            //ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
            //newWindow.Closed += NewWindow_Closed;
            //newWindow.Title = "Tabulka hodnot";

            ////newWindow.TitleBar.BackgroundColor = ColorHelper.ToColor(viewModel.MotionInfo.Color);
            //newWindow.TitleBar.ForegroundColor = Colors.White;
            //newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            //newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            //newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
            //newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
            //newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            //newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            //newWindow.RequestSize(new Size(600, 400));
            //var shown = await newWindow.TryShowAsync();
            //if (shown)
            //{
            //    _tableWindowIds.Add(viewModel, newWindow);
            //}
        }

        private void NewWindow_Closed(AppWindow sender, AppWindowClosedEventArgs args)
        {
            var pair = _tableWindowIds.FirstOrDefault(t => t.Value == sender);
            _tableWindowIds.Remove(pair.Key);
        }

        private void UpdateMotionAppWindow(MotionViewModel viewModel)
        {
            if (_tableWindowIds.TryGetValue(viewModel, out var appWindow))
            {
                var frame = ElementCompositionPreview.GetAppWindowContent(appWindow) as Frame;
                var page = frame.Content as ValuesTablePage;
                page?.Model.Reset(new TableService(new PhysicsService(viewModel.MotionInfo)), Difficulty);
            }
        }

        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;

        private async void NewView_HostedViewClosing(CoreApplicationView sender, HostedViewClosingEventArgs args)
        {
            await _dispatcher.ExecuteOnMainThreadAsync(() =>
            {

            });
        }

        //private async Task CloseAppViewForMotionAsync(MainViewModel viewModel)
        //{
        //    if (_tableWindowIds.TryGetValue(viewModel, out var appView))
        //    {
        //        await appView.CloseAsync();
        //        //await appView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        //        //    {
        //        //        await ApplicationView.GetForCurrentView().TryConsolidateAsync();
        //        //    });
        //    }
        //}

        //public override void ViewDisappearing()
        //{
        //    base.ViewDisappearing();
        //    CloseAppViewForMotionAsync(this);
        //}

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            //_timer.Stop();
        }
    }
}
