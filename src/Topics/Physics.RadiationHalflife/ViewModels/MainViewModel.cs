using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.RadiationHalflife.Dialogs;
using Physics.RadiationHalflife.Logic;
using Physics.RadiationHalflife.Rendering;
using Physics.RadiationHalflife.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.Shared.UI.Views.Interactions;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.RadiationHalflife.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<DifficultyNavigationModel>,
		IReceiveController<RadiationHalflifeController>
	{
		private RadiationHalflifeController _controller;
		private readonly IContentDialogHelper _contentDialogHelper;

		internal DifficultyOption Difficulty { get; private set; }

		public MainViewModel(IContentDialogHelper contentDialogHelper)
		{
			_contentDialogHelper = contentDialogHelper;
			_selectedNucleoid = Nucleoids[2];
			_selectedCustomUnit = CustomUnits[0];
		}

		public ICommand EditOscillationCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(EditOscillationAsync);

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand<OscillationInfoViewModel>(ShowValuesTableAsync);

		public ICommand ShowCompoundOscillationValuesTableCommand => GetOrCreateAsyncCommand(ShowCompoundOscillationValuesTableAsync);

		public string TimeElapsed => _controller?.SimulationTime.TotalTime.TotalSeconds.ToString("0.00") ?? "0.00";

		public override void Prepare(DifficultyNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

		public void SetController(RadiationHalflifeController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
			//_controller.SetActiveOscillations(HorizontalOscillation.OscillationInfo, VerticalOscillation.OscillationInfo);
			//_controller.StartSimulation();
			SimulationPlayback.PlaybackSpeed = 0.5f;
		}

		//public ObservableCollection<OscillationInfoViewModel> Oscillations { get; } = new ObservableCollection<OscillationInfoViewModel>();

		public async void AddOscillation()
		{
			//try
			//{
			//	var resourceLoader = ResourceLoader.GetForCurrentView();
			//	var dialogViewModel = new AddOrUpdateOscillationViewModel(Difficulty, Oscillations.Select(m => m.Label).ToArray());
			//	var dialog = new AddOrUpdateOscillationDialog();
			//	dialog.DataContext = dialogViewModel;
			//	var result = await dialog.ShowAsync();
			//	if (result == ContentDialogResult.Primary)
			//	{
			//		Oscillations.Add(new OscillationInfoViewModel(dialogViewModel.Result));
			//		//TODO:
			//		await StartSimulationAsync();
			//	}
			//}
			//catch (Exception ex)
			//{
			//	await _contentDialogHelper.ShowAsync("Error", ex.ToString());
			//}
		}


		private async Task EditOscillationAsync(OscillationInfoViewModel arg)
		{
			//var dialogViewModel = new AddOrUpdateOscillationViewModel(arg.OscillationInfo, Difficulty, Oscillations.Select(m => m.Label).ToArray());
			//var dialog = new AddOrUpdateOscillationDialog(dialogViewModel);
			//var result = await dialog.ShowAsync();
			//if (result == ContentDialogResult.Primary)
			//{
			//	arg.OscillationInfo = dialogViewModel.Result;
			//	await StartSimulationAsync();
			//	//UpdateMotionAppWindow(arg);
			//}
		}

		private async Task DuplicateOscillationAsync(OscillationInfoViewModel arg)
		{
			//var duplicateMotion = arg.OscillationInfo.Clone();
			//duplicateMotion.Label =
			//	$"{duplicateMotion.Label} ({Localizer.Instance.GetString("Copy")})";
			//var dialogViewModel = new AddOrUpdateOscillationViewModel(duplicateMotion, Difficulty, Oscillations.Select(m => m.Label).ToArray());
			//var dialog = new AddOrUpdateOscillationDialog(dialogViewModel);
			//var result = await dialog.ShowAsync();
			//if (result == ContentDialogResult.Primary)
			//{
			//	Oscillations.Add(new OscillationInfoViewModel(dialogViewModel.Result));
			//	await StartSimulationAsync();
			//}
		}

		private async Task DeleteTrajectoryAsync(OscillationInfoViewModel arg)
		{
			//Oscillations.Remove(arg);
			//TODO:
			//await CloseAppViewForMotionAsync(arg);
			//await StartSimulationAsync();
		}

		private async Task ShowValuesTableAsync(OscillationInfoViewModel viewModel)
		{
			//var physicsService = new OscillationPhysicsService(viewModel.OscillationInfo);
			//await ShowValuesTableAsync(physicsService, viewModel.Label);
		}

		private async Task ShowCompoundOscillationValuesTableAsync()
		{
			//var physicsService = new CompoundOscillationsPhysicsService(Oscillations.Where(o => o.IsVisible).Select(o => o.OscillationInfo).ToArray());
			//await ShowValuesTableAsync(physicsService, Localizer.Instance.GetString("CompoundOscillation"));
		}

		private async Task ShowValuesTableAsync()
		{
			//var newWindow = await AppWindow.TryCreateAsync();
			//var appWindowContentFrame = new Frame();
			//appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			//var valuesTableService = new TableService(physicsService);
			//var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, Difficulty);
			//(appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
			//// Attach the XAML content to the window.
			//ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
			//newWindow.Title = title;

			//newWindow.TitleBar.BackgroundColor = (Color)Application.Current.Resources["AppThemeColor"];
			//newWindow.TitleBar.ForegroundColor = Colors.White;
			//newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
			//newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
			//newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
			//newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
			//newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
			//newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
			//newWindow.RequestSize(new Size(480, 300));
			//var shown = await newWindow.TryShowAsync();
		}

		private async Task StartSimulationAsync(AnimationInfo animationInfo)
		{
			if (_controller == null)
			{
				return;
			}
			await _controller.RunOnGameLoopAsync(() =>
			{
				SimulationPlayback.Play();
				_controller.SetAnimation(new PhysicsService(animationInfo));
				_controller.StartSimulation();
			});
		}

		public Visibility CanvasVisibility => (SelectedVariant == 4) ? Visibility.Visible : Visibility.Collapsed;
		public Visibility AnimationPanelVisibility => (SelectedVariant != 4) ? Visibility.Visible : Visibility.Collapsed;

		public List<NucleoidItemViewModel> Nucleoids = new List<NucleoidItemViewModel>()
		{
			new NucleoidItemViewModel("Carbon", 5530),
			new NucleoidItemViewModel("Fluorine", 110),
			new NucleoidItemViewModel("Potassium", 1.27f),
			new NucleoidItemViewModel("Cobalt", 5.3f),
			new NucleoidItemViewModel("Gallium", 68),
			new NucleoidItemViewModel("Technetium",6),
			new NucleoidItemViewModel("Iodine", 8),
			new NucleoidItemViewModel("Xenon", 5.2f),
			new NucleoidItemViewModel("Cesium", 30.2f),
			new NucleoidItemViewModel("Iridium", 74),
			new NucleoidItemViewModel("Radium223", 11.46f),
			new NucleoidItemViewModel("Radium226", 1600),
			new NucleoidItemViewModel("Uranium228", 9.1f),
			new NucleoidItemViewModel("Uranium238", 4.5f),
			new NucleoidItemViewModel("Plutonium", 2400),
			new NucleoidItemViewModel("Americium", 432.6f),
			new NucleoidItemViewModel("Custom", 0)
		};

		private NucleoidItemViewModel _selectedNucleoid;
		public NucleoidItemViewModel SelectedNucleoid
		{
			get
			{
				return _selectedNucleoid;
			}

			set
			{
				_selectedNucleoid = value;
				if (_selectedNucleoid == Nucleoids.Last())
				{
					CustomHalflifeInputsVisibility = Visibility.Visible;
				}
				else
				{
					CustomHalflifeInputsVisibility = Visibility.Collapsed;
					StartSimulationAsync(new AnimationInfo(SelectedNucleoid.ChemicalElement, ParticlesCount, SelectedNucleoid.Halflife));
				}
			}
		}

		public Visibility CustomHalflifeInputsVisibility { get; set; } = Visibility.Collapsed;

		public Visibility AdvancedDifficulty => (Difficulty == DifficultyOption.Advanced) ? Visibility.Visible : Visibility.Collapsed;

		private float _customHalflife = 1f;

		public float CustomHalflife
		{
			get
			{
				return _customHalflife;
			}

			set
			{
				_customHalflife = value;
				StartSimulationAsync(new AnimationInfo(SelectedNucleoid.ChemicalElement, ParticlesCount, _customHalflife, SelectedCustomUnit));
			}
		}

		public List<string> CustomUnits { get; set; } = new List<string>
		{
			"Second",
			"Minute",
			"Hour",
			"Day",
			"Month",
			"Year"
		};

		private string _selectedCustomUnit;
		public string SelectedCustomUnit
		{
			get
			{
				return _selectedCustomUnit;
			}

			set
			{
				_selectedCustomUnit = value;
				StartSimulationAsync(new AnimationInfo(SelectedNucleoid.ChemicalElement, ParticlesCount, CustomHalflife, _selectedCustomUnit));
			}
		}

		private int _selectedVariant = 0;
		public int SelectedVariant
		{
			get
			{
				return _selectedVariant;
			}
			set
			{
				_selectedVariant = value;
				//Set new simulation GIF
				if (value == 4)
				{
					StartSimulationAsync(new AnimationInfo(SelectedNucleoid.ChemicalElement, ParticlesCount, SelectedNucleoid.Halflife));
				}
			}
		}

		public Visibility LawOfRadioactiveDecaySelection => (SelectedVariant == 4) ? Visibility.Visible : Visibility.Collapsed;

		public Visibility ParticleInputVisibility => (SelectedVariant == 4 && Difficulty == DifficultyOption.Advanced) ? Visibility.Visible : Visibility.Collapsed;
		private int _particlesCount = 100;
		public int ParticlesCount
		{
			get
			{
				return _particlesCount;
			}

			set
			{
				_particlesCount = value;
				if (SelectedNucleoid == Nucleoids.Last())
				{
					StartSimulationAsync(new AnimationInfo(SelectedNucleoid.ChemicalElement, ParticlesCount, CustomHalflife));
				}
				else
				{
					StartSimulationAsync(new AnimationInfo(SelectedNucleoid.ChemicalElement, ParticlesCount, SelectedNucleoid.Halflife));
				}
			}
		}
	}
}
