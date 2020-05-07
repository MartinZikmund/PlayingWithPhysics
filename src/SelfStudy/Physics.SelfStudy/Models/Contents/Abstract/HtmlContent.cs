﻿using Physics.SelfStudy.Editor.ViewModels;
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
        public string Title { get; set; }

        public string Html { get; set; }
    }
}
