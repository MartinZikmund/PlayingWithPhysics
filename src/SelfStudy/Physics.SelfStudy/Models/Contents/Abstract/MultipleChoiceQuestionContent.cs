using Physics.SelfStudy.Editor.ViewModels;
using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.SelfStudy.Models.Contents.Abstract
{
    public class MultipleChoiceQuestionContent : ContentBase, IContent
    {
        public override ContentType Type { get; }

        public string Title { get; set; }

        public string Question { get; set; }

        public string CorrectResponse { get; set; }

        public string WrongResponse { get; set; }

        public int CorrectOptionIndex { get; set; }

        public ObservableCollection<string> Options { get; set; } = new ObservableCollection<string>();
    }
}
