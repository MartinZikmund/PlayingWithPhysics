using System;
using System.Collections.ObjectModel;
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
			if (_difficulty == DifficultyOption.Advanced)
			{
				SurfaceTypes.Insert(1, CellState.Wall);
			}
			RaisePropertyChanged(nameof(IsAdvanced));

			DrawingState.PropertyChanged += DrawingState_PropertyChanged;
			DrawingState.IsDrawingChanged += DrawingState_IsDrawingChanged;
		}

		private void DrawingState_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
			RaisePropertyChanged(nameof(ShowDrawingWarning));

		private async void DrawingState_IsDrawingChanged(object sender, EventArgs e)
		{
			if (!DrawingState.IsDrawing)
			{
				// Restart simulation
				await DrawSceneAsync(CurrentPreset);
			}
		}

		public bool IsEasy => _difficulty == DifficultyOption.Easy;

		public bool IsAdvanced => _difficulty == DifficultyOption.Advanced;

		public ScenePreset TemplatePreset { get; set; } = new ScenePreset("Empty");

		public ScenePreset CurrentPreset { get; set; } = new ScenePreset("Empty");

		public bool IsLoading { get; set; }

		public DrawingStateViewModel DrawingState { get; } = new DrawingStateViewModel();

		public ObservableCollection<CellState> SurfaceTypes { get; } = new ObservableCollection<CellState> { CellState.Source };

		public ObservableCollection<ShapeType> ShapeTypes { get; } = new ObservableCollection<ShapeType> { ShapeType.Circle, ShapeType.Square };

		public async void SetController(HuygensPrincipleCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			_controller.SetVariantRenderer(_difficulty == DifficultyOption.Easy ? (IHuygensVariantRenderer)new EasyVariantRenderer(_controller) : new AdvancedVariantRenderer(_controller));
			_controller.SetRenderConfiguration(RenderConfiguration);
			_controller.SetDrawingState(DrawingState);
			SimulationPlayback.SetController(_controller);
			await DrawSceneAsync(CurrentPreset);
		}

		public RenderConfigurationViewModel RenderConfiguration { get; } = new RenderConfigurationViewModel();

		public RenderSettingsViewModel SavedRenderSettings { get; private set; } = new RenderSettingsViewModel();

		public bool ShowDrawingWarning =>
			_difficulty == DifficultyOption.Advanced &&
			DrawingState.Size < SavedRenderSettings.StepRadius &&
			DrawingState.ActiveTool == DrawingTool.Brush &&
			DrawingState.SurfaceType == CellState.Wall;

		internal async void OnSavedRenderSettingsChanged()
		{
			await DrawSceneAsync(CurrentPreset);
			await RaisePropertyChanged(nameof(ShowDrawingWarning));
		}

		public RenderSettingsViewModel UnconfirmedRenderSettings { get; private set; } = new RenderSettingsViewModel();

		public ICommand PickScenePresetCommand => GetOrCreateAsyncCommand(PickSceneAsync);

		public ICommand ResetPresetCommand => GetOrCreateCommand(ResetPreset);

		public ICommand ResetSimulationCommand => GetOrCreateCommand(ResetSimulation);

		public ICommand ConfirmDrawingCommand => GetOrCreateCommand(ConfirmDrawing);

		public ICommand SaveRenderingSettingsCommand => GetOrCreateCommand(SaveRenderingSettings);

		public ICommand SetDefaultRenderSettingsCommand => GetOrCreateCommand(SetDefaultRenderSettings);

		private void SetDefaultRenderSettings()
		{
			UnconfirmedRenderSettings = new RenderSettingsViewModel();
		}

		internal void SaveRenderingSettings()
		{
			SavedRenderSettings = new RenderSettingsViewModel()
			{
				FieldSize = UnconfirmedRenderSettings.FieldSize,
				StepRadius = UnconfirmedRenderSettings.StepRadius
			};
		}

		internal void CancelRenderSettings()
		{
			UnconfirmedRenderSettings = new RenderSettingsViewModel()
			{
				FieldSize = SavedRenderSettings.FieldSize,
				StepRadius = SavedRenderSettings.StepRadius
			};
		}

		private async Task PickSceneAsync()
		{
			var scenePicker = new ScenePickerDialog(_difficulty);
			if (await scenePicker.ShowAsync() == ContentDialogResult.Primary)
			{
				var scene = scenePicker.ViewModel.SelectedScene;
				TemplatePreset = scene.Preset;
				CurrentPreset = scene.Preset.Clone();
				await DrawSceneAsync(CurrentPreset);
			}
		}

		private async void ResetSimulation()
		{
			await DrawSceneAsync(CurrentPreset);
			DrawingState.IsDrawing = false;
		}

		private async void ResetPreset()
		{
			CurrentPreset = TemplatePreset.Clone();
			await DrawSceneAsync(CurrentPreset);
			DrawingState.IsDrawing = false;
		}

		private void ConfirmDrawing()
		{
			CurrentPreset.Redrawn = true;
			DrawingState.IsDrawing = false;
		}

		public async Task DrawSceneAsync(ScenePreset scene)
		{
			var wasPaused = SimulationPlayback.IsPaused;
			if (scene == null)
			{
				return;
			}

			IsLoading = true;
			_controller.SimulationTime.Restart();
			_controller.Pause();
			var huygensBuilder = new HuygensFieldBuilder(SavedRenderSettings.FieldSize, SavedRenderSettings.FieldSize);
			huygensBuilder.DrawScene(scene);

			var manager = new HuygensManager(huygensBuilder.Build(), SavedRenderSettings.StepRadius);
			if (_difficulty == DifficultyOption.Advanced)
			{
				manager.PrecalculateAsync();
			}
			_controller.StartSimulation(manager, scene);

			if (!wasPaused)
			{
				_controller.Play();
			}
			IsLoading = false;
		}

		public void ClearScene()
		{

		}
	}
}
