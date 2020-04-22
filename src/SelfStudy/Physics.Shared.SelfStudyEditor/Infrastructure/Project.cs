using System;
using Physics.SelfStudy.Editor.Infrastructure.Pickers;
using Physics.SelfStudy.Editor.ViewModels;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Physics.Shared.SelfStudy.Models;
using Microsoft.UI.Xaml.Controls;

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

        public ICommand AddRootCommand => GetOrCreateCommand(() => Tree.Add(new ChapterContent() { Title = "Untitled" }));

        private IContent GetSelectedItem()
        {
            return (IContent)AppShell.Instance.TreeView.SelectedItem;
        }

        public IContent SelectedItem { get; set; }

        public void UpdateSelection()
        {
            SelectedItem = GetSelectedItem();
        }

        public ICommand AddSectionCommand => GetOrCreateCommand((string type) =>
        {
            var selectedItem = GetSelectedItem();
            if (!(selectedItem is ChapterContent)) return;
            IContent content = null;
            var enumValue = Enum.Parse<ContentType>(type, true);
            switch (enumValue)
            {
                case ContentType.Chapter:
                    content = new ChapterContent() { Title = "Untitled" };
                    break;
                case ContentType.AdditionalResources:
                    content = new AdditionalResourcesContent() { Title = "Untitled" };
                    break;
                case ContentType.KnowledgeCheck:
                    content = new KnowledgeCheckContent() { Title = "Untitled" };
                    break;
                case ContentType.Literature:
                    content = new LiteratureContent() { Title = "Untitled" };
                    break;
                case ContentType.RealWorld:
                    content = new RealWorldContent() { Title = "Untitled" };
                    break;
                case ContentType.Tasks:
                    content = new TasksContent() { Title = "Untitled" };
                    break;
                case ContentType.ToRemember:
                    content = new ToRememberContent() { Title = "Untitled" };
                    break;
            }
            selectedItem.Subcontents.Add(content);
        });

        public async Task SaveAsync()
        {
            if (_backingFile == null)
            {
                await SaveAsAsync();
                return;
            }
            await SaveToBackingFileAsync();
        }

        public async Task SaveAsAsync()
        {
            var file = await PickProjectFileDialog.SaveAsAsync();
            if (file != null)
            {
                _backingFile = file;
                await SaveToBackingFileAsync();
            }
        }

        private async Task SaveToBackingFileAsync()
        {
            var contents = JsonSerializer.Serialize(Tree);
            await FileIO.WriteTextAsync(_backingFile, contents);
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
