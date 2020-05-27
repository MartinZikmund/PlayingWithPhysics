using Physics.SelfStudy.Models.Contents.Abstract;
using Physics.SelfStudy.Viewers.ViewModels;
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
    public sealed partial class InputQuestionViewer : UserControl
    {
        public InputQuestionViewer()
        {
            this.InitializeComponent();
        }

        public InputQuestionContent Question
        {
            get { return (InputQuestionContent)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register(nameof(Question), typeof(InputQuestionContent), typeof(InputQuestionViewer), new PropertyMetadata(null, OnQuestionChanged));

        public InputQuestionViewerViewModel Model
        {
            get { return (InputQuestionViewerViewModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(nameof(Model), typeof(InputQuestionViewerViewModel), typeof(InputQuestionViewer), new PropertyMetadata(null));

        private static void OnQuestionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (InputQuestionViewer)d;
            if (e.NewValue is InputQuestionContent question)
            {
                viewer.Model = new InputQuestionViewerViewModel(question);
            }
        }
    }
}
