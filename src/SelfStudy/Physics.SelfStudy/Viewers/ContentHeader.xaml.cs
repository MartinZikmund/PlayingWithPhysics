using Physics.SelfStudy.Models;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Physics.SelfStudy.Viewers
{
    public sealed partial class ContentHeader : UserControl
    {
        public ContentHeader()
        {
            this.InitializeComponent();
        }



        public IContent Content
        {
            get { return (IContent)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(IContent), typeof(ContentHeader), new PropertyMetadata(null, ContentChanged));

        private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var header = (ContentHeader)d;
            if (e.NewValue is IContent content && content.Type != Shared.SelfStudy.Models.ContentType.Text)
            {
                header.Show();
                var typeString = content.Type.ToString().ToLowerInvariant();
                header.IconImage.Source = new BitmapImage(new Uri($"ms-appx:///Physics.SelfStudy/Assets/Icons/{typeString}.png"));
                header.Title.Text = content.Title;
            }
            else
            {
                header.Hide();
            }
        }

        private void Hide()
        {
            Root.Visibility = Visibility.Collapsed;
        }

        private void Show()
        {
            Root.Visibility = Visibility.Visible;
        }
    }
}
