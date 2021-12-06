using System;
using System.Linq;
using Physics.OpticalInstruments.Logic;
using Physics.OpticalInstruments.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using SkiaSharp;
using Windows.Foundation;
using Windows.UI.Input;

namespace Physics.OpticalInstruments.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<OpticalInstrumentsCanvasController>
	{
		private DifficultyOption _difficulty;
		private OpticalInstrumentsCanvasController _controller;

		public bool IsLoading { get; set; }

		public InstrumentTypeViewModel[] InstrumentTypes { get; } =
			Enum.GetValues(typeof(InstrumentType))
				.OfType<InstrumentType>()
				.Select(i => new InstrumentTypeViewModel(i))
				.ToArray();

		internal void TrySetObject(Point desiredPoint)
		{
			if (_controller == null)
			{
				return;
			}

			var pointerPoint = new SKPoint((float)desiredPoint.X, (float)desiredPoint.Y);
			if (_controller.TryGetObjectPosition(
				pointerPoint,
				out var objectPoint))
			{
				SceneConfiguration.ObjectDistance = objectPoint.X;
				SceneConfiguration.ObjectHeight = objectPoint.Y;
			};
		}

		public InstrumentTypeViewModel SelectedInstrumentType { get; set; }

		public SceneConfiguration SceneConfiguration { get; } = new();

		public override void Prepare(SimulationNavigationModel parameter) => _difficulty = parameter.Difficulty;

		public void SetController(OpticalInstrumentsCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_controller.SceneConfiguration = SceneConfiguration;
			SelectedInstrumentType = InstrumentTypes.First();
		}

		public void OnSelectedInstrumentTypeChanged()
		{
			if (_controller == null || SelectedInstrumentType == null)
			{
				return;
			}

			switch (SelectedInstrumentType.Type)
			{
				case InstrumentType.ConvexMirror:
					_controller.SetVariantRenderer(new ConvexMirrorRenderer(_controller));
					break;
				case InstrumentType.ConcaveMirror:
					_controller.SetVariantRenderer(new ConcaveMirrorRenderer(_controller));
					break;
				case InstrumentType.ConvexLens:
					_controller.SetVariantRenderer(new ConvexLensRenderer(_controller));
					break;
				case InstrumentType.ConcaveLens:
					_controller.SetVariantRenderer(new ConcaveLensRenderer(_controller));
					break;
				default:
					break;
			}
		}

		//private async Task PickSceneAsync()
		//{
		//	var scenePicker = new ScenePickerDialog(_difficulty);
		//	if (await scenePicker.ShowAsync() == ContentDialogResult.Primary)
		//	{
		//		var scene = scenePicker.ViewModel.SelectedScene;
		//		await DrawSceneAsync(scene.Preset);
		//	}
		//}

		//public async Task DrawSceneAsync(ScenePreset scene)
		//{
		//	IsLoading = true;
		//	var huygensBuilder = new HuygensFieldBuilder(RenderingConfiguration.FieldSize, RenderingConfiguration.FieldSize);
		//	huygensBuilder.DrawScene(scene);

		//	var manager = new HuygensManager(huygensBuilder.Build());
		//	await manager.PrecalculateAsync();
		//	_controller.StartSimulation(manager, scene);
		//	IsLoading = false;
		//}
	}
}
