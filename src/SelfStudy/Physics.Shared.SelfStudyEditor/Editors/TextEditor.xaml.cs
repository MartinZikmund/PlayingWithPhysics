using Physics.SelfStudy.Html;
using Physics.SelfStudy.Models.Contents.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class TextEditor : UserControl, INotifyPropertyChanged
    {
        public TextEditor()
        {
            this.InitializeComponent();
            this.DataContextChanged += TextEditor_DataContextChanged;
        }

        public TextContent ViewModel { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void TextEditor_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ViewModel = args.NewValue as TextContent;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
        }
    }
}
