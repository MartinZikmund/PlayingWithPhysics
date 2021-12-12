using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using MvvmCross.Binding.Binders;

namespace Physics.Shared.UI.Localization
{
	public class Localizer
	{
		private static ResourceLoader _resourceLoader = null;
		private static ResourceLoader _sharedResourceLoader = null;

		private Localizer()
		{
		}

		public static Localizer Instance { get; } = new Localizer();

		public string GetString(string key)
		{
			_resourceLoader ??= ResourceLoader.GetForCurrentView();
			try
			{
				var result = _resourceLoader.GetString(key ?? "");
				if (!string.IsNullOrEmpty(result))
				{
					return result;
				}

				// Fallback to shared resources
				_sharedResourceLoader ??= ResourceLoader.GetForCurrentView("Physics.Shared.UI/SharedResources");
				result = _sharedResourceLoader.GetString(key);
				if (!string.IsNullOrEmpty(result))
				{
					return result;
				}
			}
			catch (Exception)
			{
				// This condition only happens for empty key (design time issue)
			}

			return "?????" + key + "?????";
		}

		public string this[string key] => GetString(key);
	}
}
