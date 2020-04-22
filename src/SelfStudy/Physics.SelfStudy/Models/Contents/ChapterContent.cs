using Physics.SelfStudy.Editor.ViewModels;
using Physics.Shared.SelfStudy.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Physics.SelfStudy.Models.Contents
{
    public class ChapterContent : ContentBase, IContent, INotifyPropertyChanged
    {
        public string Title { get; set; } = null!;

        public bool IsBrowsable => false;

        public ContentType Type => ContentType.Chapter;

        public ObservableCollection<IContent> Subcontents { get; } = new ObservableCollection<IContent>();
    }
}
