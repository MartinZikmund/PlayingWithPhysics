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
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using MvvmCross.Base;
using Physics.HomogenousMovement.Dialogs;
using Physics.HomogenousMovement.Models;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.HomogenousMovement.Rendering;
using Physics.HomogenousMovement.ViewInteractions;
using Physics.HomogenousMovement.Views;
using Physics.Shared.ViewModels;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Physics.Shared.Services.Preferences;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels;

namespace Physics.HomogenousMovement.ViewModels
{
    public abstract class ProjectileMotionSimulationViewModelBase : SimulationViewModelBase<SimulationNavigationModel>
    {
        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;
        private readonly IPreferences _preferences;
        protected bool _startWithController = false;
        protected HomogenousMovementCanvasController _controller;

        private LaunchInfo _launchInfo = null;
        private DispatcherTimer _timer = new DispatcherTimer();

        public ProjectileMotionSimulationViewModelBase(IMvxMainThreadAsyncDispatcher dispatcher, IPreferences preferences)
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

        private void MainView_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var launchInfo = new LaunchInfo();
            launchInfo.Motions = Motions.Select(t => t.MotionInfo).ToList();
            args.Request.Data.SetWebLink(LaunchUriManager.Serialize("hsf1", launchInfo));
            args.Request.Data.Properties.Title = "Share this motion's uri";
            args.Request.Data.Properties.Description = "This motion and it's settings will be shared as a URI which let's other users make use of it.";
        }

        public override void Prepare(SimulationNavigationModel parameter)
        {
            Difficulty = parameter.Difficulty;
            RaisePropertyChanged(nameof(IsAdvanced));
            _launchInfo = parameter.LaunchInfo;
        }

        public override async Task Initialize()
        {
            if (_launchInfo != null)
            {
                await LoadLaunchInfoAsync(_launchInfo);
            }
        }

        public async Task LoadLaunchInfoAsync(LaunchInfo launchInfo)
        {
            if (launchInfo?.Motions != null && launchInfo.Motions.Count > 0)
            {
                Motions.Clear();

                for (int movementId = 0; movementId < launchInfo.Motions.Count; movementId++)
                {
                    Motions.Add(new MotionInfoViewModel(launchInfo.Motions[movementId]));
                }

                await StartSimulationAsync();
            }
        }

        public DifficultyOption Difficulty { get; set; }
        
        public bool IsAdvanced => Difficulty == DifficultyOption.Advanced;

        public float StepSize { get; set; } = 0.1f;

        public ICommand ShareCommand => GetOrCreateCommand(DataTransferManager.ShowShareUI);

        public bool DrawTrajectoriesContinously { get; set; } = true;

        private void _timer_Tick(object sender, object e)
        {
            if (_timer.IsEnabled && _controller != null)
            {
                float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;

                if (_controller.TrajectoryStopTime != null && _controller.SimulationTime.TotalTime > _controller.TrajectoryStopTime)
                {
                    timeElapsed = (float)_controller.TrajectoryStopTime.Value.TotalSeconds;
                }

                foreach (var motion in Motions)
                {
                    motion.UpdateCurrentValues(timeElapsed);
                }
            }
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
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                _controller.Pause();
            }
            else
            {
                _controller.Play();
            }
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
            var dialogViewModel = new AddOrUpdateMotionViewModel(Difficulty, Motions.Select(m => m.Label).ToArray());
            var dialog = new AddOrUpdateMotionDialog(dialogViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Motions.Add(new MotionInfoViewModel(dialogViewModel.ResultMotionInfo));
                if (Motions.Count == 5)
                {
                    AddTrajectoryButtonEnabled = false;
                }
                await StartSimulationAsync();
            }
        }

        private async Task EditTrajectoryAsync(MotionInfoViewModel arg)
        {
            var dialogViewModel = new AddOrUpdateMotionViewModel(arg.MotionInfo, Difficulty, Motions.Select(m => m.Label).ToArray());
            var dialog = new AddOrUpdateMotionDialog(dialogViewModel);
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
            var duplicateMotion = arg.MotionInfo.Clone();
            duplicateMotion.Label =
                $"{duplicateMotion.Label} ({ResourceLoader.GetForCurrentView().GetString("Copy")})";
            var dialogViewModel = new AddOrUpdateMotionViewModel(duplicateMotion, Difficulty, Motions.Select(m => m.Label).ToArray());
            var dialog = new AddOrUpdateMotionDialog(dialogViewModel);
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
            (appWindowContentFrame.Content as ValuesTablePage).Initialize(new PhysicsService(viewModel.MotionInfo), viewModel.MotionInfo.Type);
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
                var page = frame?.Content as ValuesTablePage;
                page?.Initialize(new PhysicsService(viewModel.MotionInfo), viewModel.MotionInfo.Type);
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
            DataTransferManager.GetForCurrentView().DataRequested += MainView_DataRequested;
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();
            foreach (var motion in Motions)
            {
                CloseAppViewForMotionAsync(motion);
            }
            DataTransferManager.GetForCurrentView().DataRequested -= MainView_DataRequested;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            _timer.Stop();
        }
    }
}
