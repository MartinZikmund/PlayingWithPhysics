using Physics.Shared.SelfStudy.Models;
using System.Collections.ObjectModel;

namespace Physics.SelfStudy.Models.Contents
{
    public class ChapterContent : IContent
    {
        public string Title { get; set; } = null!;

        public bool IsBrowsable => false;

        public ContentType Type => ContentType.Chapter;

        public ObservableCollection<IContent> Subcontents { get; } = new ObservableCollection<IContent>();
    }
}
