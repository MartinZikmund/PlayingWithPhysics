using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Physics.Shared.Mathematics
{
	public struct BigNumber
	{
		public BigNumber(double mantisa, int exponent)
		{
			Mantisa = mantisa;
			Exponent = exponent;
		}

		public BigNumber(double value)
		{
			var exponentialNotationString = value.ToString("E", CultureInfo.InvariantCulture);
			var split = exponentialNotationString.Split('E');
			Mantisa = double.Parse(split[0], CultureInfo.InvariantCulture);
			Exponent = int.Parse(split[1], CultureInfo.InvariantCulture);
		}

		public double Mantisa { get; }

		public int Exponent { get; }

		public static explicit operator double(BigNumber number) =>
			number.Mantisa * System.Math.Pow(10, number.Exponent);

		public static implicit operator BigNumber(double number) =>
			new BigNumber(number, 0);

		public static implicit operator BigNumber(float number) =>
			new BigNumber(number, 0);

		public static bool operator ==(BigNumber left, BigNumber right) =>
			(double)left == (double)right;

		public static bool operator !=(BigNumber left, BigNumber right) =>
			!((double)left == (double)right);

		public static BigNumber operator *(BigNumber a, BigNumber b) =>
			new BigNumber(a.Mantisa * b.Mantisa, a.Exponent + b.Exponent);

		public static BigNumber operator /(BigNumber a, BigNumber b) =>
			new BigNumber(a.Mantisa / b.Mantisa, a.Exponent - b.Exponent);

		public static BigNumber operator +(BigNumber a, BigNumber b)
		{
			if (a.Exponent < b.Exponent)
			{
				b = b.WithExponent(a.Exponent);
			}
			else
			{
				a = a.WithExponent(b.Exponent);
			}
			return new BigNumber(a.Mantisa + b.Mantisa, a.Exponent);
		}

		public static BigNumber operator -(BigNumber a, BigNumber b)
		{
			if (a.Exponent < b.Exponent)
			{
				b = b.WithExponent(a.Exponent);
			}
			else
			{
				a = a.WithExponent(b.Exponent);
			}
			return new BigNumber(a.Mantisa - b.Mantisa, a.Exponent);
		}

		public static bool operator >(BigNumber a, BigNumber b) =>
			(double)a > (double)b;

		public static bool operator <(BigNumber a, BigNumber b) =>
			(double)a < (double)b;

		public static bool operator >=(BigNumber a, BigNumber b) =>
			(double)a >= (double)b;

		public static bool operator <=(BigNumber a, BigNumber b) =>
			(double)a <= (double)b;

		public BigNumber WithExponent(int newExponent)
		{
			var newMantisa = Mantisa;
			if (Exponent > newExponent)
			{
				var diff = Exponent - newExponent;
				for (int i = 0; i < diff; i++)
				{
					newMantisa *= 10;
				}
			}
			else if (Exponent < newExponent)
			{
				var diff = newExponent - Exponent;
				for (int i = 0; i < diff; i++)
				{
					newMantisa /= 10;
				}
			}
			return new BigNumber(newMantisa, newExponent);
		}

		public BigNumber Normalize()
		{
			var mantisa = Mantisa;
			var exponent = Exponent;
			while (mantisa > 10)
			{
				mantisa /= 10;
				exponent++;
			}
			while (mantisa < 1 && mantisa > 0.0001)
			{
				mantisa *= 10;
				exponent--;
			}
			while (mantisa < 0 && mantisa > -1)
			{
				mantisa *= 10;
				exponent--;
			}
			while (mantisa < -10)
			{
				mantisa /= 10;
				exponent++;
			}

			return new BigNumber(mantisa, exponent);
		}

		public BigNumber Sqrt()
		{
			var mantisa = Mantisa;
			var exponent = Exponent;
			if (Exponent % 2 == 1)
			{
				mantisa /= 10;
				exponent++;
			}
			return new BigNumber(Math.Sqrt(mantisa), exponent / 2);
		}

		public override string ToString()
		{
			var doubleValue = (double)this;
			if (doubleValue.ToString("G", CultureInfo.InvariantCulture).IndexOf("E", StringComparison.InvariantCultureIgnoreCase) >= 0)
			{
				var normalized = Normalize();
				var exponent = normalized.Exponent;
				var rounded = Math.Round(normalized.Mantisa, 3);
				if (rounded >= 10 || rounded <= -10)
				{
					rounded /= 10;
					exponent++;
				}

				if (rounded == 0)
				{
					return "0";
				}

				var exponentRepresentation = string.Join("", normalized.Exponent.ToString().Select(c => SuperscriptNumberMap[c]));
				return $"{rounded.ToString("0.000")}×10{exponentRepresentation}";
			}
			else
			{
				return doubleValue.ToString("0.###");
			}
		}

		public string ToUpperIndexString()
		{
			return ToString("0");
		}

		public string ToSimplifiedUpperIndexString()
		{
			var normalized = Normalize();
			if (normalized.Exponent >= -3 && normalized.Exponent <= 3)
			{
				return ((double)normalized).ToString("0.###");
			}
			else
			{
				return ToString("0");
			}
		}

		static Dictionary<char, char> SuperscriptNumberMap = new()
		{
			{ '-', '⁻' },
			{ '0', '⁰' },
			{ '1', '¹' },
			{ '2', '²' },
			{ '3', '³' },
			{ '4', '⁴' },
			{ '5', '⁵' },
			{ '6', '⁶' },
			{ '7', '⁷' },
			{ '8', '⁸' },
			{ '9', '⁹' },
		};

		public string ToString(string formatString)
		{
			var normalized = Normalize();
			if (normalized.Mantisa == 0)
			{
				return 0.ToString(formatString);
			}
			var exponentRepresentation = string.Join("", normalized.Exponent.ToString().Select(c => SuperscriptNumberMap[c]));

			if (normalized.Mantisa == 1)
			{
				return $"10{exponentRepresentation}";
			}
			else
			{
				return $"{normalized.Mantisa.ToString(formatString)}×10{exponentRepresentation}";
			}
		}
	}
}
