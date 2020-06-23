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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.Editor.Editors
{
    public sealed partial class KnowledgeCheckEditor : UserControl, INotifyPropertyChanged
    {
        public KnowledgeCheckEditor()
        {
            this.InitializeComponent();
        }

        public MultipleChoiceQuestionContent Question
        {
            get { return (MultipleChoiceQuestionContent)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register(nameof(Question), typeof(MultipleChoiceQuestionContent), typeof(KnowledgeCheckEditor), new PropertyMetadata(null, QuestionChanged));

        private static void QuestionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public string NewItemText { get; set; }

        public int SelectedOptionIndex { get; set; }

        public void AddOption()
        {
            Question.Options.Add(NewItemText);
            Question.ForceUpdate();
            NewItemText = "";
        }

        public void DeleteSelectedOption()
        {
            if (SelectedOptionIndex >= 0 && SelectedOptionIndex < Question.Options.Count)
            {
                Question.Options.RemoveAt(SelectedOptionIndex);
                Question.ForceUpdate();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
