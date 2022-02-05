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
using Physics.Shared.Helpers;
using Physics.Shared.Logic.Constants;
using Physics.Shared.Services.Preferences;
using Physics.Shared.Services.Sounds;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;

namespace Physics.HomogenousMovement.ViewModels
{
    public class GameViewModel : ProjectileMotionSimulationViewModelBase
    {
        private readonly Random _randomizer = new Random();
        private readonly ISoundPlayer _soundPlayer;
        private IGameViewInteraction _gameViewInteraction;
        private GamificationCanvasController _gameController;

        private float _v0 = 50;
        private float _angle = 45;
        private float _gravity = PhysicsConstants.EarthGravity;

        public GameViewModel(IMvxMainThreadAsyncDispatcher dispatcher, ISoundPlayer soundPlayer, IPreferences preferences) 
            : base(dispatcher, preferences)
        {
            _soundPlayer = soundPlayer;
        }

        public override async Task Initialize()
        {
        }

        /// <summary>
        /// Can't pause in game.
        /// </summary>
        public override bool PauseAfterChanges { get => false; set { } }

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

            var shootSourceRelativeToCannonWidth = 0.9f;
            //Calculate origin
            var cannonOperationalLength = (shootSourceRelativeToCannonWidth - GamificationCanvasController.CannonRotationPointRelativeToWidth) * GamificationCanvasController.CannonWidthInMeters;
            var sourceHeight = Math.Sin(MathHelpers.DegreesToRadians(Angle)) * cannonOperationalLength + GamificationCanvasController.CannonRelativeHeightToStand * GamificationCanvasController.CannonStandHeightInMeters;
            var sourceWidth = Math.Cos(MathHelpers.DegreesToRadians(Angle)) * cannonOperationalLength;

            var projectileMotion = MotionFactory.CreateProjectileMotion(
                    new Vector2((float)sourceWidth, (float)sourceHeight),
                    10,
                    0,
                    V0,
                    Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex((Color)Application.Current.Resources["AppThemeColor"]),
                    Angle,
                    Gravity);
            Motions.Add(new MotionInfoViewModel(projectileMotion) { Label = ResourceLoader.GetForCurrentView().GetString("Fire") });

            SimulationPlayback.PlayCommand.Execute(null);
            await StartSimulationAsync();
        }

        public async void SetViewInteraction(IGameViewInteraction gameViewInteraction)
        {
            _gameViewInteraction = gameViewInteraction;
            _gameController = _gameViewInteraction.Initialize(Difficulty, _soundPlayer);
            _controller = _gameController;
            SimulationPlayback.SetController(_controller);
			await Task.Delay(1000);
            await StartNewGameAsync();
        }

        private async Task StartNewGameAsync()
        {
			try
			{
				var castleDistance = _randomizer.Next(300, 500);
				var treeCount = _randomizer.Next(2, 15);
				var treeDistances = new List<int>();
				for (int treeId = 0; treeId < treeCount; treeId++)
				{
					treeDistances.Add(_randomizer.Next(0, castleDistance));
				}
				CurrentGame = new GameSetup(castleDistance, _randomizer.Next(50, castleDistance - 50), treeDistances.ToArray());
				await _gameController.StartNewGameAsync(CurrentGame);
				_gameController.CannonAngle = Angle;
			}
			catch (TaskCanceledException)
			{
				// Unset current game.
				CurrentGame = null;
			}
        }
    }
}
