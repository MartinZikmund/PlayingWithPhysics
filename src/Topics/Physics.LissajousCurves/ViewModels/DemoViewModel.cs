using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.LissajousCurves.Logic;
using Physics.LissajousCurves.Rendering;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.Views.Interactions;

namespace Physics.LissajousCurves.ViewModels
{
	public class DemoViewModel : MainViewModel, IReceiveController<LissajousCurvesController>
	{
		private LissajousCurvesController _controller;
		public ObservableCollection<DemoItemViewModel> DemoOscillationsFrequency { get; set; } = new ObservableCollection<DemoItemViewModel>()
		{
			new DemoItemViewModel("0,5 Hz", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123"))),
			new DemoItemViewModel("1 Hz", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("1,0 Hz", 1, 1f, 0, "#FFE81123"))),
			new DemoItemViewModel("1,5 Hz", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("1,5 Hz", 1, 1.5f, 0, "#FFE81123"))),
			new DemoItemViewModel("2 Hz", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("2 Hz", 1, 2f, 0, "#FFE81123"))),
			new DemoItemViewModel("0,75 Hz", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,75 Hz", 1, 0.75f, 0, "#FFE81123")))
		};

		public ObservableCollection<DemoItemViewModel> DemoOscillationsAmplitude { get; set; } = new ObservableCollection<DemoItemViewModel>()
		{
			new DemoItemViewModel("1 m - 0,5 m", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 0.3f, 0.5f, 90, "#FFE81123"))),
			new DemoItemViewModel("0,3 m - 0,8 m", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 0.3f, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 0.8f, 0.5f, 90f, "#FFE81123")))
		};

		public ObservableCollection<DemoItemViewModel> DemoOscillationsPhase { get; set; } = new ObservableCollection<DemoItemViewModel>()
		{
			new DemoItemViewModel("0°", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123"))),
			new DemoItemViewModel("30°", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 30, "#FFE81123"))),
			new DemoItemViewModel("60°", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 60, "#FFE81123"))),
			new DemoItemViewModel("90°", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 90, "#FFE81123"))),
			new DemoItemViewModel("150°", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 150, "#FFE81123"))),
			new DemoItemViewModel("210°", new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 0, "#FFE81123")), new OscillationInfoViewModel(new OscillationInfo("0,5 Hz", 1, 0.5f, 210, "#FFE81123")))
		};

		private DemoItemViewModel _currentOscillation;
		public DemoItemViewModel CurrentOscillation
		{
			get
			{
				return _currentOscillation;
			}
			set
			{
				if (_currentOscillation != value)
				{
					_currentOscillation = value;
					ChangeOscillationAsync(value);
				}
			}
		}

		private async Task ChangeOscillationAsync(DemoItemViewModel arg)
		{
			await StartSimulationAsync();
		}

		public void SetController(LissajousCurvesController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
			CurrentOscillation = DemoOscillationsFrequency[0];
			_controller.SetActiveOscillations(CurrentOscillation.X.OscillationInfo, CurrentOscillation.Y.OscillationInfo);
			_controller.StartSimulation();
			SimulationPlayback.PlaybackSpeed = 0.5f;
		}

		private async Task StartSimulationAsync()
		{
			if (_controller == null)
			{
				return;
			}
			await _controller.RunOnGameLoopAsync(() =>
			{
				SimulationPlayback.Play();
				_controller.SetActiveOscillations(CurrentOscillation.X.OscillationInfo, CurrentOscillation.Y.OscillationInfo);
				_controller.StartSimulation();
			});
		}

		public DemoViewModel(IContentDialogHelper contentDialogHelper) : base(contentDialogHelper)
		{
		}
	}
}
