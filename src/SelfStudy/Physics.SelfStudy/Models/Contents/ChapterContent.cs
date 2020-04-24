using Physics.SelfStudy.Editor.ViewModels;
using Physics.SelfStudy.Models.Contents.Abstract;
using Physics.Shared.SelfStudy.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Physics.SelfStudy.Models.Contents
{
    public class ChapterContent : HtmlContent
    {
        public override ContentType Type => ContentType.Chapter;

        public ObservableCollection<IContent> Subcontents { get; } = new ObservableCollection<IContent>();
    }
}
