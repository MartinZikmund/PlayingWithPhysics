using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.StationaryWaves.Dialogs;
using Physics.StationaryWaves.Logic;
using Physics.StationaryWaves.Rendering;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Physics.StationaryWaves.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<StationaryWavesCanvasController>
	{
		private DifficultyOption _difficulty;
		private StationaryWavesCanvasController _controller;
		private readonly IContentDialogHelper _contentDialogHelper;
		internal DifficultyOption Difficulty { get; private set; }

		public override void Prepare(SimulationNavigationModel parameter)
		{
			//TODO: This DifficultyOption inject does not work
			_difficulty = parameter.Difficulty;
		}

		public MainViewModel(IContentDialogHelper contentDialogHelper)
		{
			_contentDialogHelper = contentDialogHelper;
		}

		public async void AddWave()
		{
			{
				try
				{
					var resourceLoader = ResourceLoader.GetForCurrentView();
					AddOrUpdateWaveViewModel dialogViewModel = new AddOrUpdateWaveViewModel(Difficulty);
					Debug.WriteLine($"Difficulty: {Difficulty}");
					//if (Wave1 == null || Wave2 == null)
					//{
					//	dialogViewModel = new AddOrUpdateWaveViewModel(Difficulty);
					//}
					//else
					//{
					//	dialogViewModel = new AddOrUpdateWaveViewModel(Wave1.WaveInfo, Wave2.WaveInfo, SourceDistance, Difficulty);
					//}
					var dialog = new AddOrUpdateWave();
					dialog.DataContext = dialogViewModel;
					var result = await dialog.ShowAsync();
					if (result == ContentDialogResult.Primary)
					{
						Waves.Add(new WaveInfoViewModel(new WaveInfo("Test wave", dialogViewModel.Amplitude, "#FF0000")));
						foreach (var item in Waves)
							Debug.WriteLine(item.WaveInfo.Amplitude);
						//Waves.Clear();
						//Waves.Add(new WaveInfoViewModel(dialogViewModel.Result[0].Result));
						//Waves.Add(new WaveInfoViewModel(dialogViewModel.Result[1].Result));
						//SourceDistance = dialogViewModel.SourceDistance;
						//await RaisePropertyChanged(nameof(AreWavesConfigured));
						//await StartSimulationAsync();
					}
				}
				catch (Exception ex)
				{
					await _contentDialogHelper.ShowAsync("Error", ex.ToString());
				}
			}
		}

		public ObservableCollection<WaveInfoViewModel> Waves { get; private set; } = new ObservableCollection<WaveInfoViewModel>();

		public void SetController(StationaryWavesCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}
	}
}
