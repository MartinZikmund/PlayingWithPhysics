using Physics.SelfStudy.Editor.Infrastructure.Pickers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Editor.Infrastructure
{
    public static class Workspace
    {
        public static Project CurrentProject { get; private set; } = Project.CreateNew();

        public static async Task NewAsync()
        {
            if (!await TryCloseCurrentProjectAsync()) return;

            CurrentProject = Project.CreateNew();
        }

        public static async Task OpenAsync()
        {
            if (!await TryCloseCurrentProjectAsync()) return;

            if (await PickProjectFileDialog.OpenAsync() is StorageFile newProject)
            {
                CurrentProject = await Project.LoadAsync(newProject);
            }
        }

        public static async Task<bool> TryCloseCurrentProjectAsync()
        {
            if (CurrentProject.IsDirty)
            {
                // prompt user to save changes or cancel
                var dialog = new ContentDialog();
                dialog.Title = "Do you want to save changes?";
                dialog.Content = $"There are unsaved changes in project {CurrentProject.Name}";
                dialog.DefaultButton = ContentDialogButton.Close;
                dialog.CloseButtonText = "Cancel";
                dialog.SecondaryButtonText = "No";
                dialog.PrimaryButtonText = "Yes";
                dialog.IsPrimaryButtonEnabled = true;
                dialog.IsSecondaryButtonEnabled = true;
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.None)
                {
                    return false;
                }
                else if (result == ContentDialogResult.Primary)
                {
                    // save changes
                    await CurrentProject.SaveAsync();
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    // discard changes
                    await CurrentProject.DiscardAsync();
                }
            }
            return true;
        }
    }
}
