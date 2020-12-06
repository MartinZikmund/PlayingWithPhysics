using MvvmCross.Base;
using Physics.DragMovement.Gamification;
using Physics.DragMovement.Rendering;
using Physics.DragMovement.ViewInteractions;
using Physics.Shared.Services.Preferences;
using Physics.Shared.Services.Sounds;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Physics.DragMovement.ViewModels
{
    public class GameViewModel : DragMovementSimulationViewModelBase
    {
		private static readonly Random _randomizer = new Random();
		private readonly ISoundPlayer _soundPlayer;

		private IGameViewInteraction _gameViewInteraction;
		private GamificationCanvasController _gameController;
		private float _mass;

		public async void SetViewInteraction(IGameViewInteraction gameViewInteraction)
		{
			_gameViewInteraction = gameViewInteraction;
			_gameController = _gameViewInteraction.Initialize(Difficulty, _soundPlayer);
			_controller = _gameController;
			SimulationPlayback.SetController(_controller);			
			await Task.Delay(300);
			await _gameController.StartNewGameAsync(CurrentGame);
		}

		public GameViewModel(IMvxMainThreadAsyncDispatcher dispatcher, IPreferences preferences, ISoundPlayer soundPlayer) : base(dispatcher, preferences)
        {
			_soundPlayer = soundPlayer;
		}

		/// <summary>
		/// Can't pause in game.
		/// </summary>
		public override bool PauseAfterChanges { get => false; set { } }

		public GameInfo CurrentGame { get; set; } = new GameInfo(_randomizer.Next(10, 25) / 10f, _randomizer.Next(100, 180));

		private async Task StartNewGameAsync()
		{			
			CurrentGame = new GameInfo(_randomizer.Next(10, 25) / 10f, _randomizer.Next(100, 180));
			await _gameController.StartNewGameAsync(CurrentGame);
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

		public ICommand NewGameCommand => GetOrCreateAsyncCommand(StartNewGameAsync);

		public ICommand FireCommand => GetOrCreateAsyncCommand(FireAsync);

		public void Start()
		{
			_gameController.StartAttempt();
		}

		public void Drop()
		{
			_gameController.DropCargo();			
		}

		public void Restart()
		{
			_gameController.RestartAttempt();
		}

		private async Task FireAsync()
		{
			//Motions.Clear();

			//var shootSourceRelativeToCannonWidth = 0.9f;
			////Calculate origin
			//var cannonOperationalLength = (shootSourceRelativeToCannonWidth - GamificationCanvasController.CannonRotationPointRelativeToWidth) * GamificationCanvasController.CannonWidthInMeters;
			//var sourceHeight = Math.Sin(MathHelpers.DegreesToRadians(Angle)) * cannonOperationalLength + GamificationCanvasController.CannonRelativeHeightToStand * GamificationCanvasController.CannonStandHeightInMeters;
			//var sourceWidth = Math.Cos(MathHelpers.DegreesToRadians(Angle)) * cannonOperationalLength;

			//var projectileMotion = MotionFactory.CreateProjectileMotion(
			//		new Vector2((float)sourceWidth, (float)sourceHeight),
			//		10,
			//		0,
			//		V0,
			//		Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHex((Color)Application.Current.Resources["AppThemeColor"]),
			//		Angle,
			//		Gravity);
			//Motions.Add(new MotionInfoViewModel(projectileMotion) { Label = ResourceLoader.GetForCurrentView().GetString("Fire") });

			//SimulationPlayback.PlayCommand.Execute(null);
			//await StartSimulationAsync();
		}
	}
}
