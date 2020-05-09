using Physics.Shared.UI.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Markup;

namespace Physics.Shared.Extensions
{
    public class Localize : MarkupExtension
    {
        public string Key { get; set; }

        protected override object ProvideValue() => Localizer.Instance.GetString(Key);
    }
}
