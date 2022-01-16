using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.FluidFlow.Rendering;
using System.Windows.Input;
using System.Threading.Tasks;
using Physics.FluidFlow.Logic;
using Physics.FluidFlow.Controls;

namespace Physics.FluidFlow.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<FluidFlowCanvasController>
	{
		private DifficultyOption _difficulty;
		private FluidFlowCanvasController _controller;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			IsLoading = true;
			_difficulty = parameter.Difficulty;
			if (_difficulty == DifficultyOption.Easy)
			{
				InputVariants = new[]
				{
					InputVariant.ContinuityEquation,
					InputVariant.BernoulliEquationWithoutHeightChange,
				};
				SelectedVariantIndex = (int)InputVariant.ContinuityEquation;
			}
			else
			{
				InputVariants = new[]
				{
					InputVariant.BernoulliEquationWithHeightChange,
					InputVariant.RealFluidMovement,
				};
				SelectedVariantIndex = (int)InputVariant.ContinuityEquation;
			}
			IsLoading = false;
		}

		public bool IsLoading { get; set; }

		public void SetController(FluidFlowCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		public InputVariant[] InputVariants { get; private set; }

		public int SelectedVariantIndex { get; set; }

		public InputVariant SelectedVariant => (InputVariant)SelectedVariantIndex;

		internal async void OnSelectedVariantIndexChanged()
		{
			if (IsLoading)
			{
				// Ignore initial set value
				return;
			}

			if (Enum.IsDefined(typeof(InputVariant), SelectedVariantIndex))
			{
				await SetParametersAsync();
			}
		}

		public ICommand SetParametersCommand => GetOrCreateAsyncCommand(SetParametersAsync);

		private async Task SetParametersAsync()
		{
			var sceneConfigurationDialog = new SceneConfigurationDialog();
			await sceneConfigurationDialog.ShowAsync();
		}
	}
}
