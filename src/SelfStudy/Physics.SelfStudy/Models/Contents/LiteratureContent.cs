﻿using Physics.SelfStudy.Models.Contents.Abstract;
using Physics.Shared.SelfStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.SelfStudy.Models.Contents
{
    public class LiteratureContent : HtmlContent
    {
        public override ContentType Type => ContentType.Literature;
    }
}
