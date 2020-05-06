using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.SelfStudy.Models
{
    public interface IContent : INotifyPropertyChanged
    {
        string Title { get; set; }

        ContentType Type { get; }

        ObservableCollection<IContent> Subcontents { get; }
    }
}
