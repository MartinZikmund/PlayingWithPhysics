using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.LaTeX
{
    public sealed partial class LaTeXView : UserControl
    {
        private readonly DisplayInformation _displayInformation;

        public LaTeXView()
        {
            this.InitializeComponent();
            _displayInformation = DisplayInformation.GetForCurrentView();            
            //View.Padding = new CSharpMath.Structures.Thickness(4);
            this.Loaded += LaTeXView_Loaded;
            this.Unloaded += LaTeXView_Unloaded;
            UpdateFontSize();
        }

        private void LaTeXView_Loaded(object sender, RoutedEventArgs e)
        {
            _displayInformation.DpiChanged += DisplayInformation_DpiChanged;
        }

        private void LaTeXView_Unloaded(object sender, RoutedEventArgs e)
        {
            _displayInformation.DpiChanged -= DisplayInformation_DpiChanged;
        }

        private void UpdateFontSize()
        {
            var scale = (double)((int)_displayInformation.ResolutionScale / 100.0);
            View.FontSize = 15 * scale;
        }

        private void DisplayInformation_DpiChanged(DisplayInformation sender, object args) => UpdateFontSize();

        public string LaTeX
        {
            get { return (string)GetValue(LaTeXProperty); }
            set { SetValue(LaTeXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LaTeX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LaTeXProperty =
            DependencyProperty.Register("LaTeX", typeof(string), typeof(LaTeXView), new PropertyMetadata(""));
    }
}
