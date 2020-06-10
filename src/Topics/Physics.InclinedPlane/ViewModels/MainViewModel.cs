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
using Windows.UI.Xaml;
using Physics.InclinedPlane.ViewInteractions;

namespace Physics.InclinedPlane.ViewModels
{
    public class MainViewModel : SimulationViewModelBase<MainViewModel.NavigationModel>
    {
        private IMainViewInteraction _interaction;

        private DifficultyOption Difficulty;

        public class NavigationModel
        {
            public DifficultyOption Difficulty { get; set; }
        }

        public MainViewModel()
        {
            
        }

        public override void Prepare(NavigationModel parameter)
        {
            Difficulty = parameter.Difficulty;
        }

        internal void SetViewInteraction(IMainViewInteraction interaction)
        {
            _interaction = interaction;
        }

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(async () =>
        {
            var dialog = new AddOrUpdateMovement(new InclinedPlaneInputViewModel(Difficulty));
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Setup = dialog.Setup;
                Motion = new MotionViewModel(Setup);
                RestartSimulation();
            }
        });

        private void RestartSimulation()
        {
            if (_interaction == null) return;
            var controller = _interaction.PrepareController();
            SimulationPlayback.SetController(controller);
            controller.StartSimulation(Motion.MotionInfo);
        }

        public IInclinedPlaneMotionSetup Setup { get; set; }

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
            var newWindow = await AppWindow.TryCreateAsync();
            var appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(ValuesTablePage));
            var physicsService = new PhysicsService(Setup);
            var valuesTableService = new TableService(physicsService);
            var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, Difficulty);
            (appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
            // Attach the XAML content to the window.
            ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
            newWindow.Title = "Table";

            newWindow.TitleBar.BackgroundColor = (Color)Application.Current.Resources["AppThemeColor"];
            newWindow.TitleBar.ForegroundColor = Colors.White;
            newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.RequestSize(new Size(600, 400));
            var shown = await newWindow.TryShowAsync();
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
