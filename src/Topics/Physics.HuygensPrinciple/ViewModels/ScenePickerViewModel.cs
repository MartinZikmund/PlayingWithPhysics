using System.Collections.ObjectModel;
using System.Linq;
using MvvmCross.ViewModels;
using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class ScenePickerViewModel : MvxNotifyPropertyChanged
    {
        public ScenePickerViewModel(DifficultyOption difficulty)
		{
			var scenes = difficulty == DifficultyOption.Easy ?
				ScenePresets.EasyVariant : ScenePresets.AdvancedVariant.Union(ScenePresets.EasyVariant);
			Scenes = new ObservableCollection<ScenePresetViewModel>(scenes.Select(s => new ScenePresetViewModel(s)));
			SelectedScene = Scenes[0];
		}

		public ObservableCollection<ScenePresetViewModel> Scenes { get; }

		public ScenePresetViewModel SelectedScene { get; set; }

		public bool IsSceneSelected => SelectedScene != null;
    }
}
