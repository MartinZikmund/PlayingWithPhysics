using Physics.SelfStudy.Editor.Infrastructure;
using System.Windows.Input;
using Windows.Storage;

namespace Physics.SelfStudy.Editor.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        public Workspace Workspace { get; } = new Workspace();

        public ICommand NewFileCommand => GetOrCreateCommand(async () => await Workspace.NewAsync());

        public ICommand SaveFileCommand => GetOrCreateCommand(async () => await Workspace.CurrentProject.SaveAsync());

        public ICommand OpenFileCommand => GetOrCreateCommand(async () => await Workspace.OpenAsync());

        public ICommand SaveAsFileCommand => GetOrCreateCommand(async () => await Workspace.CurrentProject.SaveAsAsync());
    }
}
