using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Helpers;
using MvvmCross.Base;
using Physics.HomogenousMovement.Gamification;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.HomogenousMovement.Rendering;
using Physics.HomogenousMovement.ViewInteractions;
using Physics.Shared.Logic.Constants;
using Windows.UI;
using Windows.UI.Xaml;

namespace Physics.HomogenousMovement.ViewModels
{
    public class GameViewModel : SimulationViewModelBase
    {
        private readonly Random _randomizer = new Random();

        private IGameViewInteraction _gameViewInteraction;
        private GamificationCanvasController _gameController;

        private float _v0;
        private float _angle = 45;
        private float _gravity = GravityConstants.Earth;

        public GameViewModel(IMvxMainThreadAsyncDispatcher dispatcher) : base(dispatcher)
        {
        }

        public override async Task Initialize()
        {
        }

        public GameSetup CurrentGame { get; set; }

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
                    UpdateControllerAngle();
                    RaisePropertyChanged();
                }
            }
        }

        private void UpdateControllerAngle()
        {
            _gameController.CannonAngle = Angle;
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

        public ICommand NewGameCommand => GetOrCreateAsyncCommand(StartNewGameAsync);

        public ICommand FireCommand => GetOrCreateAsyncCommand(FireAsync);

        private async Task FireAsync()
        {
            Motions.Clear();
            var projectileMotion = MotionFactory.CreateProjectileMotion(
                    new Vector2(0, 0),
                    10,
                    0,
                    V0,
                    Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex((Color)Application.Current.Resources["AppThemeColor"]),
                    Angle,
                    Gravity);
            Motions.Add(new MotionInfoViewModel(projectileMotion));
            await StartSimulationAsync();
            _gameController.StartNewSimulation(true, Motions.Select(m => m.MotionInfo).ToArray());
        }

        public async void SetViewInteraction(IGameViewInteraction gameViewInteraction)
        {
            _gameViewInteraction = gameViewInteraction;
            _gameController = _gameViewInteraction.Initialize(Difficulty);
            _controller = _gameController;
            await Task.Delay(1000);
            await StartNewGameAsync();
        }

        private async Task StartNewGameAsync()
        {
            var castleDistance = _randomizer.Next(300, 500);
            var treeCount = _randomizer.Next(2, 6);
            var treeDistances = new List<int>();
            for (int treeId = 0; treeId < treeCount; treeId++)
            {
                treeDistances.Add(_randomizer.Next(0, castleDistance));
            }
            CurrentGame = new GameSetup(castleDistance, _randomizer.Next(20, castleDistance), treeDistances.ToArray());
            await _gameController.StartNewGameAsync(CurrentGame);
            _gameController.CannonAngle = Angle;
        }
    }
}
