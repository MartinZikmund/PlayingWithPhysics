using Physics.SelfStudy.Models.Contents.Abstract;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.Viewers
{
    public sealed partial class LaTeXViewer : UserControl
    {
        private bool _initialized = false;

        public LaTeXViewer()
        {
            this.InitializeComponent();
        }

        public TextContent ChapterContent
        {
            get { return (TextContent)GetValue(ChapterContentProperty); }
            set { SetValue(ChapterContentProperty, value); }
        }

        public static readonly DependencyProperty ChapterContentProperty =
            DependencyProperty.Register(nameof(ChapterContent), typeof(TextContent), typeof(LaTeXViewer), new PropertyMetadata(null, OnChapterContentChanged));

        private static void OnChapterContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is TextContent content)
            {
                ((LaTeXViewer)d).Display.LaTeX = content.Text;
            }
        }
    }
}
