using Physics.Shared.UI.Rendering;
using Physics.Shared.ViewModels;
using System.Windows.Input;

namespace Physics.Shared.UI.ViewModels
{
    public class SimulationPlaybackViewModel : ViewModelBase
    {
        private IRenderingPlayback _renderingPlayback;

        public void SetController(IRenderingPlayback canvasController)
        {
            _renderingPlayback = canvasController;
			if (_renderingPlayback != null)
			{
				_renderingPlayback.PlayStateChanged += PlayStateChanged;
			}
            RaisePropertyChanged(nameof(IsPaused));
        }

		private void PlayStateChanged(object sender, System.EventArgs e)
		{
			RaisePropertyChanged(nameof(IsPaused));
		}

		public ICommand PlayCommand => GetOrCreateCommand(Play);

        public ICommand PauseCommand => GetOrCreateCommand(Pause);

        public ICommand JumpBackCommand => GetOrCreateCommand(JumpBack);

        public ICommand JumpForwardCommand => GetOrCreateCommand(JumpForward);

        public ICommand JumpToStartCommand => GetOrCreateCommand(JumpToStart);

        public ICommand JumpToEndCommand => GetOrCreateCommand(JumpToEnd);

        private void JumpToEnd()
        {
        }

        private void JumpToStart() => _renderingPlayback?.SimulationTime.Restart();

        private void JumpForward() => _renderingPlayback?.FastForward(JumpSize);

        public bool IsPaused => _renderingPlayback?.IsPaused ?? false;

        public float JumpSize { get; set; } = 0.5f;

        public float PlaybackSpeed { get; set; } = 1.0f;

        private void JumpBack() => _renderingPlayback?.Rewind(JumpSize);

        private void OnPlaybackSpeedChanged()
        {
            if (_renderingPlayback != null)
            {
                _renderingPlayback.SimulationTime.SimulationSpeed = PlaybackSpeed;
            }
        }

        public void Pause()
        {
            _renderingPlayback?.Pause();
            RaisePropertyChanged(nameof(IsPaused));
        }

		public void Play()
        {
            _renderingPlayback?.Play();
            RaisePropertyChanged(nameof(IsPaused));
        }
    }
}
