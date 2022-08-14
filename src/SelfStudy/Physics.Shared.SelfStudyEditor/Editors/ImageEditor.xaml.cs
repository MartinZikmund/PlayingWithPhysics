using Physics.SelfStudy.Models.Contents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.SelfStudy.Editor.Editors
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageEditor : Page, INotifyPropertyChanged
    {
        public ImageEditor()
        {
            this.InitializeComponent();
            this.DataContextChanged += ImageEditor_ContentChanged;
        }

        public ImageContent ViewModel { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ImageEditor_ContentChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ViewModel = args.NewValue as ImageContent;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
        }

        private async void OpenTestImagesFolder()
        {
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("TestImages", CreationCollisionOption.OpenIfExists);
            await Launcher.LaunchFolderAsync(folder);
        }
    }
}
