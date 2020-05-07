using Physics.SelfStudy.Editor.ViewModels;
using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.SelfStudy.Models.Contents
{
    public class Chapter : ContentBase
    {
        public string Title { get; set; }

        public ObservableCollection<IContent> Contents { get; } = new ObservableCollection<IContent>();

        public override ContentType Type => ContentType.Chapter;
    }
}