using Physics.SelfStudy.Editor.ViewModels;
using Physics.SelfStudy.Models.Contents.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Physics.SelfStudy.Viewers.ViewModels
{
    public class InputQuestionViewerViewModel : ViewModelBase
    {
        private readonly InputQuestionContent _question;

        public InputQuestionViewerViewModel(InputQuestionContent question)
        {
            _question = question;
        }

        public bool IsShowAnswerButtonEnabled => HasNumericInput || !string.IsNullOrEmpty(TextualInput);

        public string Placeholder => _question.Placeholder;

        public string Unit => _question.Unit;

        public double NumericInput { get; set; }

        public string TextualInput { get; set; }

        public bool HasNumericInput => _question.HasNumericInput;

        public bool HasTextualInput => !_question.HasNumericInput;

        public int SelectedAnswerIndex { get; set; } = -1;

        public bool ShowQuestion { get; set; } = true;

        public bool ShowCorrectAnswer { get; set; }

        public bool ShowWrongAnswer { get; set; }

        public bool ShowAnswer => ShowCorrectAnswer || ShowWrongAnswer;

        public string Response { get; set; }

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
            if (IsAnswerCorrect())
            {
                ShowCorrectAnswer = true;
                Response = _question.CorrectResponse;
            }
            else
            {
                ShowWrongAnswer = true;
                Response = !string.IsNullOrWhiteSpace(_question.WrongResponse) ? _question.WrongResponse : _question.CorrectResponse;
            }
        }

        private bool IsAnswerCorrect()
        {
            if(HasNumericInput)
            {
                //check tolerance against all entered allowed answers
                return _question.AllowedAnswers.Any(a =>
                {
                    if (double.TryParse(a.Trim(), out var answer))
                    {
                        return Math.Abs(answer - NumericInput) <= _question.Tolerance;
                    }
                    return false;
                });
            }
            else
            {
                return _question.AllowedAnswers.Any(a => string.Equals(a.Trim(), TextualInput, StringComparison.CurrentCultureIgnoreCase));
            }
        }
    }
}
