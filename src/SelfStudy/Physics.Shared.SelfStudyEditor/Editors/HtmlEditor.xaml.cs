using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.Editor.Editors
{
    public sealed partial class HtmlEditor : UserControl
    {
        private string _layoutContents;
        private IDisposable _inputChangedDisposable;

        public HtmlEditor()
        {
            this.InitializeComponent();
            this.Loaded += HtmlEditor_Loaded;
            this.Unloaded += HtmlEditor_Unloaded;
        }        

        private async void HtmlEditor_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLayoutAsync();
            var inputChangedObservable = HtmlInput
                .Events()
                .TextChanged
                .Throttle(TimeSpan.FromMilliseconds(500))
                .ObserveOn(Dispatcher)
                .Subscribe(input => UpdatePreview());
        }

        private void HtmlEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            _inputChangedDisposable.Dispose();
        }

        private async Task LoadLayoutAsync()
        {
            var layoutFile = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Physics.SelfStudy/Assets/Html/Layout.html"));
            _layoutContents = await FileIO.ReadTextAsync(layoutFile);
        }

        private void UpdatePreview()
        {
            var input = string.Format(_layoutContents, HtmlInput.Text);
            HtmlPreview.NavigateToString(input);
        }
    }
}
