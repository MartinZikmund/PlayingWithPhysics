using Microsoft.UI.Xaml.Controls;
using Physics.SelfStudy.Editor.Infrastructure.Pickers;
using Physics.SelfStudy.Editor.ViewModels;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace Physics.SelfStudy.Editor.Infrastructure
{
    public class Project : ViewModelBase
    {
        private StorageFile _backingFile = null;

        public string Name { get; set; }

        public ObservableCollection<IContent> Tree { get; } = new ObservableCollection<IContent>();

        public bool IsDirty { get; private set; } = false;

        private Project()
        {
        }

        public ICommand AddSectionCommand => GetOrCreateCommand(() => { Tree.Add(new ChapterContent() { Title = "Ahoj" }); });

        public async Task SaveAsync()
        {

        }

        public async Task SaveAsAsync()
        {
            var file = await PickProjectFileDialog.SaveAsAsync();
            if (file != null)
            {

            }
        }

        public async Task DiscardAsync()
        {

        }

        public async Task SaveTempAsync()
        {

        }

        public static Project CreateNew() => new Project();

        public static async Task<Project> LoadAsync(StorageFile newProject)
        {
            // TODO:
            return new Project();
        }
    }
}
