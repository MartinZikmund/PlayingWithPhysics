using Newtonsoft.Json;
using Physics.SelfStudy.Json;
using Physics.SelfStudy.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace Physics.SelfStudy.ViewModels
{
    public class SelfStudyViewModel
    {
        public SelfStudyViewModel()
        {            
        }

        public async Task LoadAsync(Uri uri)
        {
            var backingFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var contents = await StudyModeManager.ReadDefinitionFileAsync(backingFile);
            Contents = new ObservableCollection<IContent>(contents);
        }

        public ObservableCollection<IContent> Contents { get; set; }
    }
}
