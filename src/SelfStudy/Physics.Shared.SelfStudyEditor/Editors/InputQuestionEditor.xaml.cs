using Physics.SelfStudy.Models.Contents;
using Physics.SelfStudy.Models.Contents.Abstract;
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

namespace Physics.SelfStudy.Editor.Editors
{
    public sealed partial class InputQuestionEditor : UserControl, INotifyPropertyChanged
    {
        private string _layoutContents;

        public InputQuestionEditor()
        {
            this.InitializeComponent();
            this.DataContextChanged += HtmlEditor_DataContextChanged;
        }

        public InputQuestionContent ViewModel { get; private set; }

        public string NewAnswerText { get; set; }

        public int SelectedOptionIndex { get; set; }

        public void AddAnswer()
        {            
            ViewModel.AllowedAnswers.Add(NewAnswerText);
            ViewModel.ForceUpdate();
            NewAnswerText = "";
        }

        public void DeleteSelectedOption()
        {
            if (SelectedOptionIndex >= 0 && SelectedOptionIndex < ViewModel.AllowedAnswers.Count)
            {
                ViewModel.AllowedAnswers.RemoveAt(SelectedOptionIndex);
                ViewModel.ForceUpdate();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void HtmlEditor_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ViewModel = args.NewValue as InputQuestionContent;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
        }
    }
}
