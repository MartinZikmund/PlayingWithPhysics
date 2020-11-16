using MvvmCross.Base;
using Physics.DragMovement.ViewInteractions;
using Physics.Shared.Services.Preferences;
using Physics.Shared.Services.Sounds;
using System.Threading.Tasks;

namespace Physics.DragMovement.ViewModels
{
    public class GameViewModel : DragMovementSimulationViewModelBase
    {
		private readonly ISoundPlayer _soundPlayer;

        private IGameViewInteraction _gameViewInteraction;

		public async void SetViewInteraction(IGameViewInteraction gameViewInteraction)
        {
            _gameViewInteraction = gameViewInteraction;
            _controller = _gameViewInteraction.Initialize(Difficulty, _soundPlayer);
            SimulationPlayback.SetController(_controller);
            if (_startWithController)
            {
                await Task.Delay(1000);
                await StartSimulationAsync();
            }
        }

        public GameViewModel(IMvxMainThreadAsyncDispatcher dispatcher, IPreferences preferences, ISoundPlayer soundPlayer) : base(dispatcher, preferences)
        {
			_soundPlayer = soundPlayer;
		}
    }
}
