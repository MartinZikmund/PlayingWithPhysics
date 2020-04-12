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
        private static ResourceLoader _resourceLoader = null;

        public string Key { get; set; }

        protected override object ProvideValue()
        {
            _resourceLoader ??= ResourceLoader.GetForCurrentView();
            return _resourceLoader.GetString(Key);
        }
    }
}
