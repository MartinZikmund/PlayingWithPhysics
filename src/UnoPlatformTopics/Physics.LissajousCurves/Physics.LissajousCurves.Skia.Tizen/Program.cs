using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace Physics.LissajousCurves.Skia.Tizen
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var host = new TizenHost(() => new Physics.LissajousCurves.App());
			host.Run();
		}
	}
}
