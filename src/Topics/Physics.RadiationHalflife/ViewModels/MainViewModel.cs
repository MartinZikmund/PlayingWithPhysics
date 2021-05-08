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
			//if ((PhenomenonVariant)SelectedVariant == PhenomenonVariant.RadioactiveLaw)
			//{
			//	preparedPhysicsService = new BeamActivityPhysicsService((BeamActivityAnimationInfo)animationInfo);
			//	_controller = (BeamActivityRenderer)_controller;
			//}
			//else if ((PhenomenonVariant)SelectedVariant == PhenomenonVariant.BeamActivity)
			//{
			//	preparedPhysicsService = new RadioactiveLawPhysicsService((RadioactiveLawAnimationInfo)animationInfo);
			//	_controller = (RadioactiveLawRenderer)_controller;
			//}

			_controller.SetRenderer(variantRenderer);

			SimulationPlayback.SetController(_controller);
			//_controller.SetActiveOscillations(HorizontalOscillation.OscillationInfo, VerticalOscillation.OscillationInfo);
			//_controller.StartSimulation();
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
				var physicsService = GeneratePhysicsService(animationInfo);
				_controller.SetAnimation(physicsService);
				_controller.StartSimulation();
			});
		}

		public PhysicsService GeneratePhysicsService(AnimationInfo animationInfo)
		{
			switch ((PhenomenonVariant)SelectedVariant)
			{
				case PhenomenonVariant.BeamActivity:
					return new BeamActivityPhysicsService(animationInfo);
					break;
				case PhenomenonVariant.RadioactiveLaw:
				default:
					return new RadioactiveLawPhysicsService(animationInfo);
					break;
			}
		}

		public Visibility CanvasVisibility => (SelectedVariant == 4) ? Visibility.Visible : Visibility.Collapsed;
		public Visibility AnimationPanelVisibility => (SelectedVariant != 4) ? Visibility.Visible : Visibility.Collapsed;

		public List<RadionuclideViewModel> Radionuclides = new List<RadionuclideViewModel>
		{
			new RadionuclideViewModel("Fluorine", 110, 4),
			new RadionuclideViewModel("Cobalt", 5.3f, 3.7f),
			new RadionuclideViewModel("Gallium", 68, 1),
			new RadionuclideViewModel("Technetium", 6, 6),
			new RadionuclideViewModel("Iodine", 8, 7),
			new RadionuclideViewModel("Xenon", 5.2f, 1),
			new RadionuclideViewModel("Cesium", 30.2f, 8.5f),
			new RadionuclideViewModel("Iridium", 74, 4.4f),
			new RadionuclideViewModel("Radium223", 11.46f, 3.7f),
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
				StartSimulationAsync(new BeamActivityAnimationInfo(SelectedRadionuclide.ChemicalElement, SelectedRadionuclide.Halflife, SelectedRadionuclide.Activity));
			}
		}

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
					StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, SelectedNucleoid.Halflife, ParticlesCount));
				}
			}
		}

		public bool BeamActivitySelected => (SelectedVariant == 5) ? true : false;

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
				StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, CustomHalflife, ParticlesCount, _selectedCustomUnit));
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

				if (value != 4)
				{
					CustomHalflifeInputsVisibility = Visibility.Collapsed;
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
					StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, CustomHalflife, ParticlesCount, _selectedCustomUnit));
				}
				else
				{
					StartSimulationAsync(new RadioactiveLawAnimationInfo(SelectedNucleoid.ChemicalElement, SelectedNucleoid.Halflife, ParticlesCount));
				}
			}
		}
	}
}
