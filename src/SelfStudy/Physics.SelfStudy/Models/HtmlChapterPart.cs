namespace Physics.Shared.SelfStudy.Models
{
    public class HtmlChapterPart : ChapterPart
    {
        public string Html { get; set; }

        public override ChapterPartType PartType => ChapterPartType.Html;
    }
}
