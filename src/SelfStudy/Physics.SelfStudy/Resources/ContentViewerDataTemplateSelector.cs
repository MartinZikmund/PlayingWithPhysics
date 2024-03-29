﻿using Physics.SelfStudy.Models.Contents;
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
        public DataTemplate ImageViewer { get; set; }

        public DataTemplate LaTeXViewer { get; set; }

        public DataTemplate HtmlViewer { get; set; }

        public DataTemplate InputQuestion { get; set; }

        public DataTemplate KnowledgeCheckViewer { get; set; }

        protected override DataTemplate SelectTemplateCore(object item) =>
            GetTemplate(item);

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) =>
            GetTemplate(item);

        private DataTemplate GetTemplate(object item) =>
            item switch
            {
                ImageContent image => ImageViewer,
                Models.Contents.Abstract.TextContent text when text.IsHtml => HtmlViewer,
                Models.Contents.Abstract.TextContent text when !text.IsHtml => LaTeXViewer,
                InputQuestionContent inputQuestion => InputQuestion,
                KnowledgeCheckContent knowledgeCheck => KnowledgeCheckViewer,
                null => null,
                _ => throw new NotImplementedException(),
            };
    }
}
