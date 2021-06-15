using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.RotationalInclinedPlane.Rendering;
using Physics.RotationalInclinedPlane.Logic;
using Physics.RotationalInclinedPlane.Dialogs;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<RotationalInclinedPlaneCanvasController>
	{
		private DifficultyOption _difficulty;
		private RotationalInclinedPlaneCanvasController _controller;

		public MotionSetup Setup { get; set; }

		public ICommand EditValuesCommand => GetOrCreateAsyncCommand(EditValuesAsync);

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

		public void SetController(RotationalInclinedPlaneCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		public async Task EditValuesAsync()
		{
			InputDialogViewModel viewModel;
			if (Setup != null)
			{
				viewModel = new InputDialogViewModel(Setup, _difficulty);
			}
			else
			{
				viewModel = new InputDialogViewModel(_difficulty);
			}

			var dialog = new InputDialog(viewModel);
			await dialog.ShowAsync();
		}
	}
}
