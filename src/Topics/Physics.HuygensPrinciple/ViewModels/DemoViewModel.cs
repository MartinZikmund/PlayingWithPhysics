using System.Linq;
using System.Threading.Tasks;
using Physics.HuygensPrinciple.Logic;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class DemoViewModel : MainViewModel
	{
		public DemoViewModel()
		{
			SelectedDemo = Demos.First();
			SavedRenderSettings = new RenderSettingsViewModel()
			{
				FieldSize = RenderSettingsDefaults.DefaultDemoFieldSize,
				StepRadius = RenderSettingsDefaults.DefaultDemoStepRadius
			};
			UnconfirmedRenderSettings = new RenderSettingsViewModel()
			{
				FieldSize = RenderSettingsDefaults.DefaultDemoFieldSize,
				StepRadius = RenderSettingsDefaults.DefaultDemoStepRadius
			};
		}

		protected override void SetDefaultRenderSettings()
		{
			UnconfirmedRenderSettings = new RenderSettingsViewModel()
			{
				FieldSize = RenderSettingsDefaults.DefaultDemoFieldSize,
				StepRadius = RenderSettingsDefaults.DefaultDemoStepRadius
			};
		}

		public DemoScenarioViewModel[] Demos { get; } = DemoScenarios.Scenarios.Select(s => new DemoScenarioViewModel(s)).ToArray();

		public DemoScenarioViewModel SelectedDemo { get; set; }

		public async void OnSelectedDemoChanged()
		{
			if (SelectedDemo == null)
			{
				return;
			}

			IsLoading = true;
			var preset = await SelectedDemo.ToPresetAsync();
			IsLoading = false;
			TemplatePreset = preset;
			CurrentPreset = preset.Clone();
			await DrawSceneAsync(CurrentPreset);
		}
	}
}
