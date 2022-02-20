using System;

namespace Physics.HuygensPrinciple.Logic
{
	public static class RenderSettingsDefaults
	{
		public const int DefaultFieldSize = 1080;

		public const int MinFieldSize = 300;
		public const int MaxFieldSize = 3000;

		public static float MinStepRadius => 4;

		public static float DefaultStepRadius => 33;

		public static float MaxStepRadius => 2 * DefaultStepRadius;

		public static float MinBrushSize = 4;

		public static float DefaultBrushSize => 1.1f * DefaultStepRadius;

		public static float MaxBrushSize => 1.5f * DefaultBrushSize;
	}
}
