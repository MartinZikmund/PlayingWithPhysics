using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.HuygensPrinciple.Logic;
using Physics.HuygensPrinciple.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<HuygensPrincipleCanvasController>
	{
		private DifficultyOption _difficulty;
		private HuygensPrincipleCanvasController _controller;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;
		}

		public bool IsLoading { get; set; }

		public void SetController(HuygensPrincipleCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_controller.SetVariantRenderer(_difficulty == DifficultyOption.Easy ? (IHuygensVariantRenderer)new EasyVariantRenderer(_controller) : new AdvancedVariantRenderer(_controller));
			SimulationPlayback.SetController(_controller);
		}

		public ICommand DrawSceneCommand => GetOrCreateAsyncCommand<int>(DrawSceneAsync);

		public async Task DrawSceneAsync(int sceneId)
		{
			IsLoading = true;
			var huygensBuilder = new HuygensFieldBuilder(RenderingConfiguration.FieldSize, RenderingConfiguration.FieldSize);
			huygensBuilder.DrawScene(ScenePresets.Presets[sceneId]);

			var manager = new HuygensManager(huygensBuilder.Build());
			await manager.PrecalculateAsync();
			_controller.StartSimulation(manager, ScenePresets.Presets[sceneId]);
			IsLoading = false;
		}
	}
}
