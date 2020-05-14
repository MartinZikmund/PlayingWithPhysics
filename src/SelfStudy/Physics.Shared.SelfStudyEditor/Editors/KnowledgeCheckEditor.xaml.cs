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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.Editor.Editors
{
    public sealed partial class KnowledgeCheckEditor : UserControl, INotifyPropertyChanged
    {
        private string _layoutContents;

        public KnowledgeCheckEditor()
        {
            this.InitializeComponent();
            this.DataContextChanged += HtmlEditor_DataContextChanged;
        }

        public KnowledgeCheckContent ViewModel { get; private set; }

        public string NewItemText { get; set; }

        public void AddItem()
        {
            ViewModel.Options.Add(NewItemText);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void HtmlEditor_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ViewModel = args.NewValue as KnowledgeCheckContent;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
        }
    }
}
