using System;
using Physics.SelfStudy.Editor.Infrastructure.Pickers;
using Physics.SelfStudy.Editor.ViewModels;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Physics.Shared.SelfStudy.Models;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Editor.Infrastructure
{
    public class Project : ViewModelBase
    {
        public StorageFile BackingFile { get; set; }

        public ObservableCollection<Chapter> Chapters { get; } = new ObservableCollection<Chapter>();

        public Chapter SelectedChapter { get; set; }

        public IContent SelectedContent { get; set; }

        public bool IsDirty { get; private set; } = false;

        private Project()
        {
        }

        public ICommand DeleteSelectedSectionCommand => GetOrCreateCommand(DeleteSelectedSection);

        private void DeleteSelectedSection()
        {
            if (SelectedContent != null)
            {
                SelectedChapter.Contents.Remove(SelectedContent);
            }
        }

        public ICommand AddChapterCommand => GetOrCreateCommand(() => Chapters.Add(new Chapter() { Title = "Untitled" }));

        public ICommand AddSectionCommand => GetOrCreateCommand((string type) =>
        {
            if (SelectedChapter == null) return;

            IContent content = null;
            var enumValue = Enum.Parse<ContentType>(type, true);
            switch (enumValue)
            {
                case ContentType.Text:
                    content = new TextContent() { Title = "Untitled" };
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
            SelectedChapter.Contents.Add(content);
        });

        public async Task SaveAsync()
        {
            if (BackingFile == null)
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
                BackingFile = file;
                await SaveToBackingFileAsync();
            }
        }

        private async Task SaveToBackingFileAsync()
        {
            var contents = JsonConvert.SerializeObject(Chapters);
            await FileIO.WriteTextAsync(BackingFile, contents);
        }

        public async Task DiscardAsync()
        {

        }

        public async Task<string> SaveTempAsync()
        {
            var contents = JsonConvert.SerializeObject(Chapters);
            var fileName = Guid.NewGuid().ToString() + ".json";
            var tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName);
            await FileIO.WriteTextAsync(tempFile, contents);
            return fileName;
        }

        public static Project CreateNew() => new Project();

        public static async Task<Project> LoadAsync(StorageFile projectFile)
        {
            var contents = await StudyModeManager.ReadDefinitionFileAsync(projectFile);

            var project = new Project();
            project.BackingFile = projectFile;
            foreach (var content in contents)
            {
                project.Chapters.Add(content);
            }
            return project;
        }

        public async Task PreviewAsync()
        {
            if (Chapters.Count == 0)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = "Can't display preview";
                dialog.Content = "Your project needs to have at least one chapter to display preview";
                dialog.CloseButtonText = "Close";                
                await dialog.ShowAsync();
                return;
            }
            var tempFileName = await SaveTempAsync();
            var uri = new Uri($"ms-appdata:///temp/{tempFileName}");
            await StudyModeManager.OpenStudyModeAsync(uri);
        }
    }
}
