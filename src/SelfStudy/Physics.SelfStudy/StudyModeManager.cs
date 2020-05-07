using Newtonsoft.Json;
using Physics.SelfStudy.Html;
using Physics.SelfStudy.Json;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using Physics.SelfStudy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.SelfStudy
{
    public static class StudyModeManager
    {
        public static async Task InitializeAsync()
        {
            await HtmlHelpers.InitializeAsync();
        }

        public static async Task OpenStudyModeAsync(Uri backingFileUri)
        {
            await InitializeAsync();

            var resourceLoader = new ResourceLoader();

            // open new application view
            var newWindow = await AppWindow.TryCreateAsync();
            var appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(SelfStudyView), backingFileUri);
            
            // Attach the XAML content to the window.
            ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
            //newWindow.Closed += NewWindow_Closed;
            newWindow.Title = resourceLoader.GetString("AppName");

            //newWindow.TitleBar.BackgroundColor = (Application;
            //newWindow.TitleBar.ForegroundColor = Colors.White;
            //newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            //newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            //newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
            //newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
            //newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            //newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.RequestSize(new Size(600, 400));
            var shown = await newWindow.TryShowAsync();
        }

        public static async Task<Chapter[]> ReadDefinitionFileAsync(StorageFile file)
        {
            var json = await FileIO.ReadTextAsync(file);
            var options = new JsonSerializerSettings()
            {
                Converters =
                {
                    new ContentConverter()
                }
            };
            return JsonConvert.DeserializeObject<Chapter[]>(json, options);
        }
    }
}
