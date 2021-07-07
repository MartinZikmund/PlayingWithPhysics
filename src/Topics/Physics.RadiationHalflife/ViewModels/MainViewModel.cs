using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.RadiationHalflife.Dialogs;
using Physics.RadiationHalflife.Logic;
using Physics.RadiationHalflife.Rendering;
using Physics.RadiationHalflife.ValuesTable;
using Physics.RadiationHalflife.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Rendering.Skia;
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
			_selectedNucleoid = Nucleoids[0];
			_selectedCustomUnit = CustomUnits[0];
			_selectedRadionuclide = Radionuclides[0];	
		}

		public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

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
			SimulationPlayback.PlaybackSpeed = 0.5f;
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
			var newWindow = await AppWindow.TryCreateAsync();
			var appWindowContentFrame = new Frame();
			appWindowContentFrame.Navigate(typeof(ValuesTablePage));

			var valuesTableService = new TableService((PhenomenonVariant)SelectedVariant, _currentPhysicsService);
			var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService, Difficulty, (PhenomenonVariant)SelectedVariant, _currentPhysicsService);
			(appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
			// Attach the XAML content to the window.
			ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
			//newWindow.Title = title;

			newWindow.TitleBar.BackgroundColor = (Color)Application.Current.Resources["AppThemeColor"];
			newWindow.TitleBar.ForegroundColor = Colors.White;
			newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
			newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
			newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
			newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
			newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
			newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
			newWindow.RequestSize(new Size(480, 300));
			var shown = await newWindow.TryShowAsync();
		}

		private async Task StartSimulationAsync(AnimationInfo animationInfo)
		{
			if (_controller == null)
			{
				return;
			}
			await _controller.RunOnGameLoopAsync(() =>
			{
				IVariantRenderer variantRenderer = null;
				switch ((PhenomenonVariant)SelectedVariant)
				{
					case PhenomenonVariant.BeamActivity:
						variantRenderer = new BeamActivityRenderer(_controller);
						break;
					case PhenomenonVariant.RadioactiveLaw:
					default:
						variantRenderer = new RadioactiveLawRenderer(_controller);
						break;
				}
				_controller.SetRenderer(variantRenderer);
				SimulationPlayback.Play();
				var physicsService = GeneratePhysicsService(animationInfo);
				_controller.SetAnimation(physicsService);
				_controller.StartSimulation();
			});
		}

		private PhysicsService _currentPhysicsService;

		public PhysicsService GeneratePhysicsService(AnimationInfo animationInfo)
		{
			switch ((PhenomenonVariant)SelectedVariant)
			{
				case PhenomenonVariant.BeamActivity:
					_currentPhysicsService = new BeamActivityPhysicsService(animationInfo);
					break;
				case PhenomenonVariant.RadioactiveLaw:
				default:
					_currentPhysicsService = new RadioactiveLawPhysicsService(animationInfo);
					break;
			}
			return _currentPhysicsService;
		}

		public Visibility CanvasVisibility => (SelectedVariant == 4 || SelectedVariant == 5) ? Visibility.Visible : Visibility.Collapsed;
		public Visibility AnimationPanelVisibility => (SelectedVariant != 4 && SelectedVariant != 5) ? Visibility.Visible : Visibility.Collapsed;

		public List<RadionuclideViewModel> Radionuclides = new List<RadionuclideViewModel>
		{
			new RadionuclideViewModel("Fluorine", 110, 4, 8),
			new RadionuclideViewModel("Cobalt", 5.3f, 3.7f, 13),
			new RadionuclideViewModel("Gallium", 68, 1, 9),
			new RadionuclideViewModel("Technetium", 6, 6, 8),
			new RadionuclideViewModel("Iodine", 8, 2, 7),
			new RadionuclideViewModel("Xenon", 5.2f, 1, 17),
			new RadionuclideViewModel("Cesium", 30.2f, 8.5f, 16),
			new RadionuclideViewModel("Iridium", 74, 4.4f, 12),
			new RadionuclideViewModel("Radium223", 11.46f, 3.7f, 13),
			new RadionuclideViewModel("Radium226", 1600, 200),
			new RadionuclideViewModel("Uranium238", 4.5f, 12000),
			new RadionuclideViewModel("Americium", 432.6f, 40000)
		};

		public RadionuclideViewModel _selectedRadionuclide;
		public RadionuclideViewModel SelectedRadionuclide
		{
			get
			{
				return _selectedRadionuclide;
			}
			set
			{
				_selectedRadionuclide = value;
				StartSimulationAsync(new BeamActivityAnimationInfo(SelectedRadionuclide.ChemicalElement, SelectedRadionuclide.Halflife, SelectedRadionuclide.ActivityBase, SelectedRadionuclide.ActivityMantissa));
			}
		}

		public List<NucleoidItemViewModel> Nucleoids = new List<NucleoidItemViewModel>()
		{
			new NucleoidItemViewModel("Carbon", 5500),
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
			new NucleoidItemViewModel("Americium", 430f),
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
					StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, SelectedNucleoid.Halflife, ParticlesCount));
				}
			}
		}

		public Visibility BeamActivitySelected => (SelectedVariant == 5) ? Visibility.Visible : Visibility.Collapsed;

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
				StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, _customHalflife, ParticlesCount, SelectedCustomUnit));
			}

		}
		public List<string> CustomUnits { get; set; } = new List<string>
		{
			Localizer.Instance["Halflife_Second"],
			Localizer.Instance["Halflife_Minute"],
			Localizer.Instance["Halflife_Hour"],
			Localizer.Instance["Halflife_Day"],
			Localizer.Instance["Halflife_Month"],
			Localizer.Instance["Halflife_Year"]
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
				StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, CustomHalflife, ParticlesCount, _selectedCustomUnit));
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
					StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, CustomHalflife, ParticlesCount, _selectedCustomUnit));
				}
				else
				{
					StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, SelectedNucleoid.Halflife, ParticlesCount));
				}
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
				//Law of radioactive decay
				if (value == 4)
				{
					if (SelectedNucleoid == Nucleoids.Last())
					{
						CustomHalflifeInputsVisibility = Visibility.Visible;
						StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, CustomHalflife, ParticlesCount, _selectedCustomUnit));
					}
					else
					{
						CustomHalflifeInputsVisibility = Visibility.Collapsed;
						StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, SelectedNucleoid.Halflife, ParticlesCount));
					}
				}
				else if (value == 5) //Beam activity
				{

					CustomHalflifeInputsVisibility = Visibility.Collapsed;
					StartSimulationAsync(new BeamActivityAnimationInfo(SelectedRadionuclide.ChemicalElement, SelectedRadionuclide.Halflife, SelectedRadionuclide.ActivityBase, SelectedRadionuclide.ActivityMantissa));
				}

				if (value != 4)
				{
					CustomHalflifeInputsVisibility = Visibility.Collapsed;
				}
			}
		}
	}
}
