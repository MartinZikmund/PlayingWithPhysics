using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Resources
{
    public class ChapterTreeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AdditionalResources { get; set; }

        public DataTemplate KnowledgeCheck { get; set; }

        public DataTemplate Literature { get; set; }

        public DataTemplate RealWorld { get; set; }

        public DataTemplate Tasks { get; set; }

        public DataTemplate ToRemember { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return AdditionalResources;
        }
    }
}
