using System;
using System.Collections.ObjectModel;
using System.Linq;
using Physics.CompoundOscillations.Dialogs;
using Physics.CompoundOscillations.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.Shared.UI.Views.Interactions;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Physics.CompoundOscillations.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<DifficultyNavigationModel>,
		IReceiveController<CompoundOscillationsController>
	{
		private CompoundOscillationsController _controller;
		private readonly IContentDialogHelper _contentDialogHelper;

		internal DifficultyOption Difficulty { get; private set; }

		public MainViewModel(IContentDialogHelper contentDialogHelper)
		{
			_contentDialogHelper = contentDialogHelper;
		}

		public override void Prepare(DifficultyNavigationModel parameter)
		{
			Difficulty = parameter.Difficulty;
		}

		public void SetController(CompoundOscillationsController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
		}

		public ObservableCollection<OscillationInfoViewModel> Oscillations { get; } = new ObservableCollection<OscillationInfoViewModel>();

		public async void AddOscillation()
		{
			try
			{
				var resourceLoader = ResourceLoader.GetForCurrentView();
				var dialogViewModel = new AddOrUpdateOscillationViewModel(Difficulty, Oscillations.Select(m => m.Label).ToArray());
				var dialog = new AddOrUpdateOscillationDialog();
				dialog.DataContext = dialogViewModel;
				var result = await dialog.ShowAsync();
				if (result == ContentDialogResult.Primary)
				{
					Oscillations.Add(new OscillationInfoViewModel(dialogViewModel.Result));
					//await StartSimulationAsync();
				}
			}
			catch (Exception ex)
			{
				await _contentDialogHelper.ShowAsync("Error", ex.ToString());
			}
		}
	}
}
