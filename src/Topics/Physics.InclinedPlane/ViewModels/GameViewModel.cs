using MvvmCross.Base;
using Physics.InclinedPlane.Game;
using Physics.InclinedPlane.Logic.PhysicsServices;
using Physics.InclinedPlane.Rendering;
using Physics.InclinedPlane.Services;
using Physics.InclinedPlane.ValuesTable;
using Physics.InclinedPlane.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.InclinedPlane.ViewModels
{
	public class GameViewModel : SimulationViewModelBase<GameViewModel.NavigationModel>, IReceiveController<InclinedPlaneSkiaController>
    {
        private DifficultyOption Difficulty;
        private InclinedPlaneSkiaController _controller;
        private DispatcherTimer _timer = new DispatcherTimer();

        public class NavigationModel
        {
            public DifficultyOption Difficulty { get; set; }
        }

        public GameViewModel()
        {
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timer.Tick += _timer_Tick;
        }

        public void PreviewStone(float? x)
        {
            if (_gameInfo.State == GameState.PlaceStone)
            {
                ((GameRenderer)_controller.Renderer).PreviewStoneXInRenderCoordinates = x;
            }
        }

        public override void Prepare(NavigationModel parameter)
        {
            Difficulty = parameter.Difficulty;
        }

		public void SetController(InclinedPlaneSkiaController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
            (_controller.Renderer as GameRenderer)?.StartGame(_gameInfo);
		}

        public ICommand StartNewGameCommand => GetOrCreateCommand(() =>
        {
            Motion = null;
            _timer.Stop();
            _gameInfo = new GameInfo();
            (_controller.Renderer as GameRenderer)?.StartGame(_gameInfo);
        });

        private GameInfo _gameInfo = new GameInfo();

        protected void RestartSimulation(float inclinedLength)
        {
            Motion = new MotionViewModel(new InclinedPlaneMotionSetup(
                19.1f,
                0,
                9.81f,
                inclinedLength,
                0.45f,
                30,
                36,
                0.03f,
                "#000000"));
            if (_controller == null) return;

            _controller.StartSimulation(Motion.MotionInfo);
            _timer.Start();
        }

        internal void CanvasTapped(float x)
        {
            if (_gameInfo.State == GameState.PlaceStone)
            {
                var placement = ((GameRenderer)_controller.Renderer).CalculateInclinedPlanePlacement(x);
                if (placement != null)
                {
                    RestartSimulation(placement.Value);
                    _gameInfo.State = GameState.Simulation;
                }
            }
            else if (_gameInfo.State == GameState.ThrowEnded)
            {
                _gameInfo.ThrowCount++;
                _gameInfo.State = GameState.PlaceStone;
            }
        }

        public IInclinedPlaneMotionSetup Setup { get; set; }

        public MotionViewModel Motion { get; set; }

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
        }

        public string DrawingContent { get; set; }

        public ICommand ShareCommand => GetOrCreateCommand(DataTransferManager.ShowShareUI);

        public float StepSize { get; set; } = 0.1f;

        public bool IsPaused { get; set; } = true;
        public Visibility ShowCurrentValuesGrid => (Setup != null) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand PauseToggleCommand => GetOrCreateCommand(PauseToggle);

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<MotionViewModel>(ShowValuesTableAsync);
        private void PauseToggle()
        {
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

        private void _timer_Tick(object sender, object e)
        {
            if (_timer.IsEnabled && _controller != null && _gameInfo.State == GameState.Simulation)
            {
                float timeElapsed = (float)_controller.SimulationTime.TotalTime.TotalSeconds;

                Motion?.UpdateCurrentValues(timeElapsed);

                if (timeElapsed >= _controller.PhysicsService.CalculateMaxT())
                {
                    var renderer = (GameRenderer)_controller.Renderer;
                    _gameInfo.AddFinishedThrow(renderer.CalculateDistanceFromTarget());
                    if (_gameInfo.ThrowCount != _gameInfo.TotalThrows)
                    {
                        _gameInfo.State = GameState.ThrowEnded;
                    }
                    else
                    {
                        _gameInfo.State = GameState.GameEnded;
                    }
                    _timer.Stop();
                }
            }
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
            _timer.Stop();
        }
	}
}
