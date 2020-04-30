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
        private static ResourceLoader _sharedResourceLoader = null;

        public string Key { get; set; }

        protected override object ProvideValue()
        {
            _resourceLoader ??= ResourceLoader.GetForCurrentView();
            var result = _resourceLoader.GetString(Key);
            if (string.IsNullOrEmpty(result))
            {
                // use shared resources
                _sharedResourceLoader ??= ResourceLoader.GetForCurrentView("Physics.Shared/SharedResources");
                result = _sharedResourceLoader.GetString(Key);
                if (string.IsNullOrEmpty(result))
                {
                    result = "?????" + Key + "?????";
                }
            }
            return result;
        }
    }
}
