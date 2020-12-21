using System.Windows.Input;
using Physics.CompoundOscillations.Rendering;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.CompoundOscillations.ViewModels
{
	public class GameViewModel : MainViewModel, IReceiveController<AngryDirectorController>
	{
		private AngryDirectorController _controller = null;
		private float _cameraHeight;

		public GameViewModel(IContentDialogHelper contentDialogHelper) : base(contentDialogHelper)
		{
		}

		public ICommand NewGameCommand => GetOrCreateCommand(NewGame);

		private void NewGame()
		{
			_controller?.Reset();
		}

		public void SetController(AngryDirectorController controller) => _controller = controller;

		public bool AreSoundsEnabled { get; set; }

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
