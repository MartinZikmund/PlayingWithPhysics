using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Physics.SelfStudy.Html
{
    public static class HtmlHelpers
    {
        public static string LayoutFormatString { get; private set; }

        public static async Task InitializeAsync()
        {
            var layoutFile = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Physics.SelfStudy/Assets/Html/Layout.html"));
            LayoutFormatString = await FileIO.ReadTextAsync(layoutFile);
        }
    }
}
