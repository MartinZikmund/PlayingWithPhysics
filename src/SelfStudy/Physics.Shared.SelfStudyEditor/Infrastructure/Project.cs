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
using System.Linq;
using CSharpMath.Rendering.FrontEnd;

namespace Physics.SelfStudy.Editor.Infrastructure
{
    public class Project : ViewModelBase
    {
        public StorageFile BackingFile { get; set; }

        public ObservableCollection<ChapterViewModel> Chapters { get; } = new ObservableCollection<ChapterViewModel>();

        internal void MoveUp(ChapterViewModel chapterViewModel)
        {
            var index = Chapters.IndexOf(chapterViewModel);
            if (index > 0 && index != -1)
            {
                Chapters.RemoveAt(index);
                Chapters.Insert(index - 1, chapterViewModel);
            }
        }


        internal void MoveUp(ContentViewModel contentViewModel)
        {
            if (SelectedChapter != null && contentViewModel != null)
            {
                var index = SelectedChapter.Contents.IndexOf(contentViewModel);
                if (index > 0 && index != -1)
                {
                    SelectedChapter.Contents.RemoveAt(index);
                    SelectedChapter.Contents.Insert(index - 1, contentViewModel);
                }
            }
        }

        internal void MoveDown(ChapterViewModel chapterViewModel)
        {
            var index = Chapters.IndexOf(chapterViewModel);
            if (index < Chapters.Count - 1 && index != -1)
            {
                Chapters.RemoveAt(index);
                Chapters.Insert(index + 1, chapterViewModel);
            }
        }

        internal void MoveDown(ContentViewModel contentViewModel)
        {
            if (SelectedChapter != null && contentViewModel != null)
            {
                var index = SelectedChapter.Contents.IndexOf(contentViewModel);
                if (index < SelectedChapter.Contents.Count - 1 && index != -1)
                {
                    SelectedChapter.Contents.RemoveAt(index);
                    SelectedChapter.Contents.Insert(index + 1, contentViewModel);
                }
            }
        }

        internal void Delete(ChapterViewModel chapterViewModel)
        {
            Chapters.Remove(chapterViewModel);
            if (SelectedChapter == chapterViewModel)
            {
                SelectedChapter = null;
            }
        }

        internal void Delete(ContentViewModel contentViewModel)
        {
            if (SelectedChapter != null && contentViewModel != null)
            {
                SelectedChapter.Contents.Remove(contentViewModel);
                if (SelectedContent == contentViewModel)
                {
                    SelectedContent = null;
                }
            }
        }

        public ChapterViewModel SelectedChapter { get; set; }

        private void OnSelectedChapterChanged() => SelectedContent = null;

        public ContentViewModel SelectedContent { get; set; }

        public bool IsChapterSelected => SelectedChapter != null;

        public bool NoContentSelected => SelectedContent == null;

        public bool IsContentSelected => SelectedContent != null;

        public bool IsDirty { get; private set; } = true;

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

        public ICommand AddChapterCommand => GetOrCreateCommand(() => Chapters.Add(new ChapterViewModel(this, new Chapter() { Title = "Untitled" })));

        public ICommand AddSectionCommand => GetOrCreateCommand((string type) =>
        {
            if (SelectedChapter == null) return;

            IContent content = null;
            var enumValue = Enum.Parse<ContentType>(type, true);
            switch (enumValue)
            {
                case ContentType.Text:
                    content = new ParagraphContent() { Title = "Untitled" };
                    break;
                case ContentType.AdditionalResources:
                    content = new AdditionalResourcesContent() { Title = "Untitled" };
                    break;
                case ContentType.KnowledgeCheck:
                    content = new KnowledgeCheckContent() { Title = "Untitled" };
                    break;
                case ContentType.Image:
                    content = new ImageContent() { ImageName = "" };
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
            SelectedChapter.Contents.Add(new ContentViewModel(this, content));
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
            var contents = SerializeProject();
            await FileIO.WriteTextAsync(BackingFile, contents);
        }

        public async Task DiscardAsync()
        {

        }

        public async Task<string> SaveTempAsync()
        {
            var contents = SerializeProject();
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
                project.Chapters.Add(new ChapterViewModel(project, content));
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
            var imageFolderPath = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "TestImages"); 
            await StudyModeManager.OpenStudyModeAsync(uri, imageFolderPath);
        }

        private string SerializeProject()
        {
            var chapters = Chapters.Select(c =>
            {
                c.Chapter.Contents = c.Contents.Select(content => content.Content).ToArray();
                return c.Chapter;
            }).ToArray();            
            return JsonConvert.SerializeObject(chapters);
        }
    }
}
