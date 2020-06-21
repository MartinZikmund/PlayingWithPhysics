using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.SelfStudy.Viewers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageViewer : UserControl, INotifyPropertyChanged
    {
        public ImageViewer()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public string ImagePath { get; private set; }

        public ImageContent Content
        {
            get { return (ImageContent)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(ImageContent), typeof(ContentHeader), new PropertyMetadata(null, ImageContentChanged));

        private static void ImageContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (ImageViewer)d;
            if ( e.NewValue is ImageContent content )
            {
                viewer.ImagePath = Path.Combine(StudyModeGlobals.ImageFolderPath, content.ImageName);
                viewer.PropertyChanged?.Invoke(viewer, new PropertyChangedEventArgs(nameof(ImagePath)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
