using Newtonsoft.Json;
using Physics.SelfStudy.Editor.ViewModels;
using Physics.SelfStudy.Json;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Localization;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;

namespace Physics.SelfStudy.ViewModels
{
    public class SelfStudyViewModel : ViewModelBase
    {
        public SelfStudyViewModel()
        {
			Title = Localizer.Instance.GetString("StudyText");
        }
		
		public string Title { get; set; }
		
        public async Task LoadAsync(string argument)
        {
            var paramParts = argument.Split("|");            
            var backingFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(paramParts[0]));
            StudyModeGlobals.ImageFolderPath = paramParts[1];
            var chapters = await StudyModeManager.ReadDefinitionFileAsync(backingFile);
            Chapters = new ObservableCollection<Chapter>(chapters);
            SelectedChapter = Chapters[0];
        }

        public ObservableCollection<Chapter> Chapters { get; set; }

        public Chapter SelectedChapter { get; set; }

        public ElementTheme PaneRequestedTheme => ((Color)Application.Current.Resources["AppThemeColor"]).GetPerceivedLuminance() < 0.5 ?
            ElementTheme.Dark : ElementTheme.Light;
    }
}
