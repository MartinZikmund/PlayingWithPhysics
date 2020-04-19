using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Physics.SelfStudy.Editor.Infrastructure.Pickers
{
    public static class PickProjectFileDialog
    {
        private const string FileType = ".json";
        private const string SettingsIdentifier = "projectfile";

        public static async Task<StorageFile> OpenAsync()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(FileType);
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.SettingsIdentifier = SettingsIdentifier;
            return await picker.PickSingleFileAsync();
        }        

        public static async Task<StorageFile> SaveAsAsync()
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Project JSON file", new[] { FileType });
            picker.DefaultFileExtension = FileType;            
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            return await picker.PickSaveFileAsync();
        }
    }
}
