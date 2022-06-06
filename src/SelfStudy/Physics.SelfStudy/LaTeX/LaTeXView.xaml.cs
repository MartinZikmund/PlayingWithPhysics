using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.LaTeX
{
	public sealed partial class LaTeXView : UserControl
	{
		private readonly DisplayInformation _displayInformation;

		public LaTeXView()
		{
			this.InitializeComponent();
			_displayInformation = DisplayInformation.GetForCurrentView();
			//View.Padding = new CSharpMath.Structures.Thickness(4);

			//LoadFont("Regular");
			//LoadSupplementFont("Italic");

			this.Loaded += LaTeXView_Loaded;
			this.Unloaded += LaTeXView_Unloaded;
			UpdateFontSize();
		}

		private static void LoadFont(string kind)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream($"Physics.SelfStudy.Assets.Fonts.arial.ttf"))
			{
				CSharpMath.Settings.GlobalTypefaces.AddOverride(new Typography.OpenFont.OpenFontReader().Read(stream));
			}
		}

		private static void LoadSupplementFont(string kind)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream($"Physics.SelfStudy.Assets.Fonts.OpenSans-{kind}.ttf"))
			{
				CSharpMath.Settings.GlobalTypefaces.AddSupplement(new Typography.OpenFont.OpenFontReader().Read(stream));
			}
		}

		private void LaTeXView_Loaded(object sender, RoutedEventArgs e)
		{
			_displayInformation.DpiChanged += DisplayInformation_DpiChanged;
		}

		private void LaTeXView_Unloaded(object sender, RoutedEventArgs e)
		{
			_displayInformation.DpiChanged -= DisplayInformation_DpiChanged;
		}

		private void UpdateFontSize()
		{
			var scale = (double)((int)_displayInformation.ResolutionScale / 100.0);
			View.FontSize = 15 * scale;
		}

		private void DisplayInformation_DpiChanged(DisplayInformation sender, object args) => UpdateFontSize();

		public string LaTeX
		{
			get { return (string)GetValue(LaTeXProperty); }
			set { SetValue(LaTeXProperty, value); }
		}

		// Using a DependencyProperty as the backing store for LaTeX.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LaTeXProperty =
			DependencyProperty.Register("LaTeX", typeof(string), typeof(LaTeXView), new PropertyMetadata("", OnLaTeXChanged));

		private static char[] _acute = new char[]{
			'á', 'ď', 'é', 'í', 'ó', 'ť', 'ú', 'ý'
		};

		private static char[] _caron = new char[]{
			'č', 'ě', 'ň', 'ř', 'š', 'ž'
		};

		private static char[] _ring = new char[]{
			'ů'
		};

		private static void OnLaTeXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var view = (LaTeXView)d;
			string text = e.NewValue?.ToString() ?? "";
			text = text.Replace("%", "\\%").Replace("\\\\%", "\\%"); //TODO: Ugly, fix this

			//text = AdjustCzechCharacters(text);

			view.View.LaTeX = text;
		}

		private static string AdjustCzechCharacters(string text)
		{
			foreach (var acuteLetter in _acute)
			{
				text = AdjustCharacter(text, acuteLetter, "'");
			}

			foreach (var ringLetter in _ring)
			{
				text = AdjustCharacter(text, ringLetter, "r");
			}

			foreach (var caronLetter in _caron)
			{
				text = AdjustCharacter(text, caronLetter, "v");
			}

			return text;
		}

		private static string AdjustCharacter(string text, char c, string command)
		{
			text = AdjustCasedCharacter(text, c, RemoveDiacritics(c.ToString()), command);
			text = AdjustCasedCharacter(text, char.ToUpper(c), RemoveDiacritics(char.ToUpper(c).ToString()), command);
			return text;
		}

		private static string AdjustCasedCharacter(string text, char c, string normalized, string command)
		{
			return text.Replace(c.ToString(), "\\" + command + "{" + normalized + "}");
		}

		private static string RemoveDiacritics(string text)
		{
			var normalizedString = text.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

			for (int i = 0; i < normalizedString.Length; i++)
			{
				char c = normalizedString[i];
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder
				.ToString()
				.Normalize(NormalizationForm.FormC);
		}		
	}
}
