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

namespace Physics.HomogenousMovement.ViewModels
{
    public class MainViewModel : ViewModelBase<MainViewModel.NavigationModel>
    {
        public class NavigationModel
        {
            public DifficultyOption Difficulty { get; set; }
            public LaunchInfo LaunchInfo { get; set; }
        }

        private MotioningCanvasController _controller;
        private LaunchInfo _launchInfo = null;
        private IPhysicsService _selectedMotionPhysicsService = null;
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
        public float StepSize { get; set; } = 0.1f;
        public ICommand ShareCommand => GetOrCreateCommand(DataTransferManager.ShowShareUI);

        public bool DrawTrajectoriesContinously { get; set; } = true;

        private void _timer_Tick(object sender, object e)
        {
            if (SelectedMotion == null)
            {
                return;
            }
            if (_timer.IsEnabled && _selectedMotionPhysicsService != null && _controller != null)
            {
                float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;
                if (timeElapsed > _selectedMotionPhysicsService.MaxT)
                {
                    timeElapsed = _selectedMotionPhysicsService.MaxT;
                }
                TimeElapsed = timeElapsed.ToString("0.##") + " s";
                CurrentSpeed = _selectedMotionPhysicsService.ComputeV(timeElapsed).ToString("0.##") + " m/s";
                CurrentX = _selectedMotionPhysicsService.ComputeX(timeElapsed).ToString("0.##") + " m";
                CurrentY = _selectedMotionPhysicsService.ComputeY(timeElapsed).ToString("0.##") + " m";
            }
        }

        public string TimeElapsed { get; private set; }
        public string CurrentSpeed { get; private set; }
        public string CurrentX { get; private set; }
        public string CurrentY { get; private set; }

        public ObservableCollection<MotionInfoViewModel> Motions { get; } =
            new ObservableCollection<MotionInfoViewModel>();

        public MotionInfoViewModel SelectedMotion { get; set; }

        public void OnSelectedMotionChanged()
        {
            if (SelectedMotion == null)
            {
                _selectedMotionPhysicsService = null;
            }
            else
            {
                _selectedMotionPhysicsService = new PhysicsService(SelectedMotion.MotionInfo);
            }
        }

        public async void SetCanvasController(MotioningCanvasController controller)
        {
            _controller = controller;
            if (_startWithController)
            {
                await Task.Delay(1000);
                await StartSimulationAsync();
            }
        }

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableDialog);

        public ICommand StartNewSimulationCommand => GetOrCreateAsyncCommand(StartSimulationAsync);

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(AddTrajectoryAsync);

        public ICommand EditTrajectoryCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(EditTrajectoryAsync);

        public ICommand DeleteTrajectoryCommand => GetOrCreateAsyncCommand<MotionInfoViewModel>(DeleteTrajectoryAsync);

        private async Task DeleteTrajectoryAsync(MotionInfoViewModel arg)
        {
            Motions.Remove(arg);
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
                if (Motions.Count == 1)
                {
                    SelectedMotion = Motions[0];
                }
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

        private async Task ShowValuesTableDialog()
        {
            if (SelectedMotion == null || _selectedMotionPhysicsService == null)
            {
                return;
            }

            //Check if service exists
            var dialog = new ValuesTableDialog(_selectedMotionPhysicsService, SelectedMotion.MotionInfo.Type);
            await dialog.ShowAsync();
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
