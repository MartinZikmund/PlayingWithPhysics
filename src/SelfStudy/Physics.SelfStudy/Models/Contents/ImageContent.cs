using Physics.SelfStudy.Editor.ViewModels;
using Physics.Shared.SelfStudy.Models;

namespace Physics.SelfStudy.Models.Contents
{
    public class ImageContent : ContentBase, IContent
    {
        public override ContentType Type => ContentType.Image;

        public string ImageName { get; set; }

        public string Title { get; set; } = "Image";
    }
}
