using Physics.SelfStudy.Editor.Infrastructure;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Editor.ViewModels
{
    public class ChapterViewModel
    {
        private readonly Project _project;

        public ChapterViewModel(Project project, Chapter chapter)
        {
            _project = project;
            Chapter = chapter;
            Contents = new ObservableCollection<ContentViewModel>(Chapter.Contents.Select(c=>new ContentViewModel(project, c)));
        }

        public Chapter Chapter { get; }

        public ObservableCollection<ContentViewModel> Contents { get; } = new ObservableCollection<ContentViewModel>();

        public void MoveUp() => _project.MoveUp(this);

        public void MoveDown() => _project.MoveDown(this);

        public async void Delete()
        {
            var confirm = new ContentDialog();
            confirm.Title = "Are you sure?";
            confirm.Content = "Are you sure you want to delete this chapter?";
            confirm.PrimaryButtonText = "Yes";
            confirm.SecondaryButtonText = "Cancel";
            confirm.DefaultButton = ContentDialogButton.Secondary;
            var result = await confirm.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                _project.Delete(this);
            }
        }
    }
}
