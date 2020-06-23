using Physics.SelfStudy.Editor.Infrastructure;
using Physics.SelfStudy.Models.Contents;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Physics.SelfStudy.Editor.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        public AppShellViewModel()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new System.TimeSpan(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            OnPropertyChanged(nameof(ProjectDirtyMark));
        }

        public Workspace Workspace { get; } = new Workspace();

        public ICommand NewFileCommand => GetOrCreateCommand(async () => await Workspace.NewAsync());

        public ICommand SaveFileCommand => GetOrCreateCommand(async () => await Workspace.CurrentProject.SaveAsync());

        public ICommand OpenFileCommand => GetOrCreateCommand(async () => await Workspace.OpenAsync());

        public ICommand SaveAsFileCommand => GetOrCreateCommand(async () => await Workspace.CurrentProject.SaveAsAsync());

        public ICommand PreviewCommand => GetOrCreateCommand(async () => await Workspace.CurrentProject.PreviewAsync());

        public string ProjectDirtyMark => Workspace?.CurrentProject?.IsDirty == true ? "*" : "";


    }
}
