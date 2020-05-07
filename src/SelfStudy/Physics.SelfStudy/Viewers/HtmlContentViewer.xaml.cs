using Physics.SelfStudy.Html;
using Physics.SelfStudy.Models.Contents.Abstract;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Viewers
{
    public sealed partial class HtmlContentViewer : UserControl
    {
        private bool _initialized = false;

        public HtmlContentViewer()
        {
            this.InitializeComponent();
            WebView.SizeChanged += WebView_SizeChanged;
            WebView.NavigationCompleted += WebView_NavigationCompleted;
            WebView.CanBeScrollAnchor = false;
        }

        private async void WebView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateHeight();
        }

        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _initialized = true;
            UpdateHeight();
        }

        private async void UpdateHeight()
        {
            if (!_initialized) return;

            var heightString = await WebView.InvokeScriptAsync("eval", new[] { "getDocHeight().toString()" });
            int height;
            if (int.TryParse(heightString, out height))
            {
                Wrapper.Height = height;
                System.Diagnostics.Debug.WriteLine(height);
            }
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
                viewer._initialized = false;
                viewer.WebView.NavigateToString(string.Format(HtmlHelpers.LayoutFormatString, htmlContent.Html));
            }
        }

        private void WebView_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
