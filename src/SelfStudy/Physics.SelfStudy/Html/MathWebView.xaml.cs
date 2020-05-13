using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.Html
{
    public sealed partial class MathWebView : UserControl
    {
        private bool _initialized;

        public MathWebView()
        {
            this.InitializeComponent();
            Web.SizeChanged += WebView_SizeChanged;
            Web.NavigationCompleted += WebView_NavigationCompleted;
            Web.CanBeScrollAnchor = false;
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

            try
            {
                var heightString = await Web.InvokeScriptAsync("eval", new[] { "getDocHeight().toString()" });
                int height;
                if (int.TryParse(heightString, out height))
                {
                    Wrapper.Height = height;
                    System.Diagnostics.Debug.WriteLine(height);
                }
            }
            catch
            {
                Debug.WriteLine("Exception thrown");
            }
        }

        private void WebView_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }

        public string HtmlContent
        {
            get { return (string)GetValue(HtmlContentProperty); }
            set { SetValue(HtmlContentProperty, value); }
        }

        public static readonly DependencyProperty HtmlContentProperty =
            DependencyProperty.Register(nameof(HtmlContent), typeof(string), typeof(MathWebView), new PropertyMetadata(null, OnHtmlContentChanged));

        private static void OnHtmlContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (MathWebView)d;
            if (e.NewValue is string html)
            {
                viewer._initialized = false;
                viewer.Web.NavigateToString(string.Format(HtmlHelpers.LayoutFormatString, html));
            }
        }
    }
}
