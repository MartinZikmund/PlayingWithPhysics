using Physics.SelfStudy.Models.Contents;
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

        public DataTemplate Chapter { get; set; }

        protected override DataTemplate SelectTemplateCore(object item) =>
            item switch
            {
                ChapterContent _ => Chapter,
                AdditionalResourcesContent _ => AdditionalResources,
                KnowledgeCheckContent _ => KnowledgeCheck,
                LiteratureContent _ => Literature,
                RealWorldContent _ => RealWorld,
                TasksContent _ => Tasks,
                ToRememberContent _ => ToRemember,
                null => null,
                _ => throw new NotImplementedException(),
            };
    }
}
