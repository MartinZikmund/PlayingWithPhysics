using System.Threading.Tasks;
using Windows.Storage;

namespace Physics.SelfStudy.Editor.Infrastructure
{
    public class Project
    {
        private StorageFile _backingFile = null;

        public string Name { get; set; }

        public bool IsDirty { get; private set; } = false;

        private Project()
        {
        }

        public async Task LoadAsync()
        {

        }

        public async Task SaveAsync()
        {

        }

        public async Task SaveAsAsync()
        {

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
