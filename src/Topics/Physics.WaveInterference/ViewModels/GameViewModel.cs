using System;
using DynamicData;
using Physics.Shared.UI.Views.Interactions;
using Physics.Shared.ViewModels;
using Physics.WaveInterference.Game;
using Physics.WaveInterference.Rendering;

namespace Physics.WaveInterference.ViewModels
{
	public class GameViewModel : ViewModelBase, IReceiveController<GameController>
	{
		private GameController _controller;
		
		public void SetController(GameController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_controller.SetGameInfo(GameInfo);
			_controller.Play();
		}

		public GameInfo GameInfo { get; } = new GameInfo();
	}
}
