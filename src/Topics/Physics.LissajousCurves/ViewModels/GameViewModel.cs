using System;
using System.Windows.Input;
using Physics.LissajousCurves.Game;
using Physics.LissajousCurves.Rendering;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.LissajousCurves.ViewModels
{
	public class GameViewModel : MainViewModel, IReceiveController<AngryDirectorController>
	{
		private AngryDirectorController _controller = null;
		private float _cameraHeight = 0;
		private Random _randomizer = new Random();
		private readonly ISoundPlayer _soundPlayer;

		public GameViewModel(IContentDialogHelper contentDialogHelper, ISoundPlayer soundPlayer) : base(contentDialogHelper)
		{
			this._soundPlayer = soundPlayer;
		}

		internal AngryDirectorController CreateController(ISkiaCanvas canvas) => new AngryDirectorController(canvas, _soundPlayer);

		public ICommand NewGameCommand => GetOrCreateCommand(NewGame);

		public ICommand StartGameCommand => GetOrCreateCommand(StartGame);

		private void NewGame()
		{
			if (_controller == null)
			{
				return;
			}
			_controller.NewGame(CurrentGame = new GameInfo(_randomizer.Next(5, 9) / 60f) { AreSoundsEnabled = AreSoundsEnabled });
		}

		private void StartGame()
		{
			CameraHeight = 0;
			_controller.StartGame();
		}

		public void SetController(AngryDirectorController controller)
		{
			_controller = controller;
			NewGame();
		}

		public GameInfo CurrentGame { get; private set; }

		private bool _areSoundsEnabled = true;

		public bool AreSoundsEnabled
		{
			get
			{
				return _areSoundsEnabled;
			}
			set
			{
				SetProperty(ref _areSoundsEnabled, value);
				if (CurrentGame != null)
				{
					CurrentGame.AreSoundsEnabled = value;
				}
			}
		}

		public float CameraHeight
		{
			get => _cameraHeight;
			set
			{
				SetProperty(ref _cameraHeight, value);
				if (_controller != null)
				{
					_controller.CameraHeight = (_cameraHeight - 50) * 2 / 100f;
				}
			}
		}
	}
}
