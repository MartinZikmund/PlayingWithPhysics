using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.SelfStudy.Models
{
    public interface IContent
    {
        string Title { get; }

        bool IsBrowsable { get; }

        ContentType Type { get; }

        ObservableCollection<IContent> Subcontents { get; }
    }
}
