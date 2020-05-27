﻿using Physics.SelfStudy.Models.Contents;
using Physics.SelfStudy.Models.Contents.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.SelfStudy.Editor.Resources
{
    public class EditorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HtmlContent { get; set; }

        public DataTemplate InputQuestion { get; set; }

        public DataTemplate KnowledgeCheck { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) => GetTemplateForItem(item);

        protected override DataTemplate SelectTemplateCore(object item) => GetTemplateForItem(item);

        private DataTemplate GetTemplateForItem(object item) =>
            item switch
            {
                HtmlContent html => HtmlContent,
                InputQuestionContent input => InputQuestion,
                KnowledgeCheckContent knowledgeCheck => KnowledgeCheck,
                null => null,
                _ => throw new NotImplementedException(),
            };        
    }
}
