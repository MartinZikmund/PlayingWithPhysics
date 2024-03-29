﻿using Physics.SelfStudy.Html;
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
        }

        public TextContent HtmlContent
        {
            get { return (TextContent)GetValue(HtmlContentProperty); }
            set { SetValue(HtmlContentProperty, value); }
        }

        public static readonly DependencyProperty HtmlContentProperty =
            DependencyProperty.Register("HtmlContent", typeof(TextContent), typeof(HtmlContentViewer), new PropertyMetadata(null, OnHtmlContentChanged));

        private static void OnHtmlContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is TextContent content)
            {
                ((HtmlContentViewer)d).Web.HtmlContent = content.Text;
            }
        }
    }
}
