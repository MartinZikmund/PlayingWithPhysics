using Physics.SelfStudy.Html;
using Physics.SelfStudy.Models.Contents.Abstract;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Viewers
{
    public sealed partial class HtmlContentViewer : UserControl
    {
        public HtmlContentViewer()
        {
            this.InitializeComponent();            
        }

        public HtmlContent HtmlContent
        {
            get { return (HtmlContent)GetValue(HtmlContentProperty); }
            set { SetValue(HtmlContentProperty, value); }
        }

        public static readonly DependencyProperty HtmlContentProperty =
            DependencyProperty.Register("HtmlContent", typeof(HtmlContent), typeof(HtmlContentViewer), new PropertyMetadata(null, OnHtmlContentChanged));

        private static void OnHtmlContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (HtmlContentViewer)d;
            if (e.NewValue is HtmlContent htmlContent)
            {
                viewer.WebView.NavigateToString(string.Format(HtmlHelpers.LayoutFormatString, htmlContent.Html));
            }
        }
    }
}
