using Physics.Shared.UI.Localization;
using Windows.UI.Xaml.Markup;

namespace Physics.Shared.Extensions
{
	public class Localize : MarkupExtension
	{
		public string Key { get; set; }

		public bool Uppercase { get; set; } = false;

		public bool Lowercase { get; set; } = false;

		protected override object ProvideValue()
		{
			var value = Localizer.Instance.GetString(Key);
			if (Uppercase)
			{
				return value.ToUpper();
			}
			else if (Lowercase)
			{
				return value.ToLower();
			}

			return value;
		}
	}
}
