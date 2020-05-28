using Physics.SelfStudy.Editor.ViewModels;
using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.ObjectModel;
using Windows.Devices.Bluetooth.Advertisement;

namespace Physics.SelfStudy.Models.Contents.Abstract
{
    public abstract class InputQuestionContent : ContentBase, IContent
    {
        public string Title { get; set; }

        public bool HasNumericInput { get; set; }

        public string Unit { get; set; }

        public string Placeholder { get; set; }

        public string Question { get; set; }

        public string CorrectResponse { get; set; }

        public string WrongResponse { get; set; }

        public ObservableCollection<string> AllowedAnswers { get; set; } = new ObservableCollection<string>();

        public double Tolerance { get; set; }
    }
}
