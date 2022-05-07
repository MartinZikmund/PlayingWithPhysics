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

		public static double MetersToAstronomicalUnits(double meters) => meters * 6.684589e-12;

		public static double AstronomicalUnitsToMeters(double astronomicalUnits) => astronomicalUnits / 6.684589e-12;

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

		public static float Clamp(float value, float min, float max)
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

		public static double Atanh(double x)
		{
			return (Math.Log(1 + x) - Math.Log(1 - x)) / 2;
		}

		public static double Acosh(double x)
		{
			return Math.Log(Math.Sqrt(x * x - 1.0d) + x);
		}
	}
}
