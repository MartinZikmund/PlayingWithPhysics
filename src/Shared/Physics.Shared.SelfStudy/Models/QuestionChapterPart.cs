using Physics.Shared.SelfStudy.Models.Questions;
using System.Collections.Generic;

namespace Physics.Shared.SelfStudy.Models
{
    public class QuestionChapterPart : ChapterPart
    {
        public string Text { get; set; }

        public QuestionType Type { get; set; }

        public List<string> Options { get; set; }

        public override ChapterPartType PartType => ChapterPartType.Question;
    }
}
