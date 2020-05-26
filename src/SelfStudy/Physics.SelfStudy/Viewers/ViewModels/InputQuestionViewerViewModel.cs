using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.SelfStudy.Viewers.ViewModels
{
    public class InputQuestionViewerViewModel
    {
        private readonly MultipleChoiceQuestionContent _question;

        public MultipleChoiceQuestionViewerViewModel(MultipleChoiceQuestionContent question)
        {
            _question = question;
        }

        public int SelectedAnswerIndex { get; set; } = -1;

        public bool IsShowAnswerButtonEnabled => SelectedAnswerIndex >= 0;

        public bool ShowQuestion { get; set; } = true;

        public bool ShowCorrectAnswer { get; set; }

        public bool ShowWrongAnswer { get; set; }

        public ICommand RevealAnswerCommand => GetOrCreateCommand(RevealAnswer);

        public ICommand TryAgainCommand => GetOrCreateCommand(TryAgain);

        private void TryAgain()
        {
            SelectedAnswerIndex = -1;
            ShowQuestion = true;
            ShowCorrectAnswer = false;
            ShowWrongAnswer = false;
        }

        private void RevealAnswer()
        {
            ShowQuestion = false;
            if (SelectedAnswerIndex == _question.CorrectAnswerIndex)
            {
                ShowCorrectAnswer = true;
            }
            else
            {
                ShowWrongAnswer = true;
            }
        }
    }
}
