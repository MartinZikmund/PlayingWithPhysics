using Physics.HomogenousMovement.Views;
using Physics.Shared.Infrastructure.Topics;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.HomogenousMovement.Rendering;
using Windows.UI.Xaml.Controls;
using Physics.Shared.ViewModels;
using Windows.ApplicationModel.Resources;
using Physics.HomogenousMovement.Dialogs;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Windows.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using Physics.HomogenousMovement.Models;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using System.Collections.Generic;

namespace Physics.HomogenousMovement.ViewModels
{
    public class MainViewModel : ViewModelBase<MainViewModel.NavigationModel>
    {
        public class NavigationModel
        {
            public DifficultyOption Difficulty { get; set; }
            public LaunchInfo LaunchInfo { get; set; }
        }

        private HomogenousMovementCanvasController _controller;
        private LaunchInfo _launchInfo = null;
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _startWithController = false;

        public MainViewModel()
        {
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timer.Tick += _timer_Tick;
        }

        private void MainView_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var launchInfo = new LaunchInfo();
            launchInfo.Motions = Motions.Select(t => t.MotionInfo).ToList();
            args.Request.Data.SetWebLink(LaunchUriManager.Serialize("hsf1", launchInfo));
            args.Request.Data.Properties.Title = "Share this motion's uri";
            args.Request.Data.Properties.Description = "This motion and it's settings will be shared as a URI which let's other users make use of it.";
        }


        public override void Prepare(NavigationModel parameter)
        {
            Difficulty = parameter.Difficulty;
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


        public ICommand ShareCommand => GetOrCreateCommand(DataTransferManager.ShowShareUI);

        public bool DrawTrajectoriesContinously { get; set; } = true;

        private void _timer_Tick(object sender, object e)
        {
            if (_timer.IsEnabled && _controller != null)
            {
                float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;

                foreach (var motion in Motions)
                {
                    motion.UpdateCurrentValues(timeElapsed);
                }
            }
        }


        public ObservableCollection<MotionInfoViewModel> Motions { get; } =
            new ObservableCollection<MotionInfoViewModel>();

        public async void SetCanvasController(HomogenousMovementCanvasController controller)
        {
            _controller = controller;
            if (_startWithController)
            {
                await Task.Delay(1000);
                await StartSimulationAsync();
            }
        }

        public ICommand StartNewSimulationCommand => GetOrCreateAsyncCommand(StartSimulationAsync);

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(AddTrajectoryAsync);

        public ICommand EditTrajectoryCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(EditTrajectoryAsync);

        public ICommand DeleteTrajectoryCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(DeleteTrajectoryAsync);

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(ShowValuesTableAsync);

        private async Task DeleteTrajectoryAsync(MotionInfoViewModel arg)
        {
            Motions.Remove(arg);
            await CloseAppViewForMotionAsync(arg);
            await StartSimulationAsync();
        }

        private async Task AddTrajectoryAsync()
        {
            var dialogViewModel = new AddOrUpdateMotionViewModel(GenerateNextUniqueMotionName(), Difficulty);
            var dialog = new AddOrUpdateMotionDialog(dialogViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Motions.Add(new MotionInfoViewModel(dialogViewModel.ResultMotionInfo));
                await StartSimulationAsync();
            }
        }

        private string GenerateNextUniqueMotionName()
        {
            var currentId = Motions.Count + 1;

            while (Motions.FirstOrDefault(t => t.Label.StartsWith($"#{currentId}")) != null)
            {
                currentId++;
            }

            var resourceLoader = ResourceLoader.GetForCurrentView();

            return $"{resourceLoader.GetString("Motion")} #{currentId}";
        }

        private async Task EditTrajectoryAsync(MotionInfoViewModel arg)
        {
            var dialogViewModel = new AddOrUpdateMotionViewModel(arg.MotionInfo, Difficulty);
            var dialog = new AddOrUpdateMotionDialog(dialogViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                arg.MotionInfo = dialogViewModel.ResultMotionInfo;
                await StartSimulationAsync();
            }
        }

        private async Task StartSimulationAsync()
        {
            if (_controller == null)
            {
                _startWithController = true;
                return;
            }

            _timer.Start();
            await _controller.RunOnGameLoopAsync(() =>
            {
                _controller.StartNewSimulation(DrawTrajectoriesContinously, Motions.Select(t => t.MotionInfo).ToArray());
            });
        }

        private Dictionary<MotionInfoViewModel, int> _tableWindowIds = new Dictionary<MotionInfoViewModel, int>();

        private async Task ShowValuesTableAsync(MotionInfoViewModel viewModel)
        {
            if (_tableWindowIds.ContainsKey(viewModel)) return;
            var newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(ValuesTablePage), null);
                (frame.Content as ValuesTablePage).Initialize(new PhysicsService(viewModel.MotionInfo), viewModel.MotionInfo.Type);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = viewModel.Label;
                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
            if (viewShown)
            {
                _tableWindowIds.Add(viewModel, newViewId);
            }
            ////Check if service exists
            //var dialog = new ValuesTableDialog(_selectedMotionPhysicsService, SelectedMotion.MotionInfo.Type);
            //await dialog.ShowAsync();
        }

        private async Task CloseAppViewForMotionAsync(MotionInfoViewModel viewModel)
        {
            //if (_tableWindowIds.TryGetValue(viewModel, out var windowId))
            //{
            //    var coreView = CoreApplication.Views.FirstOrDefault(v => ApplicationView.GetApplicationViewIdForWindow(v.CoreWindow) == windowId);

            //    if (coreView != null)
            //    {
            //        await coreView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            //        {
            //            await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            //        });
            //    }

            //    _tableWindowIds.Remove(viewModel);
            //}
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            DataTransferManager.GetForCurrentView().DataRequested += MainView_DataRequested;
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();
            DataTransferManager.GetForCurrentView().DataRequested -= MainView_DataRequested;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            _timer.Stop();
        }
    }
}
