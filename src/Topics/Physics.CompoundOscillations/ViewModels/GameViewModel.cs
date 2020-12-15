using System;
using System.Windows.Input;
using Physics.CompoundOscillations.Rendering;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.CompoundOscillations.ViewModels
{
	public class GameViewModel : MainViewModel, IReceiveController<AngryDirectorController>
	{
		private AngryDirectorController _controller = null;

		public GameViewModel(IContentDialogHelper contentDialogHelper) : base(contentDialogHelper)
		{
		}

		public ICommand NewGameCommand => GetOrCreateCommand(NewGame);

		private void NewGame()
		{
		}

		public void SetController(AngryDirectorController controller) => _controller = controller;

		public bool AreSoundsEnabled { get; set; }
	}
}
