using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Physics.SelfStudy.Editor.Infrastructure.Pickers
{
    public static class PickProjectFileDialog
    {
        private const string FileType = ".json";

        public static Task<StorageFile> OpenAsync()
        {
            return null;
        }        

        public static Task<StorageFile> SaveAsAsync()
        {
            return null;
        }
    }
}
