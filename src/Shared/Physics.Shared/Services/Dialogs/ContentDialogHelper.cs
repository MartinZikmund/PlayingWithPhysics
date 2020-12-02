using System;
using System.Threading.Tasks;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Windows.UI.Xaml.Controls;

namespace Physics.Shared.UI.Services
{
	public class ContentDialogHelper : IContentDialogHelper
    {
        public async Task ShowAsync(string title, string message)
		{
			var contentDialog = new ContentDialog
			{
				Title = title,
				Content = message,
				PrimaryButtonText = Localizer.Instance.GetString("Ok"),
				IsPrimaryButtonEnabled = true
			};
			await contentDialog.ShowAsync();
		}
    }
}
