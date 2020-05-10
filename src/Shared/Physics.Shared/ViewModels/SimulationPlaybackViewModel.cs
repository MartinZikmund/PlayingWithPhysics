using Physics.Shared.UI.Rendering;
using Physics.Shared.ViewModels;
using System.Windows.Input;

namespace Physics.Shared.UI.ViewModels
{
    public class SimulationPlaybackViewModel : ViewModelBase
    {
        private BaseCanvasController _canvasController;

        public void SetController(BaseCanvasController canvasController) => 
            _canvasController = canvasController;

        public ICommand PlayCommand => GetOrCreateCommand(Play);

        public ICommand PauseCommand => GetOrCreateCommand(Pause);

        public ICommand JumpBackCommand => GetOrCreateCommand(JumpBack);

        public ICommand JumpForwardCommand => GetOrCreateCommand(JumpForward);

        public ICommand JumpToStartCommand => GetOrCreateCommand(JumpToStart);

        public ICommand JumpToEndCommand => GetOrCreateCommand(JumpToEnd);

        private void JumpToEnd()
        {
        }

        private void JumpToStart() => _canvasController.SimulationTime.Restart();

        private void JumpForward() => _canvasController.FastForward(JumpSize);

        public bool IsPaused => _canvasController.IsPaused;

        public float JumpSize { get; set; }

        public float PlaybackSpeed { get; set; }

        private void JumpBack() => _canvasController.Rewind(JumpSize);

        private void OnPlaybackSpeedChanged()
        {
            _canvasController.SimulationTime.SimulationSpeed = PlaybackSpeed;
        }

        private void Pause()
        {
            _canvasController?.Pause();
            RaisePropertyChanged(nameof(IsPaused));
        }

        private void Play()
        {
            _canvasController?.Play();
            RaisePropertyChanged(nameof(IsPaused));
        }
    }
}
