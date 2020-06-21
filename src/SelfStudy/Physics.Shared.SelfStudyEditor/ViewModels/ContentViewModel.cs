using Physics.SelfStudy.Editor.Infrastructure;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Editor.ViewModels
{
    public class ContentViewModel
    {
        private readonly Project _project;

        public ContentViewModel(Project project, IContent content)
        {
            _project = project;
            Content = content;
        }

        public IContent Content { get; }

        public void MoveUp() => _project.MoveUp(this);

        public void MoveDown() => _project.MoveDown(this);

        public async void Delete()
        {
            var confirm = new ContentDialog();
            confirm.Title = "Are you sure?";
            confirm.Content = "Are you sure you want to delete this section?";
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
