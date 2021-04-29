using System;

namespace Physics.Shared.Extensions
{
	public static class StringExtensions
	{
		public static bool EqualsInvariantIgnoreCase(this string source, string target)
		{
			if (source == null)
			{
				return target == null;
			}

			return source.Equals(target, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
