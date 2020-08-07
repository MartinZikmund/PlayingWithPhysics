using Microsoft.Toolkit.Uwp.Helpers;
using MvvmCross.Base;
using Physics.DragMovement.Dialogs;
using Physics.DragMovement.Logic.PhysicsServices;
using Physics.DragMovement.Models;
using Physics.DragMovement.Rendering;
using Physics.DragMovement.ValuesTable;
using Physics.DragMovement.Views;
using Physics.Shared.Services.Preferences;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.DragMovement.ViewModels
{
    public abstract class DragMovementSimulationViewModelBase : SimulationViewModelBase<SimulationNavigationModel>
    {
        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;
        private readonly IPreferences _preferences;
        protected bool _startWithController = false;
        protected DragMovementCanvasController _controller;

        //private LaunchInfo _launchInfo = null;
        private DispatcherTimer _timer = new DispatcherTimer();

        public DragMovementSimulationViewModelBase(IMvxMainThreadAsyncDispatcher dispatcher, IPreferences preferences)
        {
            _dispatcher = dispatcher;
            _preferences = preferences;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timer.Tick += _timer_Tick;
        }

        public virtual bool PauseAfterChanges
        {
            get
            {
                return _preferences.GetSetting(nameof(PauseAfterChanges), false, PreferenceLocality.Local);
            }
            set
            {
                _preferences.SetSetting(nameof(PauseAfterChanges), value, PreferenceLocality.Local);
            }
        }

        public override void Prepare(SimulationNavigationModel parameter)
        {
            Difficulty = parameter.Difficulty;
            RaisePropertyChanged(nameof(IsAdvanced));
        }

        public DifficultyOption Difficulty { get; set; }

        public bool IsAdvanced => Difficulty == DifficultyOption.Advanced;

        public float StepSize { get; set; } = 0.1f;

        public bool DrawTrajectoriesContinously { get; set; } = true;

        private void _timer_Tick(object sender, object e)
        {
            //if (_timer.IsEnabled && _controller != null)
            //{
            //    float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;

            //    if (_controller.TrajectoryStopTime != null && _controller.SimulationTime.TotalTime > _controller.TrajectoryStopTime)
            //    {
            //        timeElapsed = (float)_controller.TrajectoryStopTime.Value.TotalSeconds;
            //    }

            //    foreach (var motion in Motions)
            //    {
            //        motion.UpdateCurrentValues(timeElapsed);
            //    }
            //}
        }

        public ObservableCollection<MotionInfoViewModel> Motions { get; } =
            new ObservableCollection<MotionInfoViewModel>();

        public bool AddTrajectoryButtonEnabled { get; set; } = true;

        public bool BreakDownMotions { get; set; } = false;

        public async void OnBreakDownMotionsChanged()
        {
            if (Motions.Count > 0)
            {
                await StartSimulationAsync();
            }
        }

        public bool IsPaused { get; set; }

        public ICommand PauseToggleCommand => GetOrCreateCommand(PauseToggle);

        private void PauseToggle()
        {
            //IsPaused = !IsPaused;
            //if (IsPaused)
            //{
            //    _controller.Pause();
            //}
            //else
            //{
            //    _controller.Play();
            //}
        }

        public ICommand StartNewSimulationCommand => GetOrCreateAsyncCommand(StartSimulationAsync);

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(AddTrajectoryAsync);

        public ICommand EditTrajectoryCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(EditTrajectoryAsync);

        public ICommand DeleteTrajectoryCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(DeleteTrajectoryAsync);

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(ShowValuesTableAsync);

        public ICommand DuplicateTrajectoryCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(DuplicateTrajectoryAsync);

        private async Task DeleteTrajectoryAsync(MotionInfoViewModel arg)
        {
            Motions.Remove(arg);
            if (Motions.Count < 5)
            {
                AddTrajectoryButtonEnabled = true;
            }
            await CloseAppViewForMotionAsync(arg);
            await StartSimulationAsync();
        }

        private async Task AddTrajectoryAsync()
        {
            if (Motions.Count >= 5)
            {
                await new MessageDialog("Není možné vykreslovat více než 5 pohybů najednou.", "Vytvoření pohybu se nezdařilo.").ShowAsync();
                return;
            }
            var dialogViewModel = new AddOrUpdateMotionViewModel(Difficulty, Motions.Select(m => m.Label).ToArray());
            var dialog = new AddOrUpdateMovement(dialogViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Motions.Add(new MotionInfoViewModel(dialogViewModel.ResultMotionInfo));
                await StartSimulationAsync();
            }
        }

        private async Task EditTrajectoryAsync(MotionInfoViewModel arg)
        {
            var dialogViewModel = new AddOrUpdateMotionViewModel(arg.MotionInfo, Difficulty, Motions.Select(m => m.Label).ToArray());
            var dialog = new AddOrUpdateMovement(dialogViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                arg.MotionInfo = dialogViewModel.ResultMotionInfo;
                await StartSimulationAsync();
                UpdateMotionAppWindow(arg);
            }
        }

        private async Task DuplicateTrajectoryAsync(MotionInfoViewModel arg)
        {
            if (Motions.Count >= 5)
            {
                await new MessageDialog("Není možné vykreslovat více než 5 pohybů najednou.", "Zkopírování se nezdařilo.").ShowAsync();
                return;
            }
            var duplicateMotion = arg.MotionInfo.Clone();
            duplicateMotion.Label =
                $"{duplicateMotion.Label} ({ResourceLoader.GetForCurrentView().GetString("Copy")})";
            var dialogViewModel = new AddOrUpdateMotionViewModel(duplicateMotion, Difficulty, Motions.Select(m => m.Label).ToArray());
            var dialog = new AddOrUpdateMovement(dialogViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Motions.Add(new MotionInfoViewModel(dialogViewModel.ResultMotionInfo));
                await StartSimulationAsync();
            }
        }

        protected async Task StartSimulationAsync()
        {
            if (_controller == null)
            {
                _startWithController = true;
                return;
            }

            _timer.Start();
            if (PauseAfterChanges)
            {
                IsPaused = true;
                _controller.Pause();
            }
            await _controller.RunOnGameLoopAsync(() =>
            {
                _controller.StartNewSimulation(DrawTrajectoriesContinously, BreakDownMotions, Motions.Select(t => t.MotionInfo).ToArray());
            });
        }

        private Dictionary<MotionInfoViewModel, AppWindow> _tableWindowIds =
            new Dictionary<MotionInfoViewModel, AppWindow>();

        private async Task ShowValuesTableAsync(MotionInfoViewModel viewModel)
        {
            if (_tableWindowIds.TryGetValue(viewModel, out var window))
            {
                await window.TryShowAsync();
                return;
            };
            var newWindow = await AppWindow.TryCreateAsync();
            var appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(ValuesTablePage));
            var physicsService = new PhysicsService(viewModel.MotionInfo);
            var valuesTableService = new TableService(physicsService);
            var movementType = viewModel.MotionInfo.Type;
            var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, movementType);
            valuesTableViewModel.TimeInterval = (float)(physicsService.MaxT / 30);
            (appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
            // Attach the XAML content to the window.
            ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
            newWindow.Closed += NewWindow_Closed;
            newWindow.Title = viewModel.Label;

            newWindow.TitleBar.BackgroundColor = ColorHelper.ToColor(viewModel.MotionInfo.Color);
            newWindow.TitleBar.ForegroundColor = Colors.White;
            newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.RequestSize(new Size(600, 400));
            var shown = await newWindow.TryShowAsync();
            if (shown)
            {
                _tableWindowIds.Add(viewModel, newWindow);
            }
        }

        private void NewWindow_Closed(AppWindow sender, AppWindowClosedEventArgs args)
        {
            var pair = _tableWindowIds.FirstOrDefault(t => t.Value == sender);
            _tableWindowIds.Remove(pair.Key);
        }

        private void UpdateMotionAppWindow(MotionInfoViewModel viewModel)
        {
            if (_tableWindowIds.TryGetValue(viewModel, out var appWindow))
            {
                var frame = ElementCompositionPreview.GetAppWindowContent(appWindow) as Frame;
                var page = frame.Content as ValuesTablePage;
                page?.Model.Reset(new TableService(new PhysicsService(viewModel.MotionInfo)), viewModel.MotionInfo.Type);
            }
        }

        private async void NewView_HostedViewClosing(CoreApplicationView sender, HostedViewClosingEventArgs args)
        {
            await _dispatcher.ExecuteOnMainThreadAsync(() =>
            {

            });
        }

        private async Task CloseAppViewForMotionAsync(MotionInfoViewModel viewModel)
        {
            if (_tableWindowIds.TryGetValue(viewModel, out var appView))
            {
                await appView.CloseAsync();
                //await appView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                //    {
                //        await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                //    });
            }
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();
            foreach (var motion in Motions)
            {
                CloseAppViewForMotionAsync(motion);
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            _timer.Stop();
        }
    }
}
