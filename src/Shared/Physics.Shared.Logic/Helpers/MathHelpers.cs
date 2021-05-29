using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Helpers
{
	public static class MathHelpers
	{
		public static float DegreesToRadians(float angleInDegrees) => (float)Math.PI * angleInDegrees / 180.0f;

		public static float RadiansToDegrees(float angleInRadians) => angleInRadians * 180f / (float)Math.PI;

		public static bool AlmostEqualTo(this float number, float otherNumber, float epsilon = 0.00001f)
		{
			return Math.Abs(number - otherNumber) < epsilon;
		}

		public static double Clamp(double value, double min, double max)
		{
			if (value < min)
			{
				return min;
			}

			if (value > max)
			{
				return max;
			}

			return value;
		}
	}
}
