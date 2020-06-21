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
    public class ChapterListDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AdditionalResources { get; set; }

        public DataTemplate KnowledgeCheck { get; set; }

        public DataTemplate Literature { get; set; }

        public DataTemplate RealWorld { get; set; }

        public DataTemplate Tasks { get; set; }

        public DataTemplate ToRemember { get; set; }

        public DataTemplate Text { get; set; }

        public DataTemplate Image { get; set; }

        protected override DataTemplate SelectTemplateCore(object item) =>
            item switch
            {
                ParagraphContent _ => Text,
                AdditionalResourcesContent _ => AdditionalResources,
                KnowledgeCheckContent _ => KnowledgeCheck,
                LiteratureContent _ => Literature,
                RealWorldContent _ => RealWorld,
                TasksContent _ => Tasks,
                ImageContent _ => Image,
                ToRememberContent _ => ToRemember,
                null => null,
                _ => throw new NotImplementedException(),
            };

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) => 
            SelectTemplateCore(item);
    }
}
