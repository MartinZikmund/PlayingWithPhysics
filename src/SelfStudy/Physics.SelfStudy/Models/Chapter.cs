using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.SelfStudy.Models
{
    public class Chapter
    {
        public string Name { get; set; }

        public List<ChapterPart> Contents { get; set; }

        public List<Chapter> Subchapters { get; set; }
    }
}
