using System;
using System.Linq;

namespace Physics.Shared.UI.Helpers
{
	public static class UniqueNameGenerator
    {
        public static string Generate(string prefix, string[] existingNames)
		{
			var currentId = 0;

			string generatedName;
			do
			{
				generatedName = $"{prefix} #{++currentId}";
			} while (existingNames.Any(name => generatedName.Equals(name, StringComparison.InvariantCultureIgnoreCase)));

			return generatedName;
		}
    }
}
