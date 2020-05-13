using Physics.Shared.UI.Localization;
using Windows.UI.Xaml.Markup;

namespace Physics.Shared.Extensions
{
    public class Localize : MarkupExtension
    {
        public string Key { get; set; }

        protected override object ProvideValue() => Localizer.Instance.GetString(Key);
    }
}
