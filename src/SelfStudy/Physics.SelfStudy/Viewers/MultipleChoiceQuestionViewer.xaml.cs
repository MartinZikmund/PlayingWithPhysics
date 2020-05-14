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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.SelfStudy.Viewers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MultipleChoiceQuestionViewer : Page
    {
        public MultipleChoiceQuestionViewer()
        {
            this.InitializeComponent();
        }

        public MultipleChoiceQuestionContent Question
        {
            get { return (MultipleChoiceQuestionContent)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register(nameof(Question), typeof(MultipleChoiceQuestionContent), typeof(MultipleChoiceQuestionViewer), new PropertyMetadata(null, OnQuestionChanged));

        public MultipleChoiceQuestionViewerViewModel Model
        {
            get { return (MultipleChoiceQuestionViewerViewModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(nameof(Model), typeof(MultipleChoiceQuestionViewerViewModel), typeof(MultipleChoiceQuestionViewer), new PropertyMetadata(0));

        private static void OnQuestionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (MultipleChoiceQuestionViewer)d;
            if (e.NewValue is MultipleChoiceQuestionContent question)
            {
                viewer.Model = new MultipleChoiceQuestionViewerViewModel(question);   
            }
        }
    }
}
