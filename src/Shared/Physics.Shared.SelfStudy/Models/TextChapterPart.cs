using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.SelfStudy.Models
{
    public class TextChapterPart : ChapterPart
    {
        public override ChapterPartType PartType => ChapterPartType.Text;
        
        public string Text { get; set; }
    }
}
