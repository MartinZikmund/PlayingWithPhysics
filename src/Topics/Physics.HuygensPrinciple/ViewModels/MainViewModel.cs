using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.HuygensPrinciple.Logic;
using Physics.HuygensPrinciple.Rendering;
using Physics.HuygensPrinciple.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Windows.UI.Xaml.Controls;

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
			_controller.SetRenderConfiguration(RenderConfiguration);
			SimulationPlayback.SetController(_controller);
		}

		public RenderConfigurationViewModel RenderConfiguration { get; } = new RenderConfigurationViewModel();

		public ICommand PickScenePresetCommand => GetOrCreateAsyncCommand(PickSceneAsync);

		private async Task PickSceneAsync()
		{
			var scenePicker = new ScenePickerDialog(_difficulty);
			if (await scenePicker.ShowAsync() == ContentDialogResult.Primary)
			{
				var scene = scenePicker.ViewModel.SelectedScene;
				await DrawSceneAsync(scene.Preset);
			}
		}

		public async Task DrawSceneAsync(ScenePreset scene)
		{
			IsLoading = true;
			var huygensBuilder = new HuygensFieldBuilder(RenderingConfiguration.FieldSize, RenderingConfiguration.FieldSize);
			huygensBuilder.DrawScene(scene);

			var manager = new HuygensManager(huygensBuilder.Build());
			await manager.PrecalculateAsync();
			_controller.StartSimulation(manager, scene);
			IsLoading = false;
		}
	}
}
