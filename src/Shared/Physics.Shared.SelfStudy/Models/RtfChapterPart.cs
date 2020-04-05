namespace Physics.Shared.SelfStudy.Models
{
    public class RtfChapterPart : ChapterPart
    {
        public string Rtf { get; set; }

        public override ChapterPartType PartType => ChapterPartType.Rtf;
    }
}
