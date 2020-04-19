using System.Windows.Input;
using Windows.Storage;

namespace Physics.SelfStudy.Editor.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private StorageFile _backingFile = null;

        public ICommand NewFileCommand => GetOrCreateCommand(NewFile);

        private void NewFile()
        {
            
        }

        public ICommand SaveFileCommand => GetOrCreateCommand(SaveFile);

        private void SaveFile()
        {
            
        }

        public ICommand OpenFileCommand => GetOrCreateCommand(OpenFile);

        public void OpenFile()
        {

        }

        public ICommand SaveAsFileCommand => GetOrCreateCommand(SaveAsFile);

        private void SaveAsFile()
        {
            
        }
    }
}
