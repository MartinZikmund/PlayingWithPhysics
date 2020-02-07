using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Physics.HomogenousMovement.Views;
using Physics.Shared.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.HomogenousMovement.PhysicsServices.FreeFall;
using Physics.HomogenousMovement.Rendering;
using Windows.UI.Xaml.Controls;
using Physics.Shared.ViewModels;
using System.Numerics;
using System.Resources;
using System.ServiceModel.Channels;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Popups;
using Physics.HomogenousMovement.Dialogs;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.Shared.Logic.Constants;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Windows.ApplicationModel.DataTransfer;
using Physics.HomogenousMovement.Models;

namespace Physics.HomogenousMovement.ViewModels
{
    public class MainViewModel : ViewModelBase<MainViewModel.NavigationModel>
    {
        public class NavigationModel
        {
            public DifficultyOption Difficulty { get; set; }
            public LaunchInfo LaunchInfo { get; set; }
        }

        private ThrowingCanvasController _controller;
        private DispatcherTimer _timer = new DispatcherTimer();
        private float _x0;
        private float _y0;
        private float _v0;
        private float _mass = 1;
        private float _angle;
        private float _gravity = GravityConstants.Earth;
        private Color _color = Colors.CornflowerBlue;
        private IPhysicsService _service;

        public MainViewModel()
        {
            DisableUnusedInputs();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timer.Tick += _timer_Tick;

           
        }

        private void MainView_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var launchInfo = new LaunchInfo();
            launchInfo.Throws = Throws.Select(t => t.ThrowInfo).Prepend(PrepareMotion()).ToList();
            args.Request.Data.SetWebLink(LaunchUriManager.Serialize("hsf1", launchInfo));
            args.Request.Data.Properties.Title = "Share this motion's uri";
            args.Request.Data.Properties.Description = "This motion and it's settings will be shared as a URI which let's other users make use of it.";
        }


        public override void Prepare(NavigationModel parameter)
        {
            Difficulty = parameter.Difficulty;
            LoadLaunchInfo(parameter.LaunchInfo);
        }

        public void LoadLaunchInfo(LaunchInfo launchInfo)
        {
            if (launchInfo?.Throws != null && launchInfo.Throws.Count > 0)
            {
                Throws.Clear();

                var firstThrow = launchInfo.Throws.First();

                CheckBoxMovementType = firstThrow.Type;
                Color = ColorHelper.ToColor(firstThrow.Color);
                X0 = firstThrow.Origin.X;
                Y0 = firstThrow.Origin.Y;
                V0 = firstThrow.V0;
                Mass = firstThrow.Mass;
                Gravity = firstThrow.G;
                Angle = firstThrow.Angle;

                for (int movementId = 1; movementId < launchInfo.Throws.Count; movementId++)
                {
                    Throws.Add(new MovementInfoViewModel(launchInfo.Throws[movementId]));
                }
            }
        }
        
        public DifficultyOption Difficulty { get; set; }


        public ICommand ShareCommand => GetOrCreateCommand(DataTransferManager.ShowShareUI);

        public MovementType CheckBoxMovementType { get; set; }

        public bool DrawTrajectoriesContinously { get; set; } = true;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                RaisePropertyChanged();
            }
        }

        public float X0
        {
            get => _x0;
            set
            {
                if (!float.IsNaN(value) && value != _x0)
                {
                    _x0 = value;
                    RaisePropertyChanged();
                }

            }
        }

        private void _timer_Tick(object sender, object e)
        {
            if (_timer.IsEnabled && _service != null && _controller != null)
            {
                float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;
                if (timeElapsed > _service.MaxT)
                {
                    timeElapsed = _service.MaxT;
                }
                TimeElapsed = timeElapsed.ToString("0.##") + " s";
                CurrentSpeed = _service.ComputeV(timeElapsed).ToString("0.##") + " m/s";
                CurrentX = _service.ComputeX(timeElapsed).ToString("0.##") + " m";
                CurrentY = _service.ComputeY(timeElapsed).ToString("0.##") + " m";
            }
        }

        public string TimeElapsed { get; private set; }
        public string CurrentSpeed { get; private set; }
        public string CurrentX { get; private set; }
        public string CurrentY { get; private set; }
        public float Y0
        {
            get => _y0;
            set
            {
                if (!float.IsNaN(value) && value != _y0)
                {
                    _y0 = value;
                    RaisePropertyChanged();
                }

            }
        }

        public float Mass
        {
            get => _mass;
            set
            {
                if (!float.IsNaN(value) && value != _mass)
                {
                    _mass = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float V0
        {
            get => _v0;
            set
            {
                if (!float.IsNaN(value) && value != _v0)
                {
                    _v0 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float Angle
        {
            get => _angle;
            set
            {
                if (!float.IsNaN(value) && value != _angle)
                {
                    _angle = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float Gravity
        {
            get => _gravity;
            set
            {
                if (!float.IsNaN(value) && value != _gravity)
                {
                    _gravity = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Visibility IsProjectileMotionCheckBoxEnabled
        {
            get
            {
                bool isEasy = (Difficulty == DifficultyOption.Easy);
                return isEasy ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool IsV0Enabled { get; set; }
        public bool IsMassEnabled { get; set; }
        public bool IsAngleEnabled { get; set; }
        public bool IsX0Enabled { get; set; }
        public bool IsY0Enabled { get; set; }

        public ObservableCollection<MovementInfoViewModel> Throws { get; } =
            new ObservableCollection<MovementInfoViewModel>();

        public void SetCanvasController(ThrowingCanvasController controller) => _controller = controller;

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableDialog);

        public ICommand StartNewSimulationCommand => GetOrCreateAsyncCommand(StartSimulationAsync);

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(AddTrajectoryAsync);

        public ICommand EditTrajectoryCommand => GetOrCreateAsyncCommand<MovementInfoViewModel>(EditTrajectoryAsync);

        public ICommand DeleteTrajectoryCommand => GetOrCreateAsyncCommand<MovementInfoViewModel>(DeleteTrajectoryAsync);

        private Task DeleteTrajectoryAsync(MovementInfoViewModel arg)
        {
            Throws.Remove(arg);
            return Task.CompletedTask;
        }

        private async Task AddTrajectoryAsync()
        {
            var service = PrepareService();
            var dialogViewModel = new AddOrUpdateTrajectoryViewModel(GenerateNextUniqueThrowName());
            var dialog = new AddOrUpdateTrajectoryDialog(dialogViewModel);
            var motion = PrepareMotion();
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                motion.Label = dialogViewModel.Label;
                Throws.Add(new MovementInfoViewModel(motion));
            }
        }

        private async Task EditTrajectoryAsync(MovementInfoViewModel arg)
        {
            var dialogViewModel = new AddOrUpdateTrajectoryViewModel(arg.Label);
            var dialog = new AddOrUpdateTrajectoryDialog(dialogViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                arg.Label = dialogViewModel.Label;
            }
        }

        private string GenerateNextUniqueThrowName()
        {
            var currentId = Throws.Count + 1;
            while (Throws.FirstOrDefault(t => t.Label.StartsWith($"#{currentId}")) != null)
            {
                currentId++;
            }

            var throwTypeLabel = "";
            switch (CheckBoxMovementType)
            {
                case MovementType.FreeFall:
                    throwTypeLabel = "Volný pád";
                    break;
                case MovementType.UpwardThrow:
                    throwTypeLabel = "Svislý vrh";
                    break;
                case MovementType.ForwardThrow:
                    throwTypeLabel = "Vodorovný vrh";
                    break;
                case MovementType.ProjectileMotion:
                    throwTypeLabel = "Šikmý vrh";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return $"#{currentId} - {throwTypeLabel}";
        }

        private async Task StartSimulationAsync()
        {
            var resourceManager = ResourceLoader.GetForCurrentView();
            string errorMessage = resourceManager.GetString("ArgumentExceptionErrorMessage");

            try
            {
                var motion = PrepareMotion();
                _service = PrepareService();
                _timer.Start();
                await _controller.RunOnGameLoopAsync(() =>
                {
                    _controller.StartNewSimulation(DrawTrajectoriesContinously, Throws.Select(t => t.ThrowInfo).Prepend(motion).ToArray());
                });
            }
            catch (ArgumentException exception)
            {
                var messageDialog = new MessageDialog(exception.Message, errorMessage);
                await messageDialog.ShowAsync();
            }
        }

        private async Task ShowValuesTableDialog()
        {
            var resourceManager = ResourceLoader.GetForCurrentView();
            string errorMessage = resourceManager.GetString("ArgumentExceptionErrorMessage");

            try
            {
                var service = PrepareService();

                //Check if service exists
                var dialog = new ValuesTableDialog(service, CheckBoxMovementType);
                await dialog.ShowAsync();
            }
            catch (ArgumentException exception)
            {
                var messageDialog = new MessageDialog(exception.Message, errorMessage);
                await messageDialog.ShowAsync();
            }
        }

        private ThrowInfo PrepareMotion()
            => CheckBoxMovementType switch
            {
                MovementType.FreeFall => ThrowFactory.CreateFreeFall(
                    new Vector2(0, Y0),
                    Mass,
                    0,
                    ColorHelper.ToHex(Color), Gravity),
                MovementType.UpwardThrow =>
                ThrowFactory.CreateUpwardThrow(
                    new Vector2(0, Y0),
                    Mass,
                    0,
                    V0,
                    ColorHelper.ToHex(Color), Gravity),
                MovementType.ForwardThrow => ThrowFactory.CreateHorizontalThrow(
                    new Vector2(0, Y0),
                    Mass,
                    0,
                    V0,
                    ColorHelper.ToHex(Color), Gravity),
                MovementType.ProjectileMotion => ThrowFactory.CreateProjectileMotion(
                    new Vector2(0, Y0),
                    Mass,
                    0,
                    V0,
                    ColorHelper.ToHex(Color),
                    Angle, Gravity),
                _ => throw new ArgumentNullException()
            };

        private IPhysicsService PrepareService()
        {
            var info = PrepareMotion();
            return new PhysicsService(info);
        }

        private void DisableUnusedInputs()
        {
            switch (CheckBoxMovementType)
            {
                case MovementType.UpwardThrow:
                    IsY0Enabled = true;
                    IsV0Enabled = true;
                    IsMassEnabled = true;
                    IsAngleEnabled = false;
                    break;
                case MovementType.ForwardThrow:
                    IsY0Enabled = true;
                    IsV0Enabled = true;
                    IsMassEnabled = true;
                    IsAngleEnabled = false;
                    break;
                case MovementType.ProjectileMotion:
                    IsY0Enabled = true;
                    IsV0Enabled = true;
                    IsMassEnabled = true;
                    IsAngleEnabled = true;
                    break;
                default:
                    IsY0Enabled = true;
                    IsV0Enabled = false;
                    IsMassEnabled = true;
                    IsAngleEnabled = false;
                    break;
            }
        }
        public void OnCheckBoxMovementTypeChanged()
        {
            DisableUnusedInputs();

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
            //TODO: Implement _timer.Stop() on movement halt
        }
    }
}
