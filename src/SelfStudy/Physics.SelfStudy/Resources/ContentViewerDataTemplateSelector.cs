using Physics.SelfStudy.Models.Contents.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Resources
{
    public class ContentViewerDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HtmlViewer { get; set; }

        protected override DataTemplate SelectTemplateCore(object item) =>
            GetTemplate(item);

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) =>
            GetTemplate(item);

        private DataTemplate GetTemplate(object item) =>
            item switch
            {
                HtmlContent html => HtmlViewer,
                null => null,
                _ => throw new NotImplementedException(),
            };
    }
}
