using Physics.SelfStudy.Editor.ViewModels;
using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.SelfStudy.Models.Contents.Abstract
{
    public abstract class HtmlContent : ContentBase, IContent
    {
        public abstract ContentType Type { get; }

        public string Title { get; set; }

        public string Html { get; set; }

        public bool IsBrowsable => true;

        public ObservableCollection<IContent> Subcontents { get; } = new ObservableCollection<IContent>();
    }
}
